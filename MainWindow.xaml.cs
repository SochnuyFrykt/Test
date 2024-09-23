using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneratorSinModel generator;
        private DrawerModelView drawer;

        public MainWindow()
        {
            InitializeComponent();
            generator = new GeneratorSinModel();
            drawer = new DrawerModelView(generator, SinusoidPlot);
            DataContext = drawer;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(amplitudeInput.Text, out double amplitude) &&
                double.TryParse(frequencyInput.Text, out double frequency))
            {
                generator.Amplitude = amplitude;
                generator.Frequency = frequency;
                generator.StartGenerating();
                drawer.StartDrawing();
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            generator.StopGenerating();
            drawer.StopDrawing();
        }

        private void amplitudeInput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(amplitudeInput.Text, out double amplitude))
                generator.Amplitude = amplitude;
        }

        private void frequencyInput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(frequencyInput.Text, out double frequency))
                generator.Frequency = frequency;
        }
    }
}
