using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class ObjectExtension
    {
        public static T Cast<T>(this object obj)
        {
            return (obj is T) ? (T)obj : default(T);
        }

        public static bool TryCast<T>(this object obj, out T newValue)
        {          
            bool typeCorrect = obj is T;

            newValue = typeCorrect?(T)obj:default(T);

            return typeCorrect;
        }
    }
}
