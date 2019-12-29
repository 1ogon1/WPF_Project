using Project.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Linq;

namespace Project.Infrastructure.Helper
{
    public enum LineType
    {
        Top,
        Right,
        Bottom,
        Left
    }

    public class Helper
    {
        private WorkPanel WorkPanel;

        public Helper() { }

        public Helper(WorkPanel workPanel)
        {
            WorkPanel = workPanel;

            WorkPanel.RightMenuShow.IsEnabled = false;
            WorkPanel.RightMenuHide.IsEnabled = false;

            WorkPanel.Left = (SystemParameters.PrimaryScreenWidth / 2) - (WorkPanel.Width / 2);
            WorkPanel.Top = (SystemParameters.PrimaryScreenHeight / 2) - (WorkPanel.Height / 2);

            AddHotKey(Key.E, workPanel.Exit);
            AddHotKey(Key.T, workPanel.Trapeze);
            AddHotKey(Key.O, workPanel.OpenFile);
            AddHotKey(Key.R, workPanel.Rectangle);
            AddHotKey(Key.A, workPanel.AddFigure);
            AddHotKey(Key.S, workPanel.SaveToFile);
            AddHotKey(Key.D, workPanel.ClearCanvas);
            AddHotKey(Key.OemPlus, workPanel.ZoomIncrease, ModifierKeys.None);
            AddHotKey(Key.OemMinus, workPanel.ZoomDecrease, ModifierKeys.None);
            AddHotKey(Key.T, workPanel.SetTrapezePoints, ModifierKeys.Control | ModifierKeys.Shift);
            AddHotKey(Key.R, workPanel.SetRectanglePoints, ModifierKeys.Control | ModifierKeys.Shift);
        }

        public void Update(WorkPanel window) => WorkPanel = window;

        public void AddHotKey(Key key, ExecutedRoutedEventHandler handler, ModifierKeys modifier = ModifierKeys.Control)
        {
            RoutedCommand command = new RoutedCommand();

            command.InputGestures.Add(new KeyGesture(key, modifier));
            WorkPanel.CommandBindings.Add(new CommandBinding(command, handler));
        }

        public List<Line> DrawLine(string x1, string y1, string x2, string y2, bool zoom = true, int thickness = 2)
        {
            if (double.TryParse(x1, out double vX1) && double.TryParse(x2, out double vX2) && double.TryParse(y1, out double vY1) && double.TryParse(y2, out double vY2))
            {
                return DrawLine(vX1, vY1, vX2, vY2, zoom, thickness);
            }

            return new List<Line>();
        }

        public List<Line> DrawLine(double x1, double y1, double x2, double y2, bool zoom = true, int thickness = 2)
        {
            var myX1 = (int)SetX(x1, zoom);
            var myX2 = (int)SetX(x2, zoom);

            var myY1 = (int)SetY(y1, zoom);
            var myY2 = (int)SetY(y2, zoom);

            PointCollection points = new PointCollection();
            var myPolygon = new Polygon
            {
                Stroke = Brushes.Black,
                StrokeThickness = thickness,
            };

            int deltaX = Math.Abs(myX2 - myX1);
            int deltaY = Math.Abs(myY2 - myY1);
            int signX = myX1 < myX2 ? 1 : -1;
            int signY = myY1 < myY2 ? 1 : -1;

            int error = deltaX - deltaY;

            var res = new List<Line>
            {
                new Line()
                {
                    X1 = myX2,
                    Y1 = myY2,
                    X2 = myX2 + 1,
                    Y2 = myY2 + 1,
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness
                }
            };

            while (myX1 != myX2 || myY1 != myY2)
            {
                res.Add(new Line()
                {
                    X1 = myX1,
                    Y1 = myY1,
                    X2 = myX1 + 1,
                    Y2 = myY1 + 1,
                    Stroke = Brushes.Black,
                    StrokeThickness = thickness
                });
                int error2 = error * 2;

                if (error2 > -deltaY)
                {
                    error -= deltaY;
                    myX1 += signX;
                }

                if (error2 < deltaX)
                {
                    error += deltaX;
                    myY1 += signY;
                }
            }

            myPolygon.Points = points;

            return res;
        }

