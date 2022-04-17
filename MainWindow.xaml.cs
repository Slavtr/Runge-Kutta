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

        private static double[] UE(List<double[]> ret, double dimention1, double dimention2, double h)
        {
            double[] d = new double[2];

            foreach (double[] d2 in ret)
            {
                if (d[0] <= Math.Abs(d2[0]) && d[0] <= double.MaxValue && d[0] >= double.MinValue)
                {
                    d[0] = Math.Abs(d2[0]);
                }
                if (d[1] <= Math.Abs(d2[1]) && d[1] <= double.MaxValue && d[1] >= double.MinValue)
                {
                    d[1] = Math.Abs(d2[1]);
                }
            }

            d[0] = dimention1 / d[0];
            d[1] = dimention2 / d[1];
            if (d[1] >= double.MaxValue)
            {
                d[1] = dimention2;
            }

            return d;
        }

        public static void DrawCoordinates(double h)
        {
            double centerw = mwMainCanvas.ActualWidth / 2, centerh = mwMainCanvas.ActualHeight / 2;
            Line l = new Line();

            l.StrokeThickness = 0.5;
            l.Stroke = Brushes.Black;

            l.X1 = centerw;
            l.Y1 = 0;
            l.X2 = centerw;
            l.Y2 = mwMainCanvas.ActualHeight;
            mwMainCanvas.Children.Add(l);

            l = new Line();

            l.X1 = 0;
            l.Y1 = centerh;
            l.X2 = mwMainCanvas.ActualWidth;
            l.Y2 = centerh;

            l.StrokeThickness = 0.5;
            l.Stroke = Brushes.Black;
            mwMainCanvas.Children.Add(l);

            Point p = new Point(centerw, centerh);
            Ellipse ell = new Ellipse();

            ell.Width = 3;
            ell.Height = 3;

            ell.StrokeThickness = 2;
            ell.Stroke = Brushes.Black;
            ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

            mwMainCanvas.Children.Add(ell);
        }

        public static void DrawPoints(List<double[]> ret, double h, Brush brush)
        {
            double centerw = mwMainCanvas.ActualWidth / 2, centerh = mwMainCanvas.ActualHeight / 2;
            double[] ues = UE(ret, centerw, centerh, h);
            double uew = ues[0];
            double ueh = ues[1];
            double actw, acth;
            Line l;

            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i][0] >= double.MinValue && ret[i][1] <= double.MaxValue)
                {
                    if (i == 95)
                    {
                        double d = 0;
                    }
                    actw = centerw + ret[i][0] * uew;
                    acth = centerh - ret[i][1] * ueh;
                    Point p = new Point(actw, acth);
                    Ellipse ell = new Ellipse();

                    ell.Width = 4;
                    ell.Height = 4;

                    ell.StrokeThickness = 2;
                    ell.Stroke = brush;
                    ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                    mwMainCanvas.Children.Add(ell);
                    if (i != 0)
                    {
                        l = new Line();
                        l.X1 = centerw + ret[i - 1][0] * uew;
                        l.Y1 = centerh - ret[i - 1][1] * ueh;
                        l.X2 = actw;
                        l.Y2 = acth;

                        l.StrokeThickness = 1;
                        l.Stroke = brush;

                        mwMainCanvas.Children.Add(l);
                    }
                }
            }
        }
    }
}
