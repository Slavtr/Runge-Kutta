using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Runge_Kutta
{
    public static class Equations
    {
        #region Regulars
        private static readonly Regex rBrascets = new Regex(@"\([^\(\)]+\)");
        private static readonly Regex rSinus = new Regex(@"\-?[^a]sin\([\-\d+xy]+[+\-\/*]*[\-\dxy]+\)");
        private static readonly Regex rAsinus = new Regex(@"\-?asin\([\-\d+xy]+[+\-\/*]*[\-\dxy]+\)");
        private static readonly Regex rCosinus = new Regex(@"\-?[^a]cos\([\-\d+xy]+[+\-\/*][\-\dxy]+\)");
        private static readonly Regex rAcosinus = new Regex(@"\-?acos\([\-\d+xy]+[+\-\/*]*[\-\dxy]+\)");
        private static readonly Regex rTangens = new Regex(@"\-?[^a]tan\([\-\d+xy]+[+\-\/*]*[\-\dxy]+\)");
        private static readonly Regex rAtangens = new Regex(@"\-?atan\([\-\d+xy]+[+\-\/*]*[\-\dxy]+\)");
        private static readonly Regex rExponentiation = new Regex(@"\-?[\d,]+\^\-?[\d,]+");
        private static readonly Regex rDivision = new Regex(@"\-?[\d,]+\/\-?[\d,]+");
        private static readonly Regex rMultiplication = new Regex(@"\-?[\d,]+\*\-?[\d,]+");
        private static readonly Regex rAddition = new Regex(@"\-?[\d,]+\+\-?[\d,]+");
        private static readonly Regex rSubtraction = new Regex(@"\-?[\d,]+\-\-?[\d,]+");
        #endregion
        #region Runge-Kutta_regular_equation
        public static List<double[]> ReshenieRegular(double xStart, double xEnd, double yStart, double h, string proizv)
        {
            int length = Convert.ToInt32((xEnd - xStart) / h);
            List<double[]> ret = new List<double[]>();
            double y = yStart;
            double deltay;

            ret.Add(new double[] { Math.Round(xStart, 6), Math.Round(yStart, 6) });
            for (int i = 1; i < length; i++)
            {
                ret.Add(new double[2]);
                ret[i][0] = Math.Round(xStart + h * i, 6);
                deltay = DeltaY(ret[i][0], y, h, proizv.ConvertUravn());
                y += Math.Round(deltay, 6);
                ret[i][1] = Math.Round(y, 6);
            }

            return ret;
        }
        private static string ConvertUravn(this string input)
        {
            string retStr = input.Replace("exp", Convert.ToString(Math.E + "^"));
            retStr = retStr.Replace("x", "{0}");
            retStr = retStr.Replace("y", "{1}");
            string[] str = retStr.Split('=');
            return str[1];
        }
        private static double FindZnachFunc(this string func, double x, double y)
        {
            string str = string.Format(func, x, y);
            double ret = Expression(str);
            return ret;
        }
        private static double DeltaY(double x, double y, double h, string proizvY)
        {
            double k1 = h * proizvY.FindZnachFunc(x, y);
            double k2 = h * proizvY.FindZnachFunc(x + (h / 2), y + (k1 / 2));
            double k3 = h * proizvY.FindZnachFunc(x + (h / 2), y + (k2 / 2));
            double k4 = h * proizvY.FindZnachFunc(x + h, y + k3);
            return (k1 + 2 * k2 + 2 * k3 + k4) / 6;
        }
        private static double Expression(string input)
        {
            double ret;
            while (!double.TryParse(input, out ret))
            {
                while (rBrascets.IsMatch(input))
                {
                    foreach (Match m in rBrascets.Matches(input))
                    {
                        input = input.Replace(m.Value, Algebra(m.Value).Trim('(', ')'));
                    }
                }
                input = Trigonometry(input);
                input = Algebra(input);
            }
            return ret;
        }
        private static string Trigonometry(string input)
        {
            input = Sinus(input);
            input = Asinus(input);
            input = Cosinus(input);
            input = Acosinus(input);
            input = Tangens(input);
            input = Atangens(input);

            input = input.Replace("E", "");

            return input;
        }
        private static string Algebra(string input)
        {
            input = Exponentiation(input);
            input = Division(input);
            input = Multiplication(input);
            input = Addition(input);
            input = Subtraction(input);

            input = input.Replace("E", "");

            return input;
        }
        private static string Sinus(string input)
        {
            string str;

            while (rSinus.IsMatch(input))
            {
                foreach (Match m in rSinus.Matches(input))
                {
                    str = m.Value.Replace("sin", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Sin(Expression(str))));
                }
            }

            return input;
        }
        private static string Asinus(string input)
        {
            string str;

            while (rAsinus.IsMatch(input))
            {
                foreach (Match m in rAsinus.Matches(input))
                {
                    str = m.Value.Replace("asin", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Asin(Expression(str))));
                }
            }

            return input;
        }
        private static string Cosinus(string input)
        {
            string str;

            while (rCosinus.IsMatch(input))
            {
                foreach (Match m in rCosinus.Matches(input))
                {
                    str = m.Value.Replace("cos", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Cos(Expression(str))));
                }
            }

            return input;
        }
        private static string Acosinus(string input)
        {
            string str;

            while (rAcosinus.IsMatch(input))
            {
                foreach (Match m in rAcosinus.Matches(input))
                {
                    str = m.Value.Replace("acos", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Acos(Expression(str))));
                }
            }

            return input;
        }
        private static string Tangens(string input)
        {
            string str;

            while (rTangens.IsMatch(input))
            {
                foreach (Match m in rTangens.Matches(input))
                {
                    str = m.Value.Replace("tan", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Tan(Expression(str))));
                }
            }

            return input;
        }
        private static string Atangens(string input)
        {
            string str;

            while (rAtangens.IsMatch(input))
            {
                foreach (Match m in rAtangens.Matches(input))
                {
                    str = m.Value.Replace("atan", "").Trim('(', ')');
                    input = input.Replace(m.Value, Convert.ToString(Math.Atan(Expression(str))));
                }
            }

            return input;
        }
        private static string Exponentiation(string input)
        {
            while (rExponentiation.IsMatch(input))
            {
                foreach (Match m in rExponentiation.Matches(input))
                {
                    string[] str = m.Value.Split('^');
                    input = input.Replace(m.Value, Convert.ToString(Math.Pow(Convert.ToDouble(str[0]), Convert.ToDouble(str[1]))).Trim('(', ')'));
                }
            }
            return input;
        }
        private static string Division(string input)
        {
            while (rDivision.IsMatch(input))
            {
                foreach (Match m in rDivision.Matches(input))
                {
                    string[] str = m.Value.Split('/');
                    input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble(str[0]) / Convert.ToDouble(str[1])).Trim('(', ')'));
                }
            }
            return input;
        }
        private static string Multiplication(string input)
        {
            while (rMultiplication.IsMatch(input))
            {
                foreach (Match m in rMultiplication.Matches(input))
                {
                    string[] str = m.Value.Split('*');
                    input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble(str[0]) * Convert.ToDouble(str[1])).Trim('(', ')'));
                }
            }
            return input;
        }
        private static string Addition(string input)
        {
            while (rAddition.IsMatch(input))
            {
                foreach (Match m in rAddition.Matches(input))
                {
                    string[] str = m.Value.Split('+');
                    input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble(str[0]) + Convert.ToDouble(str[1])).Trim('(', ')'));
                }
            }
            return input;
        }
        private static string Subtraction(string input)
        {
            while (rSubtraction.IsMatch(input))
            {
                foreach (Match m in rSubtraction.Matches(input))
                {
                    string[] str = m.Value.Split('-');
                    switch (str.Length)
                    {
                        case 2:
                            input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble(str[0]) - Convert.ToDouble(str[1])).Trim('(', ')'));
                            break;
                        case 3:
                            if (str[0] == String.Empty)
                            {
                                input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble("-" + str[1]) - Convert.ToDouble(str[2])).Trim('(', ')'));
                            }
                            else
                            {
                                input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble(str[0]) - Convert.ToDouble("-" + str[2])).Trim('(', ')'));
                            }
                            break;
                        case 4:
                            input = input.Replace(m.Value, Convert.ToString(Convert.ToDouble("-" + str[1]) - Convert.ToDouble("-" + str[3])).Trim('(', ')'));
                            break;
                    }
                }
            }
            return input;
        }
        #endregion
        #region Module_a
        public static List<double[]> ReshenieComplex(double delta, double F, double mu, double teta, double h)
        {
            double xStart = (-Math.Sqrt(3) / 2) * mu;
            double xEnd = -xStart;
            double length = Math.Round((xEnd - xStart) / h);
            List<double[]> ret = new List<double[]>();
            double[] a = A(teta);
            double[] buf;

            double x, module;

            for (int i = 0; i <= length; i++)
            {
                x = xStart + i * h;
                buf = DeltaY(delta, F, teta, x, h, a, mu);
                a[0] += buf[0];
                a[1] += buf[1];
                module = Math.Sqrt(Math.Pow(a[0], 2) + Math.Pow(a[1], 2));
                ret.Add(new double[] { a[0], a[1], module, x });
            }
            return ret;
        }
        private static double[] Function3(double[] a, double x, double delta, double F, double mu)
        {
            double[] ret = new double[4];

            double moduleKvadr = Math.Pow(Math.Sqrt(Math.Pow(a[0], 2) + Math.Pow(a[1], 2)), 2);
            double f1 = -(a[1] * (delta + moduleKvadr - 1));
            double f2 = a[0] * (delta + moduleKvadr - 1) + FofX(x, mu, F);

            ret[0] = f1;
            ret[1] = f2;

            return ret;
        }
        private static double[] DeltaY(double delta, double F, double teta, double x, double h, double[] prevA, double mu)
        {
            double[] k1 = Function3(prevA, x, delta, F, mu);
            k1[0] *= h;
            k1[1] *= h;
            double[] buf = new double[4];
            buf[0] = k1[0] / 2;
            buf[1] = k1[1] / 2;
            buf[0] += prevA[0];
            buf[1] += prevA[1];
            double[] k2 = Function3(buf, x + (h / 2), delta, F, mu);
            k2[0] *= h;
            k2[1] *= h;
            buf[0] = k2[0] / 2;
            buf[1] = k2[1] / 2;
            buf[0] += prevA[0];
            buf[1] += prevA[1];
            double[] k3 = Function3(buf, x + (h / 2), delta, F, mu);
            k3[0] *= h;
            k3[1] *= h;
            buf[0] = k3[0];
            buf[1] = k3[1];
            buf[0] += prevA[0];
            buf[1] += prevA[1];
            double[] k4 = Function3(buf, x + h, delta, F, mu);
            k4[0] *= h;
            k4[1] *= h;

            buf[0] = (k1[0] + 2 * k2[0] + 2 * k3[0] + k4[0]) / 6;
            buf[1] = (k1[1] + 2 * k2[1] + 2 * k3[1] + k4[1]) / 6;

            return buf;
        }
        private static double FofX(this double x, double mu, double F)
        {
            return F / (Math.Exp(4 * Math.Pow(x, 2) / Math.Pow(mu, 2)));
        }
        private static double[] A(double teta)
        {
            double[] ret = new double[2];
            ret[0] = Math.Cos(teta);
            ret[1] = Math.Sin(teta);
            return ret;
        }
        #endregion
        #region Cross_CPD
        public static double CrossCPD(double delta, double F, double mu, double h)
        {
            double TPi = Math.PI * 2;
            double teta = TPi / 32;
            double xStart = (-Math.Sqrt(3) / 2) * mu;
            double xEnd = -xStart;
            double length = Math.Round((xEnd - xStart) / h);
            double[] a = A(teta);
            double[] buf;

            double x, module = 0, crossCPD = 0;

            for (int j = 1; j <= 32; j++)
            {
                for (int i = 0; i <= length; i++)
                {
                    x = xStart + i * h;
                    buf = DeltaY(delta, F, teta * j, x, h, a, mu);
                    a[0] += buf[0];
                    a[1] += buf[1];
                    module = Math.Sqrt(Math.Pow(a[0], 2) + Math.Pow(a[1], 2));
                }
                crossCPD += Math.Pow(module, 2);
            }

            crossCPD = 1 - crossCPD / 32;

            return crossCPD;
        }
        #endregion
        #region DeltaGraph
        public static List<double[]> DeltaCPDGraph(double deltaStart, double deltaEnd, double deltaH, double F, double mu, double h)
        {
            List<double[]> result = new List<double[]>();
            double length = Math.Round((deltaStart - deltaEnd) / deltaH);

            double delta, crossCPD;

            for(int i = 0; i < length; i++)
            {
                delta = deltaStart + i * deltaH;

                crossCPD = CrossCPD(delta, F, mu, h);

                result.Add(new double[] { delta, crossCPD });
            }

            return result;
        }
        #endregion
    }
}