        public void Clear(bool clearMenu = false)
        {
            if (clearMenu)
            {
                if (WorkPanel.RightMenuHide.Visibility == Visibility.Visible)
                {
                    Toogle(WorkPanel.HideMenu, WorkPanel.HideFiguresPanel);
                }

                WorkPanel.Data.Figures = new List<Figure>();

                InitFiguresPanelContent();

                WorkPanel.Figure = new Figure();
                WorkPanel.Parameters.Children.Clear();
                WorkPanel.RightMenuShow.IsEnabled = false;
                WorkPanel.RightMenuHide.IsEnabled = false;

                WorkPanel.Top_X0.Text = string.Empty;
                WorkPanel.Top_Y0.Text = string.Empty;
                WorkPanel.Top_X1.Text = string.Empty;
                WorkPanel.Top_Y1.Text = string.Empty;

                WorkPanel.Left_X0.Text = string.Empty;
                WorkPanel.Left_Y0.Text = string.Empty;
                WorkPanel.Left_X1.Text = string.Empty;
                WorkPanel.Left_Y1.Text = string.Empty;
                             
                WorkPanel.Right_X0.Text = string.Empty;
                WorkPanel.Right_Y0.Text = string.Empty;
                WorkPanel.Right_X1.Text = string.Empty;
                WorkPanel.Right_Y1.Text = string.Empty;

                WorkPanel.Bottom_X0.Text = string.Empty;
                WorkPanel.Bottom_Y0.Text = string.Empty;
                WorkPanel.Bottom_X1.Text = string.Empty;
                WorkPanel.Bottom_Y1.Text = string.Empty;
            }

            if (WorkPanel.IsInit)
            {
                WorkPanel.Init();
            }
            else
            {
                WorkPanel.Init(true, true);
            }
        }

        public void DrawFigure(MyLine myLine1, MyLine myLine2, MyLine myLine3, MyLine myLine4, FigureType type, string name = null, int id = -1)
        {
            Clear();

            WorkPanel.Figure = new Figure()
            {
                ID = id,
                Type = type,
                Name = name,
                BaseLines = new List<MyLine>()
                {
                    myLine1,
                    myLine2,
                    myLine3,
                    myLine4
                }
            };

            WorkPanel.Top_X0.Text = myLine1.X1;
            WorkPanel.Top_Y0.Text = myLine1.Y1;
            WorkPanel.Top_X1.Text = myLine1.X2;
            WorkPanel.Top_Y1.Text = myLine1.Y2;

            WorkPanel.Left_X0.Text = myLine4.X1;
            WorkPanel.Left_Y0.Text = myLine4.Y1;
            WorkPanel.Left_X1.Text = myLine4.X2;
            WorkPanel.Left_Y1.Text = myLine4.Y2;

            WorkPanel.Right_X0.Text = myLine2.X1;
            WorkPanel.Right_Y0.Text = myLine2.Y1;
            WorkPanel.Right_X1.Text = myLine2.X2;
            WorkPanel.Right_Y1.Text = myLine2.Y2;

            WorkPanel.Bottom_X0.Text = myLine3.X1;
            WorkPanel.Bottom_Y0.Text = myLine3.Y1;
            WorkPanel.Bottom_X1.Text = myLine3.X2;
            WorkPanel.Bottom_Y1.Text = myLine3.Y2;

            ReWriteLines();
        }

