using System.Windows;

namespace Test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneratorSinModel generatorSin;
        private DrawerModelView drawer;

        public MainWindow()
        {
            InitializeComponent();
            generatorSin = new GeneratorSinModel();
            drawer = new DrawerModelView(generatorSin, SinusoidPlot);
            DataContext = drawer;
        }
    }
}
