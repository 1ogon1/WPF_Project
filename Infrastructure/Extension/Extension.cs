using System.Collections.Generic;
using System.Windows.Shapes;

namespace System.Windows.Controls
{
    public static class Extension
    {
        public static void AddRange(this UIElementCollection children, List<Line> lines)
        {
            if (lines != null)
            {
                foreach (var line in lines)
                {
                    children.Add(line);
                }
            }
        }
    }
}