        /// <summary>
        /// Показує або сховує бокову панель
        /// </summary>
        /// <param name="Storyboard">Назва панелі</param>
        public void ShowHideMenu(string MenuStoryboard, string PanelStoryboard, bool isMenu = true)
        {
            //board.Duration = TimeSpan.FromSeconds(0.2);
            if (WorkPanel.RightMenuHide.Visibility == Visibility.Hidden || WorkPanel.FiguresPanelHide.Visibility == Visibility.Hidden)
            {
                Toogle(MenuStoryboard, PanelStoryboard);

                WorkPanel.RightMenu.SetValue(Panel.ZIndexProperty, isMenu ? 2 : 1);
                WorkPanel.FiguresPanel.SetValue(Panel.ZIndexProperty, isMenu ? 1 : 2);
            }
            else
            {
                if (isMenu && Panel.GetZIndex(WorkPanel.RightMenu) == 2)
                {
                    Toogle(MenuStoryboard, PanelStoryboard);
                }
                else if (!isMenu && Panel.GetZIndex(WorkPanel.FiguresPanel) == 2)
                {
                    Toogle(MenuStoryboard, PanelStoryboard);
                }
                else
                {
                    WorkPanel.RightMenu.SetValue(Panel.ZIndexProperty, Panel.GetZIndex(WorkPanel.RightMenu) == 2 ? 1 : 2);
                    WorkPanel.FiguresPanel.SetValue(Panel.ZIndexProperty, Panel.GetZIndex(WorkPanel.FiguresPanel) == 2 ? 1 : 2);
                }
            }
            //if (parent && Panel.GetZIndex(WorkPanel.RightMenu) == 1)
            //{
            //    WorkPanel.RightMenu.SetValue(Panel.ZIndexProperty, 2);
            //}
            //else
            //{
            //    board.Begin(WorkPanel.RightMenu);
            //    WorkPanel.RightMenu.SetValue(Panel.ZIndexProperty, 2);
            //    WorkPanel.RightMenuHide.Visibility = MenuStoryboard.Contains("Show") ? Visibility.Visible : Visibility.Hidden;
            //    WorkPanel.RightMenuShow.Visibility = MenuStoryboard.Contains("Show") ? Visibility.Hidden : Visibility.Visible;
            //}

            //if (parent)
            //{
            //    FiguresPanel(MenuStoryboard, PanelStoryboard, false);
            //}
        }

        private void Toogle(string MenuStoryboard, string PanelStoryboard)
        {
            Storyboard menu = WorkPanel.Resources[MenuStoryboard] as Storyboard;
            Storyboard panel = WorkPanel.Resources[PanelStoryboard] as Storyboard;

            menu.Begin(WorkPanel.RightMenu);

            WorkPanel.RightMenuHide.Visibility = MenuStoryboard.Contains("Show") ? Visibility.Visible : Visibility.Hidden;
            WorkPanel.RightMenuShow.Visibility = MenuStoryboard.Contains("Show") ? Visibility.Hidden : Visibility.Visible;

            panel.Begin(WorkPanel.FiguresPanel);

            WorkPanel.FiguresPanelHide.Visibility = PanelStoryboard.Contains("Show") ? Visibility.Visible : Visibility.Hidden;
            WorkPanel.FiguresPanelShow.Visibility = PanelStoryboard.Contains("Show") ? Visibility.Hidden : Visibility.Visible;
        }

        /// <summary>
        /// Показує або сховує бокову панель
        /// </summary>
        /// <param name="Storyboard">Назва панелі</param>
        //public void FiguresPanel(string MenuStoryboard, string PanelStoryboard, bool parent = true)
        //{
        //    Storyboard board = WorkPanel.Resources[PanelStoryboard] as Storyboard;

        //    WorkPanel.FiguresPanel.SetValue(Panel.ZIndexProperty, parent ? 2 : 1);

        //    if (!parent)
        //    {
        //        board.Begin(WorkPanel.FiguresPanel);

        //        WorkPanel.FiguresPanelHide.Visibility = PanelStoryboard.Contains("Show") ? Visibility.Visible : Visibility.Hidden;
        //        WorkPanel.FiguresPanelShow.Visibility = PanelStoryboard.Contains("Show") ? Visibility.Hidden : Visibility.Visible;
        //    }

        //    if (parent)
        //    {
        //        ShowHideMenu(MenuStoryboard, PanelStoryboard, false);
        //    }
        //}

        /// <summary>
        /// Використовується при зміні заокруглення кутів фігури
        /// </summary>
        public void ReWriteLines()
        {
            if (WorkPanel.Figure.BaseLines.Count > 0)
            {
                var lines = new List<MyLine>()
                {
                    WorkPanel.Figure.BaseLines[0],
                    WorkPanel.Figure.BaseLines[1],
                    WorkPanel.Figure.BaseLines[2],
                    WorkPanel.Figure.BaseLines[3]
                };

                if (WorkPanel.Figure.Type == FigureType.rectangle)
                {
                    lines = GetRectangleLines(lines);
                }
                else if (WorkPanel.Figure.Type == FigureType.trapeze)
                {
                    lines = GetTrapezeLines(lines);
                }

                WorkPanel.Figure.DrawLines = lines;

                foreach (var line in lines)
                {
                    WorkPanel.MyCanvas.Children.AddRange(DrawLine(line.X1, line.Y1, line.X2, line.Y2));
                }
            }
        }

