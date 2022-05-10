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
        private static Dictionary<string, Brush> _brushes = new Dictionary<string, Brush>() { { "Red", Brushes.Red }, { "Blue", Brushes.Blue }, { "Black", Brushes.Black } };
        public HardThing()
        {
            InitializeComponent();
            cbDeistv.ItemsSource = _brushes;
            cbMnim.ItemsSource = _brushes;
            cbModule.ItemsSource = _brushes;
        }

        private void bDraw_Click(object sender, RoutedEventArgs e)
        {
            List<double[]> ret;
            try
            {
                
                ret = Equations.ReshenieComplex(Convert.ToDouble(Delta.Text.Replace(".", ",")), Convert.ToDouble(F.Text.Replace(".", ",")), Convert.ToDouble(Mu.Text.Replace(".", ",")), Convert.ToDouble(Teta.Text.Replace(".", ",")), Convert.ToDouble(H.Text.Replace(".", ",")));
                
                MainWindow.mwMainCanvas.Children.Clear();
                MainWindow.mwMainTextBox.Text = "";

                MainWindow.DrawPoints(ret, new Brush[] {((KeyValuePair<string, Brush>)cbDeistv.SelectedItem).Value, ((KeyValuePair<string, Brush>)cbMnim.SelectedItem).Value, ((KeyValuePair<string, Brush>)cbModule.SelectedItem).Value}, "у.е.", 3);

                string str = "";

                foreach (double[] d in ret)
                {
                    str += Convert.ToString(Math.Round(d[0], 6) + "; " + Math.Round(d[1], 6) + "; " + Math.Round(d[2], 6) + "; " + Math.Round(d[3], 6) + "\n");
                }

                MainWindow.mwMainTextBox.Text = str;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Одно из чисел введено некорректно");
            }
            
        }
    }
}
