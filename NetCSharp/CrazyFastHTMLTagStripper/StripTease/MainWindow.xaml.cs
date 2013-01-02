using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StripTease.Bouncer;
using StripTease.Dancers;
using StripTease.ViewModel;

namespace StripTease
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            MainViewModel viewModel = new MainViewModel();
            base.DataContext = viewModel;
         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        
    }
}