        /// <summary>
        /// Оновлює модель фігури, при зміні її координат
        /// </summary>
        /// <param name="childrens"></param>
        public void UpdateLines(UIElementCollection childrens)
        {
            var index = 0;
            var element = 0;
            MyLine line = null;

            for (int i = 1; i < childrens.Count; i += 2)
            {
                var box = childrens[i] as TextBox;

                if (WorkPanel.Figure.BaseLines.Count >= index && box.IsValid())
                {
                    if (element % 2 == 0)
                    {
                        line = WorkPanel.Figure.BaseLines[index];
                    }

                    if (line != null)
                    {
                        if (box.Name == "X1" && !string.IsNullOrWhiteSpace(box.Text) && decimal.TryParse(box.Text, out decimal value))
                        {
                            line.X1 = box.Text.Trim();
                        }
                        else if (box.Name == "X2" && !string.IsNullOrWhiteSpace(box.Text) && decimal.TryParse(box.Text, out value))
                        {
                            line.X2 = box.Text.Trim();
                        }
                        else if (box.Name == "Y1" && !string.IsNullOrWhiteSpace(box.Text) && decimal.TryParse(box.Text, out value))
                        {
                            line.Y1 = box.Text.Trim();
                        }
                        else if (box.Name == "Y2" && !string.IsNullOrWhiteSpace(box.Text) && decimal.TryParse(box.Text, out value))
                        {
                            line.Y2 = box.Text.Trim();
                        }
                    }

                    if (element % 2 != 0)
                    {
                        index++;
                    }

                    element++;
                }
            }
        }

        public void InitRectangleParameters(double width, double height)
        {
            WorkPanel.Parameters.Children.Clear();

            #region First Grid

            var firstGrid = new Grid();

            firstGrid.Children.Add(new TextBlock()
            {
                Text = "Довжина",
                Margin = new Thickness(0, 0, 64.4, 0)
            });

            var widthTextBlock = new TextBox()
            {
                Name = "RectangleWidth",
                Text = Math.Round(width, 2).ToString(),
                Margin = new Thickness(71, 0, -0.6, 0)
            };

            widthTextBlock.TextChanged += WorkPanel.ChangeRectangleParameters;

            firstGrid.Children.Add(widthTextBlock);

            #endregion

            #region Second Grid

            var secondGrid = new Grid();

            secondGrid.Children.Add(new TextBlock()
            {
                Text = "Ширина",
                Margin = new Thickness(0, 0, 64.4, 0)
            });

            var heightTExtBox = new TextBox()
            {
                Name = "RectangleHeight",
                Text = Math.Round(height, 2).ToString(),
                Margin = new Thickness(71, 0, -0.6, 0)
            };

            heightTExtBox.TextChanged += WorkPanel.ChangeRectangleParameters;

            secondGrid.Children.Add(heightTExtBox);

            #endregion

            WorkPanel.Parameters.Children.Add(new TextBlock()
            {
                Text = "Параметри",
                Margin = new Thickness(0, 0, 10, 0)
            });
            WorkPanel.Parameters.Children.Add(firstGrid);
            WorkPanel.Parameters.Children.Add(secondGrid);
        }

