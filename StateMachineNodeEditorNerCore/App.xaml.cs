using System.Reflection;
using System.Windows;
using ReactiveUI;
using Splat;
using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.Helpers.Converters;

namespace StateMachineNodeEditorNerCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator.CurrentMutable.RegisterConstant(new ConverterBoolAndVisibility(), typeof(IBindingTypeConverter));
        }

    }
}
