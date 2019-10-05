namespace Project.Model
{
    public class MyLine
    {
        public MyLine() { }

        public MyLine(string x1, string y1, string x2, string y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public MyLine(double x1, double y1, double x2, double y2)
        {
            X1 = x1.ToString();
            Y1 = y1.ToString();
            X2 = x2.ToString();
            Y2 = y2.ToString();
        }

        public string X1 { get; set; }
        public string Y1 { get; set; }

        public string X2 { get; set; }
        public string Y2 { get; set; }
    }
}
