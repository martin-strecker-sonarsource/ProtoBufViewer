using ProtoBuf.Logic;
using System.Windows;

namespace ProtoBufViewer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            (this.DataContext as MainWindowViewModel)!.SelectedMessage = e.NewValue as MessageViewModel;
        }
    }
}