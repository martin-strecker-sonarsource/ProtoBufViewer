using ProtoBuf.Logic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

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
            DataContext = new MainWindowViewModel();
            ViewModel.SelectTypedMessage = SelectTypedMessage;
        }

        public MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext;

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) =>
            ViewModel.SelectedMessage = e.NewValue as MessageViewModel;

        private void SelectTypedMessage(IEnumerable<ProtoType> messages)
        {
            var itemContainer = tv_TypedMessages.ItemContainerGenerator;
            var enumerator = messages.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (itemContainer == null)
                {
                    return;
                }

                var message = enumerator.Current;
                if (itemContainer.Status is GeneratorStatus.ContainersGenerated)
                {
                    itemContainer = ExpandMessage(itemContainer, message);
                }
                else
                {
                    Dispatcher.Invoke(delegate
                    {
                        itemContainer = ExpandMessage(itemContainer, message);
                    }, DispatcherPriority.Input);
                }
            }

            static ItemContainerGenerator ExpandMessage(ItemContainerGenerator itemContainer, ProtoType message)
            {
                var tvi = itemContainer.ContainerFromItem(message) as TreeViewItem;
                if (tvi == null && itemContainer.Items.OfType<TypedField>().FirstOrDefault(x => x.Value == message) is { } typedField)
                {
                    tvi = itemContainer.ContainerFromItem(typedField) as TreeViewItem;
                }
                if (tvi != null)
                {
                    (tvi.IsExpanded, tvi.IsSelected) = (true, true);
                    tvi.BringIntoView();
                    itemContainer = tvi.ItemContainerGenerator;
                }

                return itemContainer;
            }
        }
    }
}