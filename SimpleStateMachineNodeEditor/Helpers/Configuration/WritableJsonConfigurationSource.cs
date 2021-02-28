using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace SimpleStateMachineNodeEditor.Helpers.Configuration
{
    public class WritableJsonConfigurationSource : JsonConfigurationSource
    {
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return (IConfigurationProvider)new WritableJsonConfigurationProvider(this);
        }
    }
}