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

namespace Runge_Kutta.Pages
{
    /// <summary>
    /// Логика взаимодействия для HardThing.xaml
    /// </summary>
    public partial class HardThing : Page
    {
        public HardThing()
        {
            InitializeComponent();
        }

        private void bDraw_Click(object sender, RoutedEventArgs e)
        {
            List<double[]> ret = new();
            try
            {
                double x = (-Math.Sqrt(3) / 2) * Convert.ToDouble(Mu.Text);
                double endX = x + Convert.ToDouble(H.Text) * Convert.ToDouble(CountH.Text);
                ret = Equations.ReshenieComplex(Convert.ToDouble(Delta.Text), Convert.ToDouble(F.Text), Convert.ToDouble(Mu.Text), Convert.ToDouble(Teta.Text), x, endX, Convert.ToDouble(H.Text));
                
                MainWindow.mwMainCanvas.Children.Clear();
                MainWindow.mwMainTextBox.Text = "";

                MainWindow.DrawPoints(ret, new Brush[] { Brushes.Red, Brushes.Blue, Brushes.Black }, "у.е.", 3);

                string str = "";

                foreach (double[] d in ret)
                {
                    str += Convert.ToString(Math.Round(d[0], 6) + ";" + Math.Round(d[1], 6) + ";" + Math.Round(d[2], 6) + ";" + Math.Round(d[3], 6) + "\n");
                }

                MainWindow.mwMainTextBox.Text = str;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Одно из чисел введено некорректно");
            }
            
        }


        private void RBF1_Checked(object sender, RoutedEventArgs e)
        {
            if (RBF1.IsChecked == true)
            {
                function = 1;
                return;
            }
            else if(RBF2.IsChecked == true)
            {
                function = 2;
                return;
            }
            else if (RBF3.IsChecked == true)
            {
                function = 3;
                return;
            }
            else if(RBF4.IsChecked == true)
            {
                function = 4;
            }
        }
    }
}