        public void InitTrapezeParameters(double upperBase, double lowerBase, double height)
        {
            WorkPanel.Parameters.Children.Clear();

            #region First Grid

            var firstGrid = new Grid();

            firstGrid.Children.Add(new TextBlock()
            {
                Text = "Верхня осн.",
                Margin = new Thickness(0, 0, 64.4, 0)
            });

            var upperTextBox = new TextBox()
            {
                Name = "TrapezeTopLine",
                Text = Math.Round(upperBase, 2).ToString(),
                Margin = new Thickness(71, 0, -0.6, 0)
            };

            upperTextBox.TextChanged += WorkPanel.ChangeTrapezeParameters;

            firstGrid.Children.Add(upperTextBox);

            #endregion

            #region Second Grid

            var secondGrid = new Grid();

            secondGrid.Children.Add(new TextBlock()
            {
                Text = "Нижня осн.",
                Margin = new Thickness(0, 0, 64.4, 0)
            });

            var lowerTextBox = new TextBox()
            {
                Name = "TrapezeBottomLine",
                Text = Math.Round(lowerBase, 2).ToString(),
                Margin = new Thickness(71, 0, -0.6, 0)
            };

            lowerTextBox.TextChanged += WorkPanel.ChangeTrapezeParameters;

            secondGrid.Children.Add(lowerTextBox);

            #endregion

            #region Third Grid

            var thirdGrid = new Grid();

            thirdGrid.Children.Add(new TextBlock()
            {
                Text = "Висота",
                Margin = new Thickness(0, 0, 64.4, 0)
            });

            var heightTextBox = new TextBox()
            {
                Name = "TrapezeHeight",
                Text = Math.Round(height, 2).ToString(),
                Margin = new Thickness(71, 0, -0.6, 0)
            };

            heightTextBox.TextChanged += WorkPanel.ChangeTrapezeParameters;

            thirdGrid.Children.Add(heightTextBox);

            #endregion

            WorkPanel.Parameters.Children.Add(new TextBlock()
            {
                Text = "Параметри",
                Margin = new Thickness(0, 0, 10, 0)
            });
            WorkPanel.Parameters.Children.Add(firstGrid);
            WorkPanel.Parameters.Children.Add(secondGrid);
            WorkPanel.Parameters.Children.Add(thirdGrid);

            ReWriteLines();
        }

        public void ChangeIsEnabled(bool isTop)
        {
            if (isTop)
            {
                if (WorkPanel.TopRadius.Text.IsEmpty() && WorkPanel.BottomRadius.Text.IsEmpty())
                {
                    WorkPanel.LeftRadius.IsEnabled = true;
                    WorkPanel.RightRadius.IsEnabled = true;
                }
                else
                {
                    WorkPanel.LeftRadius.IsEnabled = false;
                    WorkPanel.RightRadius.IsEnabled = false;
                }
            }
            else
            {
                if (WorkPanel.LeftRadius.Text.IsEmpty() && WorkPanel.RightRadius.Text.IsEmpty())
                {
                    WorkPanel.TopRadius.IsEnabled = true;
                    WorkPanel.BottomRadius.IsEnabled = true;
                }
                else
                {
                    WorkPanel.TopRadius.IsEnabled = false;
                    WorkPanel.BottomRadius.IsEnabled = false;
                }
            }
        }

        public void InitFiguresPanelContent()
        {
            WorkPanel.FiguresPanelContent.Children.Clear();

            WorkPanel.FiguresPanelContent.Children.Add(new TextBlock()
            {
                Text = "Саисок фігур",
                Margin = new Thickness(0, 0, 10.4, -0.4)
            });

            foreach (var item in WorkPanel.Data.Figures)
            {
                var button = new Button()
                {
                    Content = item.Name,
                    CommandParameter = item.ID,
                    Margin = new Thickness(0, 8, 0, 0)
                };

                button.Click += WorkPanel.DrawSelectedFigure;

                WorkPanel.FiguresPanelContent.Children.Add(button);
            }
        }

        #region Get / Set X:Y Fi0

        public double SetX(string x, bool zoom) => zoom ? WorkPanel.CenterX + (WorkPanel.DPI * x.ToDouble()) : x.ToDouble();

        public double SetY(string y, bool zoom) => zoom ? WorkPanel.CenterY - (WorkPanel.DPI * y.ToDouble()) : y.ToDouble();

        public double SetX(double x, bool zoom) => zoom ? WorkPanel.CenterX + (WorkPanel.DPI * x) : x;

        public double SetY(double y, bool zoom) => zoom ? WorkPanel.CenterY - (WorkPanel.DPI * y) : y;

        public double GetX(double x, bool zoom) => Math.Abs((x - WorkPanel.CenterX) / WorkPanel.DPI);

        public double GetY(double y, bool zoom) => Math.Abs((y - WorkPanel.MyCanvas.Margin.Top - WorkPanel.CenterY) / WorkPanel.DPI);

        public double GetX(double x, bool zoom, int round) => Math.Round(Math.Abs((x - WorkPanel.CenterX) / WorkPanel.DPI), round);

