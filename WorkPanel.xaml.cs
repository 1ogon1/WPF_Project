using Project.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Input;
using Project.Infrastructure.Helper;
using System.IO;
using System.Linq;
using System.Threading;

namespace Project
{
    public enum FigureType
    {
        none,
        trapeze,
        rectangle
    }

    public partial class WorkPanel : Window
    {
        #region Constructor

        public WorkPanel(bool openFile = false)
        {
            InitializeComponent();

            Helper = new Helper(this);

            IsOpenFile = openFile;
        }

        #endregion

        #region Properties

        #region Public

        public bool IsInit = true;

        public double Zoom = 1;
        public double DPI = default(double);
        public double CenterX = default(double);
        public double CenterY = default(double);

        public Data Data = new Data();
        public Figure Figure = new Figure();

        public readonly string HideMenu = "sbHideRightMenu";
        public readonly string ShowMenu = "sbShowRightMenu";

        public readonly string HideFiguresPanel = "sbHideFiguresPanel";
        public readonly string ShowFiguresPanel = "sbShowFiguresPanel";

        #endregion

        #region Private

        private Helper Helper;
        private bool? IsOpenFile = null;

        private bool CanSetMousePoint = false;

        private double DPIDefault { get { return 46.456692913; } }

        #endregion

        #endregion

        #region Methods

        #region Public 

        public void Init(bool recalc = false, bool clear = false)
        {
            if (IsInit || recalc)
            {
                Helper.Update(this);

                if (clear)
                {
                    Zoom = 1;

                    Figure = new Figure();

                    DPI = DPIDefault * Zoom;
                }

                if (recalc)
                {
                    MyCanvas.Children.Clear();
                }
                else
                {
                    IsInit = false;
                }

                if (DPI < 1)
                {
                    DPI = DPIDefault;
                }

                ZoomBox.Text = $"Zoom: 1:{Zoom}";

                CenterX = MyCanvas.ActualWidth / 2;
                CenterY = MyCanvas.ActualHeight / 2;

                MyCanvas.Children.AddRange(Helper.DrawLine(CenterX, 0, CenterX, MyCanvas.ActualHeight, false, 1)); //vertical line

                MyCanvas.Children.AddRange(Helper.DrawLine(0, CenterY, MyCanvas.ActualWidth, CenterY, false, 1)); //horizontal line

                for (double i = CenterY; i > 0; i -= DPI)
                {
                    if (i != CenterY)
                    {
                        MyCanvas.Children.AddRange(Helper.DrawLine(CenterX - 5 * Zoom, i, CenterX + 5 * Zoom, i, false, 1));
                    }
                }

                for (double i = CenterX; i < MyCanvas.ActualWidth; i += DPI)
                {
                    if (i != CenterX)
                    {
                        MyCanvas.Children.AddRange(Helper.DrawLine(i, CenterY - 5 * Zoom, i, CenterY + 5 * Zoom, false, 1));
                    }
                }

                for (double i = CenterY; i < MyCanvas.ActualHeight; i += DPI)
                {
                    if (i != CenterY)
                    {
                        MyCanvas.Children.AddRange(Helper.DrawLine(CenterX - 5 * Zoom, i, CenterX + 5 * Zoom, i, false, 1));
                    }
                }

                for (double i = CenterX; i > 0; i -= DPI)
                {
                    if (i != CenterX)
                    {
                        MyCanvas.Children.AddRange(Helper.DrawLine(i, CenterY - 5 * Zoom, i, CenterY + 5 * Zoom, false, 1));
                    }
                }

                if (recalc && !clear)
                {
                    Helper.ReWriteLines();
                }

                if (IsOpenFile.HasValue && IsOpenFile.Value == true)
                {
                    IsOpenFile = null;

                    UploadFile();
                }
            }
        }

