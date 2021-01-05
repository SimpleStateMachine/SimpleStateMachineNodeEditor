using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class Keyboard
    {
        public static void Init()
        {

        }
        static Keyboard()
        {        
            InputElement.KeyDownEvent.AddClassHandler<InputElement>((x, e) => Keyboard.OnKeyDown(e));
            InputElement.KeyUpEvent.AddClassHandler<InputElement>((x, e) => Keyboard.OnKeyUp(e));
        }
        public static readonly HashSet<Key> Keys = new HashSet<Key>();
        public static bool IsKeyDown(Key key) => Keys.Contains(key);

        public static bool IsKeyDownOneOf(params Key[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (Keys.Contains(keys[i]))
                {
                    return true;
                }
            }
            return false;
        }
         
        public static  Dictionary<Key, string> Keyss = new Dictionary<Key, string>();

        public static void OnKeyDown(KeyEventArgs e)
        {
            Keys.Add(e.Key);
        }
        public static void OnKeyUp(KeyEventArgs e)
        {
            Keys.Remove(e.Key);
        }
    }
}
