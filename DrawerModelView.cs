using ScottPlot.WPF;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Test
{
    public class DrawerModelView : INotifyPropertyChanged
    {
        public GeneratorSinModel generatorSin { get; }
        private CancellationTokenSource? cancellationTokenSource;
        private WpfPlot wpfPlot;
        private bool isStarting;

        public ObservableCollection<double> Data => generatorSin.Data;

        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public DrawerModelView(GeneratorSinModel generatorSin, WpfPlot wpfPlot)
        {
            this.generatorSin = generatorSin;
            this.wpfPlot = wpfPlot;
            StartCommand = new Command(StartDrawing);
            StopCommand = new Command(StopDrawing);
            this.wpfPlot.Plot.YLabel("Амлитуда");
            this.wpfPlot.Plot.XLabel("Время (мс)");
        }

        public async void StartDrawing()
        {
            if (isStarting == false)
            {
                isStarting = true;
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                try
                {
                    await Task.WhenAll(generatorSin.GenerateData(token), DrawData(token));
                }
                catch (OperationCanceledException) { }
            }
        }

        public void StopDrawing()
        {
            if (isStarting == true)
            {
                isStarting = false;
                cancellationTokenSource?.Cancel();
            }
        }

        private async Task DrawData(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wpfPlot.Plot.Clear();
                    wpfPlot.Plot.Add.Signal(generatorSin.Data.ToList());

                    double xMax = generatorSin.Data.Count;
                    double xMin = xMax - 100;

                    wpfPlot.Plot.Axes.SetLimitsX(xMin, xMax);
                    wpfPlot.Plot.Axes.SetLimitsY(bottom: generatorSin.Amplitude + 1, top: generatorSin.Amplitude + 1);
                    wpfPlot.Refresh();
                });
                try { await Task.Delay(10, token); }
                catch (TaskCanceledException) { break; }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}