        public void DrawSelectedFigure(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            var getFigure = Data.Figures.FirstOrDefault(f => f.ID == (int)button.CommandParameter);

            if (getFigure != null)
            {
                Figure = getFigure;

                if (Figure.Type == FigureType.rectangle)
                {
                    Helper.InitRectangleParameters(Math.Abs(Figure.BaseLines[0].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[0].Y1.ToDouble() * 2));
                }
                else
                {
                    Helper.InitTrapezeParameters(Math.Abs(Figure.BaseLines[0].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[2].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[0].Y1.ToDouble()) + Math.Abs(Figure.BaseLines[2].Y1.ToDouble()));
                }

                Helper.Update(this);
                Helper.InitFiguresPanelContent();
                Helper.DrawFigure(Figure.BaseLines[0], Figure.BaseLines[1], Figure.BaseLines[2], Figure.BaseLines[3], Figure.Type, Figure.Name, Figure.ID);
                RightMenuShow.IsEnabled = true;
                RightMenuHide.IsEnabled = true;

                SetRadius(Figure.BaseLines[0].TopRadius, Figure.BaseLines[2].RightRadius, Figure.BaseLines[1].BottomRadius, Figure.BaseLines[3].LeftRadius);
            }
        }

        public void SetRadius(decimal top, decimal right, decimal bottom, decimal left)
        {
            TopRadius.Text = top.ToString();
            LeftRadius.Text = left.ToString();
            RightRadius.Text = right.ToString();
            BottomRadius.Text = bottom.ToString();
        }

        public void ChangeRectangleParameters(object sender, RoutedEventArgs e)
        {
            var isValid = true;
            double? width = null;
            double? height = null;

            foreach (var element in Parameters.Children)
            {
                if (element is Grid grid)
                {
                    foreach (var item in grid.Children)
                    {
                        if (item is TextBox box && (box.Name == "RectangleWidth" || box.Name == "RectangleHeight"))
                        {
                            if (!box.Text.IsDouble())
                            {
                                isValid = false;
                            }
                            else
                            {
                                if (box.Name == "RectangleWidth" && box.Text.ToDouble() != 0)
                                {
                                    width = Math.Abs(box.Text.ToDouble());
                                }

                                if (box.Name == "RectangleHeight" && box.Text.ToDouble() != 0)
                                {
                                    height = Math.Abs(box.Text.ToDouble());
                                }
                            }
                        }
                    }
                }
            }

            if (isValid && width.HasValue && height.HasValue)
            {
                Helper.DrawFigure(new MyLine(-(width.Value / 2), height.Value / 2, width.Value / 2, height.Value / 2, Figure.BaseLines[0].TopRadius, Figure.BaseLines[0].BottomRadius, Figure.BaseLines[0].LeftRadius, Figure.BaseLines[0].RightRadius),
                                  new MyLine(width.Value / 2, height.Value / 2, width.Value / 2, -(height.Value / 2), Figure.BaseLines[1].TopRadius, Figure.BaseLines[1].BottomRadius, Figure.BaseLines[1].LeftRadius, Figure.BaseLines[1].RightRadius),
                                  new MyLine(width.Value / 2, -(height.Value / 2), -(width.Value / 2), -(height.Value / 2), Figure.BaseLines[2].TopRadius, Figure.BaseLines[2].BottomRadius, Figure.BaseLines[2].LeftRadius, Figure.BaseLines[2].RightRadius),
                                  new MyLine(-(width.Value / 2), -(height.Value / 2), -(width.Value / 2), height.Value / 2, Figure.BaseLines[3].TopRadius, Figure.BaseLines[3].BottomRadius, Figure.BaseLines[3].LeftRadius, Figure.BaseLines[3].RightRadius),
                                  FigureType.rectangle,
                                  Figure.Name,
                                  Figure.ID);

                Init(true);
            }
        }

