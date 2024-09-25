using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    public class GeneratorSinModel : INotifyPropertyChanged
    {
        private double amplitude = 1;
        private double frequency = 1;
        private double index = 0;

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

        public event PropertyChangedEventHandler? PropertyChanged;

        public GeneratorSinModel()
        {
            Data = new ObservableCollection<double>();
        }

        public async Task GenerateData(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Data.Add(amplitude * Math.Sin(frequency * index * 0.1));
                index++;
                try { await Task.Delay(10, token); }
                catch (TaskCanceledException) { break; }
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
