using System.Threading;
using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneratorSin dataGenerator;
        private Drawer plotUpdater;
        private Thread dataThread;
        private Thread plotThread;
        private bool isCreateThead = false;

        public MainWindow()
        {
            InitializeComponent();

            SinusoidPlot.Plot.YLabel("Амлитуда");
            SinusoidPlot.Plot.XLabel("Время (мс)");
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            double amplitude, frequency;
            try
            {
                amplitude = double.Parse(amplitudeInput.Text);
                frequency = double.Parse(frequencyInput.Text);
                dataGenerator = new GeneratorSin(amplitude, frequency);
                plotUpdater = new Drawer(SinusoidPlot, dataGenerator);
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
            
            if (isCreateThead == false)
            {
                dataThread = new Thread(new ThreadStart(dataGenerator.Generate));
                dataThread.IsBackground = true;
                plotThread = new Thread(new ThreadStart(plotUpdater.UpdatePlot));
                plotThread.IsBackground = true;
                isCreateThead = true;
            }

            Start();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void Start()
        {
            dataThread.Start();
            plotThread.Start();
        }

        private void Stop()
        {
            dataThread.Join();
            plotThread.Join();
        }
    }
}
