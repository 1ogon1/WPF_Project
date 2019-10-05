using Project.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Project.Infrastructure.Helper;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Project
{
    public partial class MainWindow : Window
    {
        //public Helper Helper;
        public List<MyLine> Lines = new List<MyLine>();

        public bool IsInit = true;

        public double Multiplicity = 1;
        public double Zoom = default(double);
        public double CenterX = default(double);
        public double CenterY = default(double);

        public double ZoomBase { get { return 46.456692913; } }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void ZoonIncrease(object sender, RoutedEventArgs e)
        {
            Multiplicity += 0.1;

            Zoom = ZoomBase * Multiplicity;

            Init(true);
        }

        private void ZoonDecrease(object sender, RoutedEventArgs e)
        {
            if (Multiplicity <= 0.2) { Multiplicity = 0.1; }
            else { Multiplicity -= 0.1; }

            Zoom = ZoomBase * Multiplicity;

            Init(true);
        }

        private void Init(bool recalc = false, bool clear = false)
        {
            if (IsInit || recalc)
            {
                if (clear)
                {
                    Multiplicity = 1;

                    Lines = new List<MyLine>();

                    Zoom = ZoomBase * Multiplicity;
                }

                if (recalc)
                {
                    MyCanvas.Children.Clear();
                }
                else
                {
                    IsInit = false;
                }

                if (Zoom < 1)
                {
                    Zoom = ZoomBase;
                }

                CurrentMultiplicity.Text = $"Zoom: 1:{Multiplicity}";

                Increase.Visibility = Visibility.Visible;
                Decrease.Visibility = Visibility.Visible;
                ControlPanel.Visibility = Visibility.Visible;

                CenterX = MyCanvas.ActualWidth / 2;
                CenterY = MyCanvas.ActualHeight / 2;

                MyCanvas.Children.Add(GetLine(CenterX, 0, CenterX, MyCanvas.ActualHeight, false, 1)); //vertical line

                MyCanvas.Children.Add(GetLine(0, CenterY, MyCanvas.ActualWidth, CenterY, false, 1)); //horizontal line

                for (double i = CenterY; i > 0; i -= Zoom)
                {
                    if (i != CenterY)
                    {
                        MyCanvas.Children.Add(GetLine(CenterX - 5 * Multiplicity, i, CenterX + 5 * Multiplicity, i, false, 1));
                    }
                }

                for (double i = CenterX; i < MyCanvas.ActualWidth; i += Zoom)
                {
                    if (i != CenterX)
                    {
                        MyCanvas.Children.Add(GetLine(i, CenterY - 5 * Multiplicity, i, CenterY + 5 * Multiplicity, false, 1));
                    }
                }

                for (double i = CenterY; i < MyCanvas.ActualHeight; i += Zoom)
                {
                    if (i != CenterY)
                    {
                        MyCanvas.Children.Add(GetLine(CenterX - 5 * Multiplicity, i, CenterX + 5 * Multiplicity, i, false, 1));
                    }
                }

                for (double i = CenterX; i > 0; i -= Zoom)
                {
                    if (i != CenterX)
                    {
                        MyCanvas.Children.Add(GetLine(i, CenterY - 5 * Multiplicity, i, CenterY + 5 * Multiplicity, false, 1));
                    }
                }

                //Polyline vertArr = new Polyline();
                //vertArr.Points = new PointCollection();
                //vertArr.Points.Add(new Point(center + 5, 15));
                //vertArr.Points.Add(new Point(center + 10, 10));
                //vertArr.Points.Add(new Point(center + 15, 15));
                //vertArr.Stroke = Brushes.Black;
                //MyCanvas.Children.Add(vertArr);
                //Polyline horArr = new Polyline();
                //horArr.Points = new PointCollection();
                //horArr.Points.Add(new Point(center + 145, 145));
                //horArr.Points.Add(new Point(center + 150, 150));
                //horArr.Points.Add(new Point(center + 145, 155));
                //horArr.Stroke = Brushes.Black;
                //MyCanvas.Children.Add(horArr);

                if (recalc && !clear)
                {
                    ReWriteLines();
                }
            }
        }

        //private void AddLine(object sender, RoutedEventArgs e)
        //{
        //    Lines.Add(new MyLine(X1.Text, Y1.Text, X2.Text, Y2.Text));
        //    Myanvas.Children.Add(GetLine(X1.Text, Y1.Text, X2.Text, Y2.Text));
        //}

        private void AddFigure1(object sender, RoutedEventArgs e)
        {
            Clear();

            var line1 = GetLine(-2, 1, 2, 1);
            var line2 = GetLine(2, 1, 2, -1);
            var line3 = GetLine(2, -1, -2, -1);
            var line4 = GetLine(-2, -1, -2, 1);

            Lines.Add(new MyLine(-2, 1, 2, 1));
            Lines.Add(new MyLine(2, 1, 2, -1));
            Lines.Add(new MyLine(2, -1, -2, -1));
            Lines.Add(new MyLine(-2, -1, -2, 1));

            MyCanvas.Children.Add(line1);
            MyCanvas.Children.Add(line2);
            MyCanvas.Children.Add(line3);
            MyCanvas.Children.Add(line4);

            InitControlPanel();
        }

        private void InitControlPanel()
        {
            ControlPanel.Children.Clear();

            foreach (var line in Lines)
            {
                ControlPanel.Children.Add(new TextBlock()
                {
                    Text = "X1"
                });
                ControlPanel.Children.Add(new TextBox()
                {
                    Width = 90,
                    Height = 20,
                    Text = line.X1,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Top
                });

                ControlPanel.Children.Add(new TextBlock()
                {
                    Text = "Y1"
                });
                ControlPanel.Children.Add(new TextBox()
                {
                    Width = 90,
                    Height = 20,
                    Text = line.Y1,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Top
                });
                ControlPanel.Children.Add(new TextBlock()
                {
                    Text = "X2"
                });
                ControlPanel.Children.Add(new TextBox()
                {
                    Width = 90,
                    Height = 20,
                    Text = line.X2,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Top
                });
                ControlPanel.Children.Add(new TextBlock()
                {
                    Text = "Y2"
                });
                ControlPanel.Children.Add(new TextBox()
                {
                    Width = 90,
                    Height = 20,
                    Text = line.Y2,
                    TextWrapping = TextWrapping.Wrap,
                    VerticalAlignment = VerticalAlignment.Top
                });
            }

            ControlPanel.Children.Add(new Button()
            {
                Width = 75,
                Name = "ClearCanvas",
                Content = "Очистити",
                Margin = new Thickness(0, 5 , 0, 0),
                VerticalAlignment =  VerticalAlignment.Top
            });
        }

        private void AddFigure2(object sender, RoutedEventArgs e)
        {
            Clear();

            var line1 = GetLine(-1.5, 1, 1.5, 1);
            var line2 = GetLine(1.5, 1, 2, -1);
            var line3 = GetLine(2, -1, -2, -1);
            var line4 = GetLine(-2, -1, -1.5, 1);

            Lines.Add(new MyLine(-1.5, 1, 1.5, 1));
            Lines.Add(new MyLine(1.5, 1, 2, -1));
            Lines.Add(new MyLine(2, -1, -2, -1));
            Lines.Add(new MyLine(-2, -1, -1.5, 1));

            MyCanvas.Children.Add(line1);
            MyCanvas.Children.Add(line2);
            MyCanvas.Children.Add(line3);
            MyCanvas.Children.Add(line4);

            InitControlPanel();
        }

        private void ReWriteLines()
        {
            foreach (var line in Lines)
            {
                MyCanvas.Children.Add(GetLine(line.X1, line.Y1, line.X2, line.Y2));
            }
        }

        private void ClearCanvas(object sender, RoutedEventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            if (IsInit)
            {
                Init();
            }
            else
            {
                Init(true, true);
            }
        }

        private void SaveToFile(object sender, RoutedEventArgs e)
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //File.WriteAllText(System.IO.Path.Combine(docPath, "WriteFile.txt"), text);

            string[] lines = { };

            File.AppendAllLines(System.IO.Path.Combine(docPath, "WriteFile.txt"), lines);
        }
        #region Helper

        public Line GetLine(string x1, string y1, string x2, string y2, bool zoom = true, int thickness = 2) => new Line()
        {
            X1 = SetX(x1, zoom),
            Y1 = SetY(y1, zoom),
            X2 = SetX(x2, zoom),
            Y2 = SetY(y2, zoom),
            StrokeThickness = 2,
            Stroke = new SolidColorBrush(Colors.Black)
        };

        public Line GetLine(double x1, double y1, double x2, double y2, bool zoom = true, int thickness = 2) => new Line()
        {
            X1 = SetX(x1, zoom),
            Y1 = SetY(y1, zoom),
            X2 = SetX(x2, zoom),
            Y2 = SetY(y2, zoom),
            StrokeThickness = thickness,
            Stroke = new SolidColorBrush(Colors.Black)
        };

        private double SetX(string x, bool zoom) => zoom ? CenterX + (Zoom * Convert.ToDouble(x)) : Convert.ToDouble(x);

        private double SetY(string y, bool zoom) => zoom ? CenterY - (Zoom * Convert.ToDouble(y)) : Convert.ToDouble(y);

        private double SetX(double x, bool zoom) => zoom ? CenterX + (Zoom * Convert.ToDouble(x)) : x;

        private double SetY(double y, bool zoom) => zoom ? CenterY - (Zoom * Convert.ToDouble(y)) : y;

        #endregion

    }
}
