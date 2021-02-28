using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.VisualTree;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions
{
    public class ContentPresenterExtension 
    {
        public static readonly AttachedProperty<CornerRadius> CornerRadiusProperty = AvaloniaProperty.RegisterAttached<ContentPresenter, Control, CornerRadius>("CornerRadius");

        public static CornerRadius GetCornerRadius(Control element)
        {
            return element.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(Control element, CornerRadius value)
        {
            var t = element.GetVisualChildren();
            element.AttachedToVisualTree += Test;
            element.SetValue(CornerRadiusProperty, value);
        }

        public static void Test(object sender, VisualTreeAttachmentEventArgs attachmentEventArgs)
        {
            var control = sender as Control;
            var t = control.GetVisualChildren();

            var k = sender as UserControl;
            var l = sender as ContentPresenter;
            var j = k.GetVisualChildren();

            var contentPresenter = k.FindWithExeption<ContentPresenter>("PART_ContentPresenter");
        }
    }
}

