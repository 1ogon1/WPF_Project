using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Project.Infrastructure.Helper
{
    public class Helper
    {
        public Helper() { }

        public Helper(double zoom, double x, double y)
        {
            Zoom = zoom;
            CenterX = x;
            CenterY = y;
        }

        public double Zoom = default(double);
        public double CenterX = default(double);
        public double CenterY = default(double);

    
    }
}
