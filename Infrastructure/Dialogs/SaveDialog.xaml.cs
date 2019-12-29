using Microsoft.Win32;
using Project.Infrastructure.Interfaces;
using Project.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Project
{
    public partial class SaveDialog : Window, IDialog
    {
        public Data Result { get; set; }

        public SaveDialog(Data data = null)
        {
            Result = data;

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);

            RoutedCommand command = new RoutedCommand();

            command.InputGestures.Add(new KeyGesture(Key.Escape, ModifierKeys.None));
            CommandBindings.Add(new CommandBinding(command, Cancel));

            InitializeComponent();
        }

        private void OnInit(object sender, RoutedEventArgs e)
        {
            if (ModelName != null && Result != null)
            {
                if (!string.IsNullOrEmpty(Result.Name))
                {
                    ModelName.Text = Result.Name;
                }

                if (!string.IsNullOrEmpty(Result.Material))
                {
                    Material.Text = Result.Material;
                }
            }
        }

        private void ValidateField(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ModelName.Text))
            {
                SaveButton.IsEnabled = false;
                ModelName.BorderBrush = Brushes.Red;
            }
            else if (string.IsNullOrWhiteSpace(Material.Name))
            {
                SaveButton.IsEnabled = false;
                Material.BorderBrush = Brushes.Red;
            }
            else
            {
                SaveButton.IsEnabled = true;

                Material.BorderBrush = Brushes.Gray;
                ModelName.BorderBrush = Brushes.Gray;
            }
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            try
            {
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var lines = new List<string>
                {
                    ModelName.Text,
                    Material.Text,
                    Result.Figures.Count.ToString()
                };

                lines.AddRange(Result.Figures.Select(f => f.Name).ToList());
                lines.AddRange(Result.Figures.Select(f => (f.BaseLines.Count + 1).ToString()).ToList());

                foreach (var figure in Result.Figures)
                {
                    var printName = true;

                    foreach (var line in figure.BaseLines)
                    {
                        if (printName)
                        {
                            printName = false;
                            lines.Add($"{line.X1} {line.Y1} {figure.Name}");
                        }
                        else
                        {
                            lines.Add($"{line.X1} {line.Y1}");
                        }
                    }

                    lines.Add($"{figure.BaseLines[0].X1} {figure.BaseLines[0].Y1}");
                }

                var savedDialod = new SaveFileDialog();

                if (savedDialod.ShowDialog() == true)
                {
                    File.AppendAllLines(savedDialod.FileName, lines);
                }

                DialogResult = true;
            }
            catch
            {
                DialogResult = false;
            }
        }

        public void Cancel(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
