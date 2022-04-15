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
        private int function = 1;
        public HardThing()
        {
            InitializeComponent();
        }

        private void bDraw_Click(object sender, RoutedEventArgs e)
        {
            List<double[]> ret = new();
            try
            {
                ret = Equations.ReshenieComplex(Convert.ToDouble(Delta.Text), Convert.ToDouble(F.Text), Convert.ToDouble(Mu.Text), Convert.ToDouble(Teta.Text), Convert.ToDouble(StartX.Text), Convert.ToDouble(EndX.Text), Convert.ToDouble(H.Text), function);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Одно из чисел введено некорректно");
            }
            List<double[]> f1 = ConvertToLine(ret, 3, 0);
            List<double[]> f2 = ConvertToLine(ret, 3, 1);
            List<double[]> module = ConvertToLine(ret, 3, 2);

            MainWindow.mwMainCanvas.Children.Clear();
            MainWindow.mwMainTextBox.Text = "";

            MainWindow.DrawPoints(f1, Convert.ToDouble(H.Text), Brushes.Red);
            MainWindow.DrawPoints(f2, Convert.ToDouble(H.Text), Brushes.Blue);
            MainWindow.DrawPoints(module, Convert.ToDouble(H.Text), Brushes.Black);

            string str = "";

            foreach(double[] d in ret)
            {
                str += Convert.ToString(Math.Round(d[0], 6) + ";" + Math.Round(d[1], 6) + ";" + Math.Round(d[2], 6) + ";" + Math.Round(d[3], 6) + "\n");
            }

            MainWindow.mwMainTextBox.Text = str;
        }

        private List<double[]> ConvertToLine(List<double[]> input, int numberX, int numberY)
        {
            List<double[]> ret = new List<double[]>();
            for(int i = 0; i<input.Count; i++)
            {
                if(Math.Abs(input[i][numberX]) >= double.MaxValue || Math.Abs(input[i][numberY]) >= double.MaxValue)
                {
                    ret.RemoveAt(i - 1);
                    return ret; 
                }
                ret.Add(new double[] { input[i][numberX], input[i][numberY] });
            }
            return ret;
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
