using System.Windows;

namespace Project.Infrastructure.Interfaces
{
    interface IDialog
    {
        void Save(object sender, RoutedEventArgs e);
        void Cancel(object sender, RoutedEventArgs e);
    }
}
