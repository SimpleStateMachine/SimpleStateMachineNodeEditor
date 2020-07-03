using System.Reflection;
using Microsoft.Extensions.Configuration;
using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers.Configuration;
using Splat;
using SimpleStateMachineNodeEditor.Helpers.Converters;
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

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfigurationRoot configuration = configurationBuilder.Add<WritableJsonConfigurationSource>(s =>
                {
                    s.FileProvider = null;
                    s.Path = "Settings.json";
                    s.Optional = true;
                    s.ReloadOnChange = true;
                    s.ResolveFileProvider();
                }).Build();

            Locator.CurrentMutable.RegisterConstant(configuration, typeof(IConfiguration));
        }
    }
}
