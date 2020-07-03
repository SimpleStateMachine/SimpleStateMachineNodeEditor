using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace SimpleStateMachineNodeEditor.Helpers.Configuration
{
    public static class WritableJsonConfigurationProviderExtensions
    {
        public static void Set(this IConfiguration configuration, object value)
        {
            switch (configuration)
            {
                case IConfigurationRoot configurationRoot:
                {
                    var provider = configurationRoot.Providers.First(p => p is WritableJsonConfigurationProvider) as WritableJsonConfigurationProvider;
                    provider.Set(null, value);
                    break;
                }
                case ConfigurationSection configurationSection:
                {
                    var rootProp = typeof(ConfigurationSection).GetField("_root", BindingFlags.NonPublic | BindingFlags.Instance); ;
                    var root = rootProp.GetValue(configurationSection) as IConfigurationRoot;
                    var provider = root.Providers.First(p => p is WritableJsonConfigurationProvider) as WritableJsonConfigurationProvider;
                    provider.Set(configurationSection.Path, value);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(configuration));
            }
        }
    }
}