        public void ChangeTrapezeParameters(object sender, RoutedEventArgs e)
        {
            var isValid = true;
            double? top = null;
            double? bottom = null;
            double? height = null;

            foreach (var element in Parameters.Children)
            {
                if (element is Grid grid)
                {
                    foreach (var item in grid.Children)
                    {
                        if (item is TextBox box && (box.Name == "TrapezeTopLine" || box.Name == "TrapezeBottomLine" || box.Name == "TrapezeHeight"))
                        {
                            if (!box.Text.IsDouble())
                            {
                                isValid = false;
                            }
                            else
                            {
                                if (box.Name == "TrapezeTopLine" && box.Text.ToDouble() != 0)
                                {
                                    top = Math.Abs(box.Text.ToDouble());
                                }

                                if (box.Name == "TrapezeBottomLine" && box.Text.ToDouble() != 0)
                                {
                                    bottom = Math.Abs(box.Text.ToDouble());
                                }

                                if (box.Name == "TrapezeHeight" && box.Text.ToDouble() != 0)
                                {
                                    height = Math.Abs(box.Text.ToDouble());
                                }
                            }
                        }
                    }
                }
            }

            if (isValid && top.HasValue && bottom.HasValue && height.HasValue)
            {
                Helper.DrawFigure(new MyLine(-(top.Value / 2), height.Value / 2, top.Value / 2, height.Value / 2, Figure.BaseLines[0].TopRadius, Figure.BaseLines[0].BottomRadius, Figure.BaseLines[0].LeftRadius, Figure.BaseLines[0].RightRadius),
                                  new MyLine(top.Value / 2, height.Value / 2, bottom.Value / 2, -(height.Value / 2), Figure.BaseLines[1].TopRadius, Figure.BaseLines[1].BottomRadius, Figure.BaseLines[1].LeftRadius, Figure.BaseLines[1].RightRadius),
                                  new MyLine(bottom.Value / 2, -(height.Value / 2), -(bottom.Value / 2), -(height.Value / 2), Figure.BaseLines[2].TopRadius, Figure.BaseLines[2].BottomRadius, Figure.BaseLines[2].LeftRadius, Figure.BaseLines[2].RightRadius),
                                  new MyLine(-(bottom.Value / 2), -(height.Value / 2), -(top.Value / 2), height.Value / 2, Figure.BaseLines[3].TopRadius, Figure.BaseLines[3].BottomRadius, Figure.BaseLines[3].LeftRadius, Figure.BaseLines[3].RightRadius),
                                  FigureType.trapeze,
                                  Figure.Name,
                                  Figure.ID);

                Init(true);
            }
        }

        public void Rectangle(object sender, RoutedEventArgs e)
        {
            RightMenuShow.IsEnabled = true;
            RightMenuHide.IsEnabled = true;
            Figure = new Figure() { Type = FigureType.rectangle };

            Helper.Update(this);
            SetRadius(0, 0, 0, 0);
            Helper.InitRectangleParameters(4, 2);

            Helper.DrawFigure(new MyLine(-2, 1, 2, 1), new MyLine(2, 1, 2, -1), new MyLine(2, -1, -2, -1), new MyLine(-2, -1, -2, 1), FigureType.rectangle);
        }

        public void Trapeze(object sender, RoutedEventArgs e)
        {
            RightMenuShow.IsEnabled = true;
            RightMenuHide.IsEnabled = true;
            Figure = new Figure() { Type = FigureType.trapeze };

            Helper.Update(this);
            SetRadius(0, 0, 0, 0);
            Helper.InitTrapezeParameters(3, 4, 2);

            Helper.DrawFigure(new MyLine(-1.5, 1, 1.5, 1), new MyLine(1.5, 1, 2, -1), new MyLine(2, -1, -2, -1), new MyLine(-2, -1, -1.5, 1), FigureType.trapeze);
        }

        public void SetRectanglePoints(object sender, RoutedEventArgs e)
        {
            Helper.Clear();
            RightMenuShow.IsEnabled = true;
            RightMenuHide.IsEnabled = true;

            CanSetMousePoint = true;
            Figure = new Figure() { Type = FigureType.rectangle };
        }

        public void SetTrapezePoints(object sender, RoutedEventArgs e)
        {
            Helper.Clear();
            RightMenuShow.IsEnabled = true;
            RightMenuHide.IsEnabled = true;

            CanSetMousePoint = true;
            Figure = new Figure() { Type = FigureType.trapeze };
        }

        public void ZoomIncrease(object sender, RoutedEventArgs e)
        {
            Zoom += 0.1;

            DPI = DPIDefault * Zoom;

            Init(true);
        }

        public void ZoomDecrease(object sender, RoutedEventArgs e)
        {
            if (Zoom <= 0.2) { Zoom = 0.1; }
            else { Zoom -= 0.1; }

            DPI = DPIDefault * Zoom;

            Init(true);
        }

        public void ClearCanvas(object sender, RoutedEventArgs e) => Helper.Clear(true);

