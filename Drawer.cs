using ScottPlot.WPF;
using System.Threading;
using System.Windows;

namespace Test
{
    internal class Drawer
    {
        private WpfPlot wpfPlot;
        private GeneratorSin dataGenerator;
        private bool isRunning = true;

        public Drawer(WpfPlot wpfPlot, GeneratorSin dataGenerator)
        {
            this.wpfPlot = wpfPlot;
            this.dataGenerator = dataGenerator;
        }

        public void UpdatePlot()
        {
            while (true)
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
                Thread.Sleep(50);
            }
        }

        public void Stop()
        {
            isRunning = false;
        }
    }
}
