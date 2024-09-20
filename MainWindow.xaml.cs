using ScottPlot;
using ScottPlot.TickGenerators;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneratorSin generatorSin;
        private Drawer plotDrawer;
        private CancellationTokenSource cancellationTokenSource;

        public MainWindow()
        {
            InitializeComponent();

            SinusoidPlot.Plot.YLabel("Амлитуда");
            SinusoidPlot.Plot.XLabel("Время (мс)");
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            double amplitude, frequency;
            try
            {
                amplitude = double.Parse(amplitudeInput.Text);
                frequency = double.Parse(frequencyInput.Text);
                if (generatorSin == null)
                {
                    generatorSin = new GeneratorSin(amplitude, frequency);
                    plotDrawer = new Drawer(SinusoidPlot, generatorSin);
                }
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                try
                {
                    await Task.WhenAll(generatorSin.GenerateDataAsync(token), plotDrawer.plotDrawerAsync(token));
                }
                catch (OperationCanceledException) { }
            }
            catch
            {
                MessageBox.Show(
                    "Пожалуйста, введите корректные значения для амплитуды и частоты.",
                    "Ошибка ввода",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
