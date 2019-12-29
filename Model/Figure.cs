using System.Collections.Generic;

namespace Project.Model
{
    public class Figure
    {
        public Figure()
        {
            ID = -1;

            Type = FigureType.none;

            BaseLines = new List<MyLine>();
            DrawLines = new List<MyLine>();
        }

        public Figure(string name)
        {
            ID = -1;

            Name = name;
            Type = FigureType.none;

            BaseLines = new List<MyLine>();
            DrawLines = new List<MyLine>();
        }

        public int ID { get; set; }

        public string Name { get; set; }

        public FigureType Type { get; set; }

        public List<MyLine> BaseLines { get; set; }
        public List<MyLine> DrawLines { get; set; }
    }
}