        public void Exit(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        public void SaveToFile(object sender, RoutedEventArgs e)
        {
            if (Data.Figures.Count > 0)
            {
                var dialog = new SaveDialog(Data);

                if (dialog.ShowDialog() == true)
                {
                    Data.Name = dialog.Result.Name;
                    Data.Material = dialog.Result.Material;

                    Helper.Clear();
                }
                else
                {

                }
            }
        }

        public void AddFigure(object sender, RoutedEventArgs e)
        {
            if (Figure != null && Figure.BaseLines.Count == 4)
            {
                var dialog = new AddFigureDialog(Figure.Name);

                if (dialog.ShowDialog() == true)
                {
                    if (Data == null) Data = new Data();

                    Figure.Name = dialog.Result;

                    if (Figure.ID == -1)
                    {
                        Figure.ID = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

                        Data.Figures.Add(Figure);
                    }
                    else
                    {
                        var index = Data.Figures.IndexOf(Data.Figures.Where(f => f.ID == Figure.ID).FirstOrDefault());

                        if (index > -1)
                        {
                            Data.Figures.RemoveAt(index);
                            Data.Figures.Insert(index, Figure);
                        }
                    }

                    Helper.InitFiguresPanelContent();
                }
            }
        }

        public void OpenFile(object sender, RoutedEventArgs e) => UploadFile();

        #endregion

        #region Private

        private void OnLoad(object sender, RoutedEventArgs e) => Init();

        private void ChangeCoordinates(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox box && box.IsFocused)
            {
                var data = box.Name.Split('_').ToList();

                var index = (int)(LineType)Enum.Parse(typeof(LineType), data[0]);

                if (box.Name.Contains("X0"))
                {
                    Figure.BaseLines[index].X1 = box.Text;
                }
                else if (box.Name.Contains("Y0"))
                {
                    Figure.BaseLines[index].Y1 = box.Text;
                }
                else if (box.Name.Contains("X1"))
                {
                    Figure.BaseLines[index].X2 = box.Text;
                }
                else if (box.Name.Contains("Y1"))
                {
                    Figure.BaseLines[index].Y2 = box.Text;
                }

                //Figure.BaseLines[3].X1 = box.Text;
                //Figure.BaseLines[3].Y1 = box.Text;
                //Figure.BaseLines[3].X2 = box.Text;
                //Figure.BaseLines[3].Y2 = box.Text;

                //Figure.BaseLines[1].X1 = box.Text;
                //Figure.BaseLines[1].Y1 = box.Text;
                //Figure.BaseLines[1].X2 = box.Text;
                //Figure.BaseLines[1].Y2 = box.Text;

                //Figure.BaseLines[2].X1 = box.Text;
                //Figure.BaseLines[2].Y1 = box.Text;
                //Figure.BaseLines[2].X2 = box.Text;
                //Figure.BaseLines[2].Y2 = box.Text;

                Init(true);
            }
        }

        private void SetPiont(object sender, MouseButtonEventArgs e)
        {
            if (CanSetMousePoint && Figure.Type != FigureType.none)
            {
                if (Figure.Type == FigureType.rectangle)
                {
                    var posX = Helper.GetX(e.GetPosition(this).X, false);
                    var posY = Helper.GetY(e.GetPosition(this).Y, false);

                    CanSetMousePoint = false;

                    Helper.InitRectangleParameters(Math.Abs(posX) * 2, Math.Abs(posY) * 2);

                    Helper.DrawFigure(new MyLine(-posX, posY, posX, posY),
                                      new MyLine(posX, posY, posX, -posY),
                                      new MyLine(posX, -posY, -posX, -posY),
                                      new MyLine(-posX, -posY, -posX, posY),
                                      FigureType.rectangle);
                }
                else
                {
                    if (Application.Current.Resources.ContainsKey("firstPoint"))
                    {
                        var values = Application.Current.Resources["firstPoint"] as List<double>;

                        var firstPosX = values[0];
                        var firstPosY = values[1];

                        var secondPosX = Helper.GetX(e.GetPosition(this).X, false);
                        var secondPosY = Helper.GetY(e.GetPosition(this).Y, false);

                        if (firstPosY < secondPosY)
                        {
                            Helper.InitTrapezeParameters(Math.Abs(firstPosX) * 2, Math.Abs(secondPosX) * 2, Math.Abs(firstPosY) * 2);

                            Helper.DrawFigure(new MyLine(-firstPosX, firstPosY, firstPosX, firstPosY),
                                              new MyLine(firstPosX, firstPosY, secondPosX, -secondPosY),
                                              new MyLine(secondPosX, -secondPosY, -secondPosX, -secondPosY),
                                              new MyLine(-secondPosX, -secondPosY, -firstPosX, firstPosY),
                                              FigureType.trapeze);
                        }
                        else
                        {
                            Helper.InitTrapezeParameters(Math.Abs(secondPosX) * 2, Math.Abs(firstPosX) * 2, Math.Abs(firstPosY) * 2);

                            Helper.DrawFigure(new MyLine(-secondPosX, secondPosY, secondPosX, secondPosY),
                                              new MyLine(secondPosX, secondPosY, firstPosX, -firstPosY),
                                              new MyLine(firstPosX, -firstPosY, -firstPosX, -firstPosY),
                                              new MyLine(-firstPosX, -firstPosY, -secondPosX, secondPosY),
                                              FigureType.trapeze);
                        }

                        CanSetMousePoint = false;

                        Application.Current.Resources.Remove("firstPoint");
                    }
                    else
                    {
                        Application.Current.Resources.Add("firstPoint", new List<double>() { Helper.GetX(e.GetPosition(this).X, false), Helper.GetY(e.GetPosition(this).Y, false) });
                    }
                }
            }
        }

