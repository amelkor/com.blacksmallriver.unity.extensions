namespace Bsr.Unity.Extensions
{
    /// <summary>
    /// Provides RichText strings.
    /// </summary>
    public static class RichText
    {
        /// <summary>
        /// RichText formatted string.
        /// </summary>
        public static string Colored(string value, string color = Colors.CodeBlue)
        {
            return $"<color={color}>{value}</color>";
        }
        
        /// <summary>
        /// RichText formatted string.
        /// </summary>
        public static string Parameter(string name, string value, string nameColor = Colors.CodeBlue, string valueColor = Colors.CodeWhite)
        {
            return $"<color={nameColor}>{name}</color>: <color={valueColor}>{value}</color>";
        }
        
        /// <summary>
        /// RichText formatted string. Colors value in green if true, otherwise in red.
        /// </summary>
        public static string Parameter(string name, bool value, string nameColor = Colors.CodeBlue)
        {
            return $"<color={nameColor}>{name}</color>: <color={(value ? Colors.CodeGreen : Colors.CodeRed)}>{value.ToString()}</color>";
        }
    }
}