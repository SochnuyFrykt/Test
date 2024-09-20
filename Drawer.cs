using ScottPlot.WPF;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Test
{
    internal class Drawer
    {
        private WpfPlot wpfPlot;
        private GeneratorSin generatorSin;

        public Drawer(WpfPlot wpfPlot, GeneratorSin generatorSin)
        {
            this.wpfPlot = wpfPlot;
            this.generatorSin = generatorSin;
        }

        public async Task plotDrawerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wpfPlot.Plot.Clear();
                    wpfPlot.Plot.Add.Signal(generatorSin.Data.ToArray());

                    double xCenter = generatorSin.Data.Count - 50;
                    double xMin = xCenter - 50;
                    double xMax = xCenter + 150;

                    wpfPlot.Plot.Axes.SetLimitsX(xMin, xMax);

                    wpfPlot.Refresh();
                });
                try
                {
                    await Task.Delay(60, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
}