        private void ChangeRadiusTop(object sender, RoutedEventArgs e)
        {
            if (Figure.BaseLines.Count > 0)
            {
                Helper.ChangeIsEnabled(true);

                var value = (sender as TextBox).Text;

                if (decimal.TryParse(string.IsNullOrWhiteSpace(value) ? "0" : value.Trim(), out decimal radius))
                {
                    Figure.BaseLines[3].TopRadius = radius;
                    Figure.BaseLines[0].TopRadius = radius;
                    Figure.BaseLines[1].TopRadius = radius;

                    Init(true);
                }
            }
        }

        private void ChangeRadiusBottom(object sender, RoutedEventArgs e)
        {
            if (Figure.BaseLines.Count > 0)
            {
                Helper.ChangeIsEnabled(true);

                var value = (sender as TextBox).Text;

                if (decimal.TryParse(string.IsNullOrWhiteSpace(value) ? "0" : value.Trim(), out decimal radius))
                {
                    Figure.BaseLines[1].BottomRadius = radius;
                    Figure.BaseLines[2].BottomRadius = radius;
                    Figure.BaseLines[3].BottomRadius = radius;

                    Init(true);
                }
            }
        }

        private void ChangeRadiusLeft(object sender, RoutedEventArgs e)
        {
            if (Figure.BaseLines.Count > 0)
            {
                Helper.ChangeIsEnabled(false);

                var value = (sender as TextBox).Text;

                if (decimal.TryParse(string.IsNullOrWhiteSpace(value) ? "0" : value.Trim(), out decimal radius))
                {
                    Figure.BaseLines[0].LeftRadius = radius;
                    Figure.BaseLines[3].LeftRadius = radius;
                    Figure.BaseLines[2].LeftRadius = radius;

                    Init(true);
                }
            }
        }

        private void ChangeRadiusRight(object sender, RoutedEventArgs e)
        {
            if (Figure.BaseLines.Count > 0)
            {
                Helper.ChangeIsEnabled(false);

                var value = (sender as TextBox).Text;

                if (decimal.TryParse(string.IsNullOrWhiteSpace(value) ? "0" : value.Trim(), out decimal radius))
                {
                    Figure.BaseLines[0].RightRadius = radius;
                    Figure.BaseLines[1].RightRadius = radius;
                    Figure.BaseLines[2].RightRadius = radius;

                    Init(true);
                }
            }
        }

        private void RightMenuHideHandle(object sender, RoutedEventArgs e) => Helper.ShowHideMenu(HideMenu, HideFiguresPanel);

        private void RightMenuShowHandle(object sender, RoutedEventArgs e) => Helper.ShowHideMenu(ShowMenu, ShowFiguresPanel);

        private void FiguresPanelHideHandle(object sender, RoutedEventArgs e) => Helper.ShowHideMenu(HideMenu, HideFiguresPanel, false);

        private void FiguresPanelShowHandle(object sender, RoutedEventArgs e) => Helper.ShowHideMenu(ShowMenu, ShowFiguresPanel, false);

