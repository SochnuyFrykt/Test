using ScottPlot.WPF;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

public class DrawerModelView : INotifyPropertyChanged
{
    private GeneratorSinModel generatorSin;
    private bool isDrawing;
    private CancellationTokenSource cancellationTokenSource;
    private WpfPlot wpfPlot;

    public ObservableCollection<double> Data => generatorSin.Data;

    public event PropertyChangedEventHandler PropertyChanged;

    public DrawerModelView(GeneratorSinModel generatorSin, WpfPlot wpfPlot)
    {
        this.generatorSin = generatorSin;
        this.wpfPlot = wpfPlot;
        this.wpfPlot.Plot.YLabel("Амлитуда");
        this.wpfPlot.Plot.XLabel("Время (мс)");
    }

    public void StartDrawing()
    {
        if (isDrawing) return;

        isDrawing = true;
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => DrawData(cancellationTokenSource.Token));
    }

    public void StopDrawing()
    {
        if (!isDrawing) return;

        isDrawing = false;
        cancellationTokenSource.Cancel();
    }

    private async Task DrawData(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                wpfPlot.Plot.Clear();
                //double[] dataArray = generatorSin.Data.ToArray();
                wpfPlot.Plot.Add.Signal(generatorSin.Data.ToList());

                double xMax = generatorSin.Data.Count;
                double xMin = xMax - 100;

                wpfPlot.Plot.Axes.SetLimitsX(xMin, xMax);
                wpfPlot.Plot.Axes.SetLimitsY(bottom: generatorSin.Amplitude + 1, top: generatorSin.Amplitude + 1);
                wpfPlot.Refresh();
            });
            try
            {
                await Task.Delay(11, token);
            }
            catch (TaskCanceledException)
            {
                break;
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
