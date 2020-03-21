using System;
using System.Windows;
using ReactiveUI;

namespace SimpleStateMachineNodeEditor.Helpers.Converters
{
    public class ConverterBoolAndVisibility : IBindingTypeConverter
    {
        public int GetAffinityForObjects(Type fromType, Type toType)
        {
            bool fromTypeIsCorrect = (fromType == typeof(bool?)) || (fromType == typeof(Visibility));
            bool toTypeIsCorrect = (toType == typeof(Visibility)) || (toType == typeof(bool?));
            if (!fromTypeIsCorrect || !toTypeIsCorrect)
                return 0;

            return 100;
        }

        public bool TryConvert(object from, Type toType, object conversionHint, out object result)
        {
            result = null;
            if (toType == typeof(Visibility))
            {
                bool? value = (bool?)from;
                result = Visibility.Collapsed;

                if (value != null)
                    result = (value.Value) ? Visibility.Visible : Visibility.Hidden;

                return true;
            }
            else if (toType == typeof(bool?))
            {

                Visibility value = (Visibility)from;
                result = null;

                if (value != Visibility.Collapsed)
                    result = (value == Visibility.Visible);

                return true;
            }

            return false;
        }
    }
}
