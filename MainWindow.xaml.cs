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

namespace Runge_Kutta
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Canvas mwMainCanvas;
        public static TextBox mwMainTextBox;
        public static Frame mwMainFrame;
        private static List<Page> pages;
        public MainWindow()
        {
            InitializeComponent();
            mwMainCanvas = MainCanvas;
            mwMainTextBox = TBPoints;
            mwMainFrame = MainFrame;
            pages = new List<Page>();
            pages.Add(new Pages.Runge_Kutta());
            pages.Add(new Pages.HardThing());
            MainFrame.Content = pages[0];
        }

        private void RK_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = pages[0];
        }

        private void Hard_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = pages[1];
        }
    }
}
