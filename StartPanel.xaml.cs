using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Project
{
    public partial class StartPanel : Window
    {
        public StartPanel()
        {
            InitializeComponent();

            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);

            Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Media/Images/Zastavka.bmp", UriKind.Absolute))
            };
        }

        private void NewProject(object sender, RoutedEventArgs e)
        {
            var dialog = new WorkPanel();

            dialog.ShowDialog();
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            var dialog = new WorkPanel(true);

            dialog.ShowDialog();
        }
    }
}
