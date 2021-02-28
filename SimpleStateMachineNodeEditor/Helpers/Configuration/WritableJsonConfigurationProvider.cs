using System;
using System.IO;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SimpleStateMachineNodeEditor.Helpers.Configuration
{
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
        {
        }

        private void Save(dynamic jsonObj)
        {
            var fileFullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }

        private void SetValue(string key, string value, dynamic jsonObj)
        {
            base.Set(key, value);
            var split = key.Split(":");
            var context = jsonObj;
            for (int i = 0; i < split.Length; i++)
            {
                var currentKey = split[i];
                if (i < split.Length - 1)
                {
                    var child = jsonObj[currentKey];
                    if (child == null)
                    {
                        context[currentKey] = new JObject();
                    }
                    context = context[currentKey];
                }
                else
                {
                    context[currentKey] = value;
                }
            }
        }

        private dynamic GetJsonObj()
        {
            var fileFullPath = Source.FileProvider.GetFileInfo(Source.Path).PhysicalPath;
            var json = File.Exists(fileFullPath) ? File.ReadAllText(fileFullPath) : "{}";
            return JsonConvert.DeserializeObject(json);
        }

        public override void Set(string key, string value)
        {
            var jsonObj = GetJsonObj();
            SetValue(key, value, jsonObj);
            Save(jsonObj);
        }

        public void Set(string key, object value)
        {
            var jsonObj = GetJsonObj();
            var serialized = JsonConvert.SerializeObject(value);
            var jToken = JsonConvert.DeserializeObject(serialized) as JToken ?? new JValue(value);
            WalkAndSet(key, jToken, jsonObj);
            Save(jsonObj);
        }

        private void WalkAndSet(string key, JToken value, dynamic jsonObj)
        {
            switch (value)
            {
                case JArray jArray:
                {
                    //TODO Realize arrays
                    break;
                }
                case JObject jObject:
                {
                    foreach (var propertyInfo in jObject.Properties())
                    {
                        var propName = propertyInfo.Name;
                        var currentKey = key == null ? propName : $"{key}:{propName}";
                        var propValue = propertyInfo.Value;
                        WalkAndSet(currentKey, propValue, jsonObj);
                    }
                    break;
                }
                case JValue jValue:
                {
                    SetValue(key, jValue.ToString(), jsonObj);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(value));
            }
        }
    }
}