        private void OnMouseMove(object sender, MouseEventArgs e) => MousePosition.Text = $"X: {Helper.GetX(e.GetPosition(this).X, false, 2).ToString()}{Environment.NewLine}" +
                                                                                          $"Y: {Helper.GetY(e.GetPosition(this).Y, false, 2).ToString()}";

        private void UploadFile()
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                var extensionIndex = dialog.FileName.IndexOf(".txt");

                if (extensionIndex > -1 && extensionIndex == dialog.FileName.Length - 4)
                {
                    int index = 0;
                    var readData = File.ReadAllLines(dialog.FileName);

                    if (readData != null && readData.Length >= 8)
                    {
                        if (Data == null) Data = new Data();

                        if (!string.IsNullOrWhiteSpace(readData[index]))
                        {
                            Data.Name = readData[index++].Trim();

                            if (!string.IsNullOrWhiteSpace(readData[index]))
                            {
                                Data.Material = readData[index++].Trim();

                                if (!string.IsNullOrWhiteSpace(readData[index]) && int.TryParse(readData[index++].Trim(), out int figureCount))
                                {
                                    try
                                    {
                                        var vertices = new Dictionary<string, int>();

                                        for (var i = index; i < index + figureCount; i++)
                                        {

                                            if (!string.IsNullOrWhiteSpace(readData[i]) && !string.IsNullOrWhiteSpace(readData[i + figureCount]) && int.TryParse(readData[i + figureCount].Trim(), out int value) && value > 0)
                                            {
                                                vertices.Add(readData[i].Trim(), value);
                                            }
                                            else
                                            {
                                                //Helper.Error(type);

                                                return;
                                            }
                                        }

                                        index += figureCount * 2;

                                        foreach (var item in vertices)
                                        {
                                            var figure = new Figure(item.Key)
                                            {
                                                ID = GetID()
                                            };

                                            for (int i = index; i < index + item.Value - 1; i++)
                                            {
                                                if (readData[i].GetCoordinates(out double x1, out double y1) && readData[i + 1].GetCoordinates(out double x2, out double y2))
                                                {
                                                    figure.BaseLines.Add(new MyLine(x1, y1, x2, y2));
                                                }
                                                else
                                                {
                                                    //Helper.Error(type);

                                                    return;
                                                }
                                            }

                                            if (figure.BaseLines.Count != 4)
                                            {
                                                //Helper.Error(type);

                                                return;
                                            }

                                            index += item.Value;
                                            Data.Figures.Add(figure);
                                        }

                                        //Helper.FiguresPanel(ShowFiguresPanel);

                                        Figure = Data.Figures[0];

                                        if (Math.Abs(Figure.BaseLines[0].X1.ToDouble()) == Math.Abs(Figure.BaseLines[2].X1.ToDouble()))
                                        {
                                            Figure.Type = FigureType.rectangle;
                                            Helper.InitRectangleParameters(Math.Abs(Figure.BaseLines[0].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[0].Y1.ToDouble() * 2));
                                        }
                                        else
                                        {
                                            Figure.Type = FigureType.trapeze;
                                            Helper.InitTrapezeParameters(Math.Abs(Figure.BaseLines[0].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[2].X1.ToDouble() * 2), Math.Abs(Figure.BaseLines[0].Y1.ToDouble() * 2));
                                        }
                                        Helper.Update(this);
                                        Helper.InitFiguresPanelContent();
                                        Helper.DrawFigure(Figure.BaseLines[0], Figure.BaseLines[1], Figure.BaseLines[2], Figure.BaseLines[3], Figure.Type);
                                        RightMenuShow.IsEnabled = true;
                                        RightMenuHide.IsEnabled = true;

                                        return;
                                    }
                                    catch
                                    {
                                        //Helper.Error(type);

                                        return;
                                    }
                                }

                                //Helper.Error(type);

                                return;
                            }

                            //Helper.Error(type);

                            return;
                        }

                        //Helper.Error(type);

                        return;
                    }

                    //Helper.Error(type);

                    return;
                }

                //Helper.Error(type);

                return;
            }

            //Helper.Error(type);

            return;
        }

        #endregion

        #endregion

        private int GetID()
        {
            Random rnd = new Random();

            int id = rnd.Next(int.MaxValue);

            if (Data.Figures.FirstOrDefault(f => f.ID == id) != null)
            {
                id = GetID();
            }

            return id;
        }
    }


}