        public double GetY(double y, bool zoom, int round) => Math.Round(Math.Abs((y - WorkPanel.MyCanvas.Margin.Top - WorkPanel.CenterY) / WorkPanel.DPI), round);

        #endregion

        #region Private

        private List<MyLine> GetRectangleLines(List<MyLine> lines)
        {
            if (lines.Where(l => l.BottomRadius > 0).Count() > 0)
            {
                var radius = (double)lines[1].BottomRadius / 10;

                var leftLine = new MyLine(lines[3].X1, (Convert.ToDouble(lines[3].Y1) + radius).ToString(), lines[3].X2, lines[3].Y2, lines[3].TopRadius.ToString(), lines[3].BottomRadius.ToString());
                var rightLine = new MyLine(lines[1].X1, lines[1].Y1, lines[1].X2, (Convert.ToDouble(lines[1].Y2) + radius).ToString(), lines[1].TopRadius.ToString(), lines[1].BottomRadius.ToString());
                var bottomLine = new MyLine(Convert.ToDouble(lines[2].X1) - radius, lines[2].Y1.ToDouble(), Convert.ToDouble(lines[2].X2) + radius, lines[2].Y2.ToDouble(), lines[2].TopRadius, lines[2].BottomRadius);

                lines[3] = leftLine;
                lines[1] = rightLine;
                lines[2] = bottomLine;

                lines.AddRange(GetRectangleRadiusLines(bottomLine.X1.ToDouble(), bottomLine.Y1.ToDouble(), bottomLine.X1, rightLine.Y2, radius, 3 * Math.PI / 2));
                lines.AddRange(GetRectangleRadiusLines(leftLine.X1.ToDouble(), leftLine.Y1.ToDouble(), bottomLine.X2, leftLine.Y1, radius, Math.PI));
            }

            if (lines.Where(l => l.TopRadius > 0).Count() > 0)
            {
                var radius = (double)lines[1].TopRadius / 10;

                var topLine = new MyLine(lines[0].X1.ToDouble() + radius, lines[0].Y1.ToDouble(), Convert.ToDouble(lines[0].X2) - radius, lines[0].Y2.ToDouble(), lines[0].TopRadius, lines[0].BottomRadius);
                var rightLine = new MyLine(lines[1].X1, (Convert.ToDouble(lines[1].Y1) - radius).ToString(), lines[1].X2, lines[1].Y2, lines[1].TopRadius.ToString(), lines[1].BottomRadius.ToString());
                var leftLine = new MyLine(lines[3].X1, lines[3].Y1, lines[3].X2, (Convert.ToDouble(lines[3].Y2) - radius).ToString(), lines[3].TopRadius.ToString(), lines[3].BottomRadius.ToString());

                lines[0] = topLine;
                lines[1] = rightLine;
                lines[3] = leftLine;

                lines.AddRange(GetRectangleRadiusLines(topLine.X1.ToDouble(), topLine.Y1.ToDouble(), topLine.X1, leftLine.Y2, radius, Math.PI / 2));
                lines.AddRange(GetRectangleRadiusLines(rightLine.X1.ToDouble(), rightLine.Y1.ToDouble(), topLine.X2, rightLine.Y1, radius, 2 * Math.PI));
            }

            if (lines.Where(l => l.LeftRadius > 0).Count() > 0)
            {
                var radius = (double)lines[0].LeftRadius / 10;

                var topLine = new MyLine(lines[0].X1.ToDouble() + radius, lines[0].Y1.ToDouble(), lines[0].X2.ToDouble(), lines[0].Y2.ToDouble(), leftR: lines[0].LeftRadius, rightR: lines[0].RightRadius);
                var leftLine = new MyLine(lines[3].X1.ToDouble(), lines[3].Y1.ToDouble() + radius, lines[3].X2.ToDouble(), lines[3].Y2.ToDouble() - radius, leftR: lines[3].LeftRadius, rightR: lines[3].RightRadius);
                var bottomLine = new MyLine(lines[2].X1.ToDouble(), lines[2].Y1.ToDouble(), lines[2].X2.ToDouble() + radius, lines[2].Y2.ToDouble(), leftR: lines[2].LeftRadius, rightR: lines[2].RightRadius);

                lines[0] = topLine;
                lines[3] = leftLine;
                lines[2] = bottomLine;

                lines.AddRange(GetRectangleRadiusLines(topLine.X1.ToDouble(), topLine.Y1.ToDouble(), topLine.X1, leftLine.Y2, radius, Math.PI / 2));
                lines.AddRange(GetRectangleRadiusLines(leftLine.X1.ToDouble(), leftLine.Y1.ToDouble(), bottomLine.X2, leftLine.Y1, radius, Math.PI));
            }

            if (lines.Where(l => l.RightRadius > 0).Count() > 0)
            {
                var radius = (double)lines[0].RightRadius / 10;

                var topLine = new MyLine(lines[0].X1.ToDouble(), lines[0].Y1.ToDouble(), lines[0].X2.ToDouble() - radius, lines[0].Y2.ToDouble(), leftR: lines[0].LeftRadius, rightR: lines[0].RightRadius);
                var rightLine = new MyLine(lines[1].X1.ToDouble(), lines[1].Y1.ToDouble() - radius, lines[1].X2.ToDouble(), lines[1].Y2.ToDouble() + radius, leftR: lines[1].LeftRadius, rightR: lines[1].RightRadius);
                var bottomLine = new MyLine(lines[2].X1.ToDouble() - radius, lines[2].Y1.ToDouble(), lines[2].X2.ToDouble(), lines[2].Y2.ToDouble(), leftR: lines[2].LeftRadius, rightR: lines[2].RightRadius);

                lines[0] = topLine;
                lines[1] = rightLine;
                lines[2] = bottomLine;

                lines.AddRange(GetRectangleRadiusLines(rightLine.X1.ToDouble(), rightLine.Y1.ToDouble(), topLine.X2, rightLine.Y1, radius, 2 * Math.PI));
                lines.AddRange(GetRectangleRadiusLines(bottomLine.X1.ToDouble(), bottomLine.Y1.ToDouble(), bottomLine.X1, rightLine.Y2, radius, 3 * Math.PI / 2));
            }

            return lines;
        }

