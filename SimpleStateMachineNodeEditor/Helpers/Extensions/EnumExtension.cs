using System;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class EnumExtension
    {
        public static string Name(this Enum enumType)
        {
            return Enum.GetName(enumType.GetType(), enumType);
        }


    }
}
