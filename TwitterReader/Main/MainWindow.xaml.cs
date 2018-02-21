using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TwitterReader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {            
            App.MainViewModel.Initialize();
            DataContext = App.MainViewModel;

            // Note: ImageBrush ProfileImageUrl binding can take a while, hence the delay.
            //       Increase the delay if needed.
            await Task.Delay(1000);
            TreeView.InvalidateVisual();
        }
    }
}
