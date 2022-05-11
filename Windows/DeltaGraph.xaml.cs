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
using System.Windows.Shapes;

namespace Runge_Kutta.Windows
{
    /// <summary>
    /// Логика взаимодействия для DeltaGraph.xaml
    /// </summary>
    public partial class DeltaGraph : Window
    {
        public DeltaGraph()
        {
            InitializeComponent();
        }

        public void DrawDelta(double F, double mu, double h)
        {
            List<double[]> ret;
            try
            {
                ret = Equations.DeltaCPDGraph(Convert.ToDouble(DeltaStart.Text.Replace(".", ",")), Convert.ToDouble(DeltaEnd.Text.Replace(".", ",")), Convert.ToDouble(H.Text.Replace(".", ",")), F, mu, h);

                MainCanvas.Children.Clear();
                TBPoints.Text = string.Empty;

                DrawPoints(ret, Brushes.Black, " у.е.");

                string str = "";

                foreach (double[] d in ret)
                {
                    str += Convert.ToString(Math.Round(d[0], 6) + "; " + Math.Round(d[1], 6) + "\n");
                }

                TBPoints.Text = str;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Одно из чисел в окне \"КПД по Дельта\" введено некорректно");
            }
        }

        private void DrawCoordinates(string Units, double[] UE)
        {
            int roundNum = 6;
            if (MainCanvas.ActualWidth <= 1000)
            {
                roundNum = 3;
            }
            double centerw = MainCanvas.ActualWidth / 2, centerh = MainCanvas.ActualHeight / 2;
            Line l = new Line();

            l.StrokeThickness = 0.5;
            l.Stroke = Brushes.Black;

            l.X1 = centerw;
            l.Y1 = 0;
            l.X2 = centerw;
            l.Y2 = MainCanvas.ActualHeight;
            MainCanvas.Children.Add(l);

            l = new Line();

            l.X1 = 0;
            l.Y1 = centerh;
            l.X2 = MainCanvas.ActualWidth;
            l.Y2 = centerh;

            l.StrokeThickness = 0.5;
            l.Stroke = Brushes.Black;
            MainCanvas.Children.Add(l);

            double hv = MainCanvas.ActualHeight / 12, wv = MainCanvas.ActualWidth / 12;

            for (int i = 0; i <= 12; i++)
            {
                Point p = new Point(i * wv, centerh);
                Ellipse ell = new Ellipse();

                ell.Width = 3;
                ell.Height = 3;

                ell.StrokeThickness = 2;
                ell.Stroke = Brushes.Black;
                ell.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);

                MainCanvas.Children.Add(ell);

                Point p1 = new Point(centerw, i * hv);
                Ellipse ell1 = new Ellipse();

                ell1.Width = 3;
                ell1.Height = 3;

                ell1.StrokeThickness = 2;
                ell1.Stroke = Brushes.Black;
                ell1.Margin = new Thickness(p1.X - 2, p1.Y - 2, 0, 0);

                MainCanvas.Children.Add(ell1);

                if (i * hv < centerh)
                {
                    Label lab = new Label();
                    lab.Content = Math.Round((centerh - i * hv) / UE[1], roundNum).ToString() + Units;
                    lab.Margin = new Thickness(p1.X - 2, p1.Y - 2, 0, 0);
                    MainCanvas.Children.Add(lab);
                }
                if (i * wv > centerw)
                {
                    Label lab = new Label();
                    lab.Content = Math.Round((i * wv - centerw) / UE[0], roundNum).ToString() + Units;
                    lab.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);
                    MainCanvas.Children.Add(lab);
                }
                if (i * wv == centerw && i * hv == centerh)
                {
                    Label lab = new Label();
                    lab.Content = (Math.Round((centerh - i * hv) / UE[1], roundNum) + Math.Round((i * wv - centerw) / UE[0], roundNum)).ToString() + Units;
                    lab.Margin = new Thickness(p.X - 2, p.Y - 2, 0, 0);
                    MainCanvas.Children.Add(lab);
                }
            }
        }

        private void DrawPoints(List<double[]> ret, Brush brush, string Units)
        {
            double centerw = MainCanvas.ActualWidth / 2, centerh = MainCanvas.ActualHeight / 2;
            double[] ues = UE(ret, centerw, centerh);
            double uew = ues[0];
            double ueh = ues[1];
            DrawCoordinates(Units, ues);

            DrawSingleLine(ret, brush, centerw, centerh, uew, ueh);
        }

        private void DrawSingleLine(List<double[]> ret, Brush brush, double centerw, double centerh, double uew, double ueh)
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

                    MainCanvas.Children.Add(ell);
                    if (i != 0)
                    {
                        l = new Line();
                        l.X1 = centerw + ret[i - 1][0] * uew;
                        l.Y1 = centerh - ret[i - 1][1] * ueh;
                        l.X2 = actw;
                        l.Y2 = acth;

                        l.StrokeThickness = 1;
                        l.Stroke = brush;

                        MainCanvas.Children.Add(l);
                    }
                }
            }
        }

        private double[] UE(List<double[]> ret, double dimentionw, double dimentionh, int XIndex = 0)
        {
            double[] d = new double[2];

            // Самая большая по модулю часть графика, которая не улетает в бесконечность, равна соответствующему dimention
            // Условная единица измерения равна dimention, поделённому на самую большую по модулю часть графика, которая не улетает в бесконечность
            // При умножении координат точки графика на условные единицы, оная должна оказаться где-то между краем экрана и соответствующей осью координат,
            // если не улетает в бесконечность. 0 - X, 1 - Y

            foreach (double[] dbl in ret)
            {
                if (d[0] <= Math.Abs(dbl[XIndex]))
                {
                    d[0] = Math.Abs(dbl[XIndex]);
                }
                for (int i = 0; i < dbl.Length; i++)
                {
                    if (i == XIndex)
                    {
                        continue;
                    }
                    if (dbl[i] <= double.MaxValue && dbl[i] >= double.MinValue)
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
    }
}
