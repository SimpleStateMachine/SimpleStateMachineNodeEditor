using System.Reflection;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using Splat;
using SimpleStateMachineNodeEditor.Helpers.Converters;
using WritableJsonConfiguration;
using Application = System.Windows.Application;

namespace SimpleStateMachineNodeEditor
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

            IConfigurationRoot configuration;
            configuration = WritableJsonConfigurationFabric.Create("Settings.json");
            Locator.CurrentMutable.RegisterConstant(configuration, typeof(IConfiguration));
        }
    }
}
