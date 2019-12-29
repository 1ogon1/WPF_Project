using Project.Infrastructure.Interfaces;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Project
{
    public partial class AddFigureDialog : Window, IDialog
    {
        public string Result { get; set; }

        public AddFigureDialog(string name = null)
        {
            Result = name;
            InitializeComponent();

            SaveButton.IsEnabled = false;
            Left = (SystemParameters.PrimaryScreenWidth / 2) - (Width / 2);
            Top = (SystemParameters.PrimaryScreenHeight / 2) - (Height / 2);

            RoutedCommand command = new RoutedCommand();

            command.InputGestures.Add(new KeyGesture(Key.Escape, ModifierKeys.None));
            CommandBindings.Add(new CommandBinding(command, Cancel));
        }

        private void OnLoad(object sender, RoutedEventArgs e) => Name.Text = Result;

        private void ValidateField(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                SaveButton.IsEnabled = false;
                Name.BorderBrush = Brushes.Red;
            }
            else
            {
                SaveButton.IsEnabled = true;
                Name.BorderBrush = Brushes.Gray;
            }
        }

        public void Save(object sender, RoutedEventArgs e)
        {
            Result = Name.Text;
            DialogResult = true;
        }

        public void Cancel(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
