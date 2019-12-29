using Project.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static bool ContainsKey(this ResourceDictionary values, string key) => values[key] != null;

        public static void RemoveKey(this ResourceDictionary values, string key)
        {
            var value = values[key];

            if (value != null)
            {
                values.Remove(value);
            }
        }

        public static double ToDouble(this string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }

            return default(double);
        }

        public static bool IsDouble(this string value) => double.TryParse(value, out double result);

        public static decimal ToDecimal(this string value)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                return result;
            }

            return default(decimal);
        }

        public static bool IsValid(this TextBox value)
        {
            if (value == null)
            {
                return false;
            }

            return value.Name == "X1" || value.Name == "Y1" || value.Name == "X2" || value.Name == "Y2" ? true : false;
        }

        public static bool IsEmpty(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return true;
            }

            if (value.IsDouble() && value.ToDouble() == 0)
            {
                return true;
            }

            return false;
        }

        public static bool GetCoordinates(this string value, out double x, out double y)
        {
            x = 0;
            y = 0;

            if (string.IsNullOrWhiteSpace(value)) return false;

            var values = value.Split(' ').ToList();

            return values.Count >= 2 && double.TryParse(values[0].Trim(), out x) && double.TryParse(values[1].Trim(), out y);
        }
    }
}
