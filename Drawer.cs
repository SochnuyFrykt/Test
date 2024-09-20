using ScottPlot.WPF;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Test
{
    internal class Drawer
    {
        private WpfPlot wpfPlot;
        private GeneratorSin dataGenerator;

        public Drawer(WpfPlot wpfPlot, GeneratorSin dataGenerator)
        {
            this.wpfPlot = wpfPlot;
            this.dataGenerator = dataGenerator;
        }

        public async Task plotDrawerAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    wpfPlot.Plot.Clear();
                    wpfPlot.Plot.Add.Signal(dataGenerator.Data.ToArray());

                    double xCenter = dataGenerator.Data.Count - 50;
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
