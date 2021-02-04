namespace SimpleStateMachineNodeEditorAvalonia.Helpers.Extensions
{
    public static class StringWithEnableExtensions
    {
        public static bool IsNullOrEmpty(this StringWithEnable str)
        {
            return string.IsNullOrEmpty(str.Value);
        }
        public static bool IsNotNullAndEmpty(this StringWithEnable str)
        {
            return !str.IsNullOrEmpty();
        }
    }
}