        private List<MyLine> GetRectangleRadiusLines(double prevX, double prevY, string x0, string y0, double radius, double Fi0)
        {
            var n = 100;
            var lines = new List<MyLine>();

            for (int i = 0; i < n; i++)
            {
                var x = radius * Math.Cos(GetFi(i, n, Fi0)) + x0.ToDouble();
                var y = radius * Math.Sin(GetFi(i, n, Fi0)) + y0.ToDouble();

                //if (x > x0.ToDouble())
                //{
                lines.Add(new MyLine(prevX, prevY, x, y));
                //}

                prevX = x;
                prevY = y;
            }

            return lines;
        }

        private double GetFi(int i, int n, double Fi0) => Fi0 + Math.PI / 2 / n * i;

        private List<MyLine> GetTrapezeLines(List<MyLine> lines)
        {
            if (lines.Where(l => l.BottomRadius > 0).Count() > 0)
            {
                var radius = (double)lines[1].BottomRadius / 10;

                var leftLine = new MyLine(lines[3].X1, (Convert.ToDouble(lines[3].Y1) + radius).ToString(), lines[3].X2, lines[3].Y2, lines[3].TopRadius.ToString(), lines[3].BottomRadius.ToString());
                var rightLine = new MyLine(lines[1].X1, lines[1].Y1, lines[1].X2, (Convert.ToDouble(lines[1].Y2) + radius).ToString(), lines[1].TopRadius.ToString(), lines[1].BottomRadius.ToString());
                var bottomLine = new MyLine(Convert.ToDouble(lines[2].X1) - radius, lines[2].Y1.ToDouble(), Convert.ToDouble(lines[2].X2) + radius, lines[2].Y2.ToDouble(), lines[2].TopRadius, lines[2].BottomRadius);

                lines[3] = leftLine;
                lines[1] = rightLine;
                lines[2] = bottomLine;

                var Fi2 = Math.Atan((lines[1].X1.ToDouble() - lines[1].X2.ToDouble()) / (lines[1].Y1.ToDouble() - lines[1].Y2.ToDouble()));

                var Fi = Math.PI - Fi2;

                var Fi4 = Math.Atan((lines[3].X2.ToDouble() - lines[3].X1.ToDouble()) / (lines[3].Y2.ToDouble() - lines[3].Y1.ToDouble()));

                var Fi3 = Math.PI - Fi4;

                lines.AddRange(GetRectangleRadiusLines(bottomLine.X1.ToDouble(), bottomLine.Y1.ToDouble(), bottomLine.X1, rightLine.Y2, radius, 3 * Math.PI / 2));
                lines.AddRange(GetRectangleRadiusLines(leftLine.X1.ToDouble(), leftLine.Y1.ToDouble(), bottomLine.X2, leftLine.Y1, radius, Math.PI));
            }

            if (lines.Where(l => l.TopRadius > 0).Count() > 0)
            {
                var radius = (double)lines[1].TopRadius / 10;

                var topLine = new MyLine(lines[0].X1.ToDouble() + radius, lines[0].Y1.ToDouble(), Convert.ToDouble(lines[0].X2) - radius, lines[0].Y2.ToDouble(), lines[0].TopRadius, lines[0].BottomRadius);
                var rightLine = new MyLine(lines[1].X1, (Convert.ToDouble(lines[1].Y1) - radius).ToString(), lines[1].X2, lines[1].Y2, lines[1].TopRadius.ToString(), lines[1].BottomRadius.ToString());
                var leftLine = new MyLine(lines[3].X1, lines[3].Y1, lines[3].X2, (Convert.ToDouble(lines[3].Y2) - radius).ToString(), lines[3].TopRadius.ToString(), lines[3].BottomRadius.ToString());

                lines[0] = topLine;
                lines[1] = rightLine;
                lines[3] = leftLine;

                var Fi2 = Math.Atan((lines[1].X1.ToDouble() - lines[1].X2.ToDouble()) / (lines[1].Y1.ToDouble() - lines[1].Y2.ToDouble()));// + Math.PI;

                var Fi = Math.PI - Fi2;

                var Fi4 = Math.Atan((lines[0].X1.ToDouble() - lines[3].X1.ToDouble()) / (lines[0].Y1.ToDouble() - lines[3].Y1.ToDouble()));

                var Fi3 = Math.PI - Fi4;

                lines.AddRange(GetTrapezeRadiusLinesLeft(topLine.X1.ToDouble(), topLine.Y1.ToDouble(), topLine.X1, leftLine.Y2, -Fi3, radius, Math.PI / 2/*Fi3*/));
                lines.AddRange(GetTrapezeRadiusLinesRight(rightLine.X1.ToDouble(), rightLine.Y1.ToDouble(), topLine.X2, rightLine.Y1, radius, 2 * /*Math.PI*/Fi4));


            }

            if (lines.Where(l => l.LeftRadius > 0).Count() > 0)
            {

            }

            if (lines.Where(l => l.RightRadius > 0).Count() > 0)
            {

            }

            return lines;
        }

