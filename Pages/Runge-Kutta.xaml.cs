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
            MainWindow.mwMainCanvas.Children.Clear();
            MainWindow.mwMainTextBox.Text = "";
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
                double[,] ret = Equations.ReshenieRegular(Convert.ToDouble(StartX.Text), Convert.ToDouble(EndX.Text), Convert.ToDouble(StartY.Text), Convert.ToDouble(H.Text), MainTextBox.Text);
                for (int i = 0; i < ret.GetLength(0); i++)
                {
                    Point p = new Point(ret[i, 1] * 100, MainWindow.mwMainCanvas.ActualHeight - ret[i, 0] * 100);
                    Ellipse ell = new Ellipse();

                    ell.Width = 4;
                    ell.Height = 4;

                    ell.StrokeThickness = 2;
                    ell.Stroke = Brushes.Black;
                    ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                    MainWindow.mwMainCanvas.Children.Add(ell);
                    MainWindow.mwMainTextBox.Text += Convert.ToString(ret[i, 0] + ";" + ret[i, 1] + "\n");
                    if (i != 0)
                    {
                        Line l = new Line();
                        l.X1 = ret[i - 1, 1] * 100;
                        l.Y1 = MainWindow.mwMainCanvas.ActualHeight - ret[i - 1, 0] * 100;
                        l.X2 = ret[i, 1] * 100;
                        l.Y2 = MainWindow.mwMainCanvas.ActualHeight - ret[i, 0] * 100;

                        l.StrokeThickness = 1;
                        l.Stroke = Brushes.Black;
                        MainWindow.mwMainCanvas.Children.Add(l);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}
