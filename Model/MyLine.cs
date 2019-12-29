using System;
using System.Windows.Controls;

namespace Project.Model
{
    public class MyLine
    {
        public MyLine() { }

        public MyLine(string x1, string y1, string x2, string y2, string topR = null, string bottomR = null, string leftR = null, string rightR = null)
        {
            TopRadius = topR.ToDecimal();
            BottomRadius = bottomR.ToDecimal();

            LeftRadius = leftR.ToDecimal();
            RightRadius = rightR.ToDecimal();

            X1 = Math.Round(x1.ToDouble(), 2).ToString();
            Y1 = Math.Round(y1.ToDouble(), 2).ToString();
            X2 = Math.Round(x2.ToDouble(), 2).ToString();
            Y2 = Math.Round(y2.ToDouble(), 2).ToString();
        }

        public MyLine(double x1, double y1, double x2, double y2, decimal topR = 0, decimal bottomR = 0, decimal leftR = 0, decimal rightR = 0)
        {
            TopRadius = topR;
            BottomRadius = bottomR;

            LeftRadius = leftR;
            RightRadius = rightR;

            X1 = Math.Round(x1, 2).ToString();
            Y1 = Math.Round(y1, 2).ToString();
            X2 = Math.Round(x2, 2).ToString();
            Y2 = Math.Round(y2, 2).ToString();
        }

        public string X1 { get; set; }
        public string Y1 { get; set; }

        public string X2 { get; set; }
        public string Y2 { get; set; }

        public decimal TopRadius { get; set; }
        public decimal BottomRadius { get; set; }

        public decimal LeftRadius { get; set; }
        public decimal RightRadius { get; set; }
    }
}
