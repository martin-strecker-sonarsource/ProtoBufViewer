using ProtoBuf.Logic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ProtoBufViewer.WPF;

public class ProtBufTypeTemplateSelector : DataTemplateSelector
{
    public DataTemplate TypedFieldTemplate { get; set; }
    public DataTemplate TypedMessageTemplate { get; set; }
    public DataTemplate ProtoTypeTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (container is FrameworkElement element)
        {
            var x = item switch
            {
                TypedField => element.FindResource("TypeFieldTemplate") as DataTemplate,
                TypedMessage => element.FindResource("TypedMessageTemplate") as DataTemplate,
                _ => element.FindResource("ProtoTypeTemplate") as DataTemplate
            };
            //Debug.WriteLine(x.GetType().Name);
            return x;
        }
        return null;
    }
}