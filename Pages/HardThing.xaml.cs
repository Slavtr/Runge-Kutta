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
            MainWindow.mwMainCanvas.Children.Clear();
            MainWindow.mwMainTextBox.Text = "";
            List<double[]> ret = Equations.ReshenieComplex(Convert.ToDouble(Delta.Text), Convert.ToDouble(F.Text), Convert.ToDouble(Mu.Text), Convert.ToDouble(Teta.Text), Convert.ToDouble(StartX.Text), Convert.ToDouble(EndX.Text), Convert.ToDouble(H.Text), function);
            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i][0] >= double.MinValue && ret[i][1] <= double.MaxValue)
                {
                    Point p = new Point(ret[i][1] * 300, MainWindow.mwMainCanvas.ActualHeight - ret[i][0] * 300);
                    Ellipse ell = new Ellipse();

                    ell.Width = 4;
                    ell.Height = 4;

                    ell.StrokeThickness = 2;
                    ell.Stroke = Brushes.Black;
                    ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                    MainWindow.mwMainCanvas.Children.Add(ell);
                    MainWindow.mwMainTextBox.Text += Convert.ToString(ret[i][0] + ";" + ret[i][1] + "\n");
                    if (i != 0)
                    {
                        Line l = new Line();
                        l.X1 = ret[i - 1][1] * 300;
                        l.Y1 = MainWindow.mwMainCanvas.ActualHeight - ret[i - 1][0] * 300;
                        l.X2 = ret[i][1] * 300;
                        l.Y2 = MainWindow.mwMainCanvas.ActualHeight - ret[i][0] * 300;

                        l.StrokeThickness = 1;
                        l.Stroke = Brushes.Black;
                        MainWindow.mwMainCanvas.Children.Add(l);
                    }
                }
            }
            double[] module = ret.Where(x=> x[0] >= double.MinValue && x[1] <= double.MaxValue).Last();
            string str = "Модуль числа равен: " + Convert.ToString(Math.Sqrt(Math.Pow(module[0], 2) + Math.Pow(module[1], 2)));
            MessageBox.Show(str);
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
