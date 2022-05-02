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

        private static double[] UE(List<double[]> ret, double dimentionw, double dimentionh, int XIndex = 0)
        {
            double[] d = new double[2];

            // Самая большая по модулю часть графика, которая не улетает в бесконечность, равна соответствующему dimention
            // Условная единица измерения равна dimention, поделённому на самую большую по модулю часть графика, которая не улетает в бесконечность
            // При умножении координат точки графика на условные единицы, оная должна оказаться где-то между краем экрана и соответствующей осью координат,
            // если не улетает в бесконечность. 0 - X, 1 - Y

            foreach(double[] dbl in ret)
            {
                if(d[0] <= Math.Abs(dbl[XIndex]))
                {
                    d[0] = Math.Abs(dbl[XIndex]);
                }
                for (int i = 0; i < dbl.Length; i++) 
                {
                    if (i == XIndex)
                    {
                        continue;
                    }
                    if(dbl[i] <= double.MaxValue && dbl[i] >= double.MinValue)
                    {
                        if (Math.Abs(dbl[i]) >= d[1]) 
                        {
                            d[1] = Math.Abs(dbl[i]);
                        }
                    }
                }
            }

            d[0] = dimentionw / d[0];

            if (d[1] == 0)
            {
                d[1] = dimentionh;
            }

            d[1] = dimentionh / d[1];

            return d;
        }

        public static void DrawCoordinates(string Units, double[] UE)
        {
            int roundNum = 6;
            if(mwMainCanvas.ActualWidth <= 1000)
            {
                roundNum = 3;
            }
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

            double hv = mwMainCanvas.ActualHeight / 12, wv = mwMainCanvas.ActualWidth / 12;

            for (int i = 0; i <= 12; i++) 
            {
                Point p = new Point(i * wv, centerh);
                Ellipse ell = new Ellipse();

                ell.Width = 3;
                ell.Height = 3;

                ell.StrokeThickness = 2;
                ell.Stroke = Brushes.Black;
                ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                mwMainCanvas.Children.Add(ell);

                Point p1 = new Point(centerw, i * hv);
                Ellipse ell1 = new Ellipse();

                ell1.Width = 3;
                ell1.Height = 3;

                ell1.StrokeThickness = 2;
                ell1.Stroke = Brushes.Black;
                ell1.Margin = new Thickness(p1.X - 2, p1.Y - 2, 0, 0);

                mwMainCanvas.Children.Add(ell1);

                if (i * hv < centerh) 
                {
                    Label lab = new Label();
                    lab.Content = Math.Round((centerh - i * hv) / UE[1], roundNum).ToString() + Units;
                    lab.Margin = new Thickness(p1.X - 2, p1.Y - 2, 0, 0);
                    mwMainCanvas.Children.Add(lab);
                }
                if (i * wv > centerw)
                {
                    Label lab = new Label();
                    lab.Content = Math.Round((i * wv - centerw) / UE[0], roundNum).ToString() + Units;
                    lab.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);
                    mwMainCanvas.Children.Add(lab);
                }
                if(i * wv == centerw && i * hv == centerh)
                {
                    Label lab = new Label();
                    lab.Content = (Math.Round((centerh - i * hv) / UE[1], roundNum) + Math.Round((i * wv - centerw) / UE[0], roundNum)).ToString() + Units;
                    lab.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);
                    mwMainCanvas.Children.Add(lab);
                }
            }
        }

        public static void DrawPoints(List<double[]> ret, Brush brush, string Units)
        {
            double centerw = mwMainCanvas.ActualWidth / 2, centerh = mwMainCanvas.ActualHeight / 2;
            double[] ues = UE(ret, centerw, centerh);
            double uew = ues[0];
            double ueh = ues[1];
            DrawCoordinates(Units, ues);

            DrawSingleLine(ret, brush, centerw, centerh, uew, ueh);
        }
        public static void DrawPoints(List<double[]> ret, Brush[] brush, string Units, int XIndex)
        {
            double centerw = mwMainCanvas.ActualWidth / 2, centerh = mwMainCanvas.ActualHeight / 2;
            double[] ues = UE(ret, centerw, centerh, XIndex);
            double uew = ues[0];
            double ueh = ues[1];
            DrawCoordinates(Units, ues);

            DrawMultiLine(ret, brush, centerw, centerh, uew, ueh, XIndex);
        }
        private static void DrawSingleLine(List<double[]> ret, Brush brush, double centerw, double centerh, double uew, double ueh)
        {
            double actw, acth;
            Line l;
            Point p;
            Ellipse ell;

            for (int i = 0; i < ret.Count; i++)
            {
                if (ret[i][0] >= double.MinValue && ret[i][1] <= double.MaxValue)
                {
                    actw = centerw + ret[i][0] * uew;
                    acth = centerh - ret[i][1] * ueh;

                    p = new Point(actw, acth);
                    ell = new Ellipse();

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
        private static void DrawMultiLine(List<double[]> ret, Brush[] brush, double centerw, double centerh, double uew, double ueh, int XIndex)
        {
            List<List<double[]>> lineList = new List<List<double[]>>();

            for (int i = 0; i < ret.First().Length && i != XIndex; i++) 
            {
                lineList.Add(ConvertToLine(ret, XIndex, i));
            }

            double actw, acth;
            Line l;
            Point p;
            Ellipse ell;

            foreach(List<double[]> ls in lineList)
            {
                for(int i = 0; i<ls.Count; i++)
                {
                    if (ls[i][0] >= double.MinValue && ls[i][1] <= double.MaxValue)
                    {
                        actw = centerw + ls[i][0] * uew;
                        acth = centerh - ls[i][1] * ueh;
                        p = new Point(actw, acth);
                        ell = new Ellipse();

                        ell.Width = 4;
                        ell.Height = 4;

                        ell.StrokeThickness = 2;
                        ell.Stroke = brush[lineList.IndexOf(ls)];
                        ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                        mwMainCanvas.Children.Add(ell);
                        if (i != 0)
                        {
                            l = new Line();
                            l.X1 = centerw + ls[i - 1][0] * uew;
                            l.Y1 = centerh - ls[i - 1][1] * ueh;
                            l.X2 = actw;
                            l.Y2 = acth;

                            l.StrokeThickness = 1;
                            l.Stroke = brush[lineList.IndexOf(ls)];

                            mwMainCanvas.Children.Add(l);
                        }
                    }
                }
            }
        }

        private static List<double[]> ConvertToLine(List<double[]> input, int numberX, int numberY)
        {
            List<double[]> ret = new List<double[]>();
            for (int i = 0; i < input.Count; i++)
            {
                if (Math.Abs(input[i][numberX]) >= double.MaxValue || Math.Abs(input[i][numberY]) >= double.MaxValue)
                {
                    ret.RemoveAt(i - 1);
                    return ret;
                }
                ret.Add(new double[] { input[i][numberX], input[i][numberY] });
            }
            return ret;
        }
    }
}
