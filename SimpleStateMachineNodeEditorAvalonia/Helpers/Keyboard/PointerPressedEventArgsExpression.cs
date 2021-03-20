using Avalonia.Input;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class KeyModifiersExpression
    {
        public static bool HasShift(this PointerPressedEventArgs e)
        {
            return (e.KeyModifiers & KeyModifiers.Shift) != 0;
        }
        
        public static bool HasControl(this PointerPressedEventArgs e)
        {
            return (e.KeyModifiers & KeyModifiers.Control) != 0;
        }
    }
}