using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor
{
    public class AppSettings
    {
        public class AppearanceSettings
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public Themes Theme { get; set; }
        }

        public AppearanceSettings Appearance { get; set; }
    }
}