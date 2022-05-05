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
    /// Логика взаимодействия для Runge_Kutta.xaml
    /// </summary>
    public partial class Runge_Kutta : Page
    {
        public Runge_Kutta()
        {
            InitializeComponent();
        }
        private void bDraw_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(MainTextBox.Text) || string.IsNullOrWhiteSpace(MainTextBox.Text))
            {
                MessageBox.Show("Уравнение не введено");
                return;
            }
            if (string.IsNullOrEmpty(StartX.Text) || string.IsNullOrWhiteSpace(StartX.Text))
            {
                MessageBox.Show("Начальное число X не введено");
                return;
            }
            if (string.IsNullOrEmpty(EndX.Text) || string.IsNullOrWhiteSpace(EndX.Text))
            {
                MessageBox.Show("Конечное число X не введено");
                return;
            }
            if (string.IsNullOrEmpty(StartY.Text) || string.IsNullOrWhiteSpace(StartY.Text))
            {
                MessageBox.Show("Начальное число Y не введено");
                return;
            }
            try
            {
                List<double[]> ret = Equations.ReshenieRegular(Convert.ToDouble(StartX.Text.Replace(".", ",")), Convert.ToDouble(EndX.Text.Replace(".", ",")), Convert.ToDouble(StartY.Text.Replace(".", ",")), Convert.ToDouble(H.Text.Replace(".", ",")), MainTextBox.Text.Replace(".", ","));
                MainWindow.mwMainCanvas.Children.Clear();
                MainWindow.mwMainTextBox.Text = "";
                MainWindow.DrawPoints(ret, Brushes.Black, "у.е.");

                string str = "";

                foreach (double[] d in ret)
                {
                    str += Convert.ToString(Math.Round(d[0], 6) + "; " + Math.Round(d[1], 6) + "\n");
                }

                MainWindow.mwMainTextBox.Text = str;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