        private List<MyLine> GetTrapezeRadiusLinesRight(double prevX, double prevY, string x0, string y0, double radius, double Fi0)
        {
            var n = 100;
            var lines = new List<MyLine>();

            for (int i = 0; i < n; i++)
            {
                var x = radius * Math.Cos(GetFi(i, n, Fi0)) + x0.ToDouble();
                var y = radius * Math.Sin(GetFi(i, n, Fi0)) + y0.ToDouble();

                if (x > x0.ToDouble())
                {
                    lines.Add(new MyLine(prevX, prevY, x, y));
                }

                prevX = x;
                prevY = y;
            }

            return lines;
        }

        private List<MyLine> GetTrapezeRadiusLinesLeft(double prevX, double prevY, string x0, string y0, double Fi3, double radius, double Fi0)
        {
            var n = 36;
            var lines = new List<MyLine>();

            for (int i = 0; i < n; i++)
            {
                var x = radius * Math.Cos(GetFi(i, n, Fi0)) + x0.ToDouble();
                var y = radius * Math.Sin(GetFi(i, n, Fi0)) + y0.ToDouble();

                if (x > Fi3)
                {
                    lines.Add(new MyLine(prevX, prevY, x, y));
                }

                prevX = x;
                prevY = y;
            }

            return lines;
        }

        #endregion
    }
}
