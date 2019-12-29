using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Data
    {
        public Data()
        {
            Figures = new List<Figure>();
        }

        public string Name { get; set; }
        public string Material { get; set; }

        public List<Figure> Figures { get; set; }
    }
}
