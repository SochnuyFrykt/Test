using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

public class GeneratorSinModel : INotifyPropertyChanged
{
    private double amplitude;
    private double frequency;
    private bool isGenerating;
    private double index = 0;
    private CancellationTokenSource cancellationTokenSource;
    public ObservableCollection<double> Data { get; private set; }

    public double Amplitude
    {
        get => amplitude;
        set
        {
            amplitude = value;
            OnPropertyChanged(nameof(Amplitude));
        }
    }

    public double Frequency
    {
        get => frequency;
        set
        {
            frequency = value;
            OnPropertyChanged(nameof(Frequency));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public GeneratorSinModel()
    {
        Data = new ObservableCollection<double>();
    }

    public void StartGenerating()
    {
        if (isGenerating) return;

        isGenerating = true;
        cancellationTokenSource = new CancellationTokenSource();
        Task.Run(() => GenerateData(cancellationTokenSource.Token));
    }

    public void StopGenerating()
    {
        if (!isGenerating) return;

        isGenerating = false;
        cancellationTokenSource.Cancel();
    }

    private async Task GenerateData(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Data.Add(amplitude * Math.Sin(frequency * index * 0.1));
            index++;
            try
            {
                await Task.Delay(10, token);
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
