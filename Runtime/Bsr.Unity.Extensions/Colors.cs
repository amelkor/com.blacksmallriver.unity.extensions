using UnityEngine;

namespace Bsr.Unity.Extensions
{
    public static class Colors
    {
        /// <summary>
        /// rgb(220, 220, 220)
        /// </summary>
        public const string CodeWhite = "DCDCDC";

        /// <summary>
        /// rgb(169, 169, 169)
        /// </summary>
        public const string CodeGray = "#A9A9A9";

        /// <summary>
        /// rgb(86, 156, 214)
        /// </summary>
        public const string CodeBlue = "#569CD6";

        /// <summary>
        /// rgb(255, 128, 0)
        /// </summary>
        public const string CodeOrange = "#FF8000";

        /// <summary>
        /// rgb(255, 215, 0)
        /// </summary>
        public const string CodeYellow = "#FFD700";

        /// <summary>
        /// rgb(58, 214, 186)
        /// </summary>
        public const string CodeAzure = "#3AD6BA";

        /// <summary>
        /// rgb(0, 255, 255)
        /// </summary>
        public const string CodeAzureBright = "#00FFFF";

        /// <summary>
        /// rgb(189, 183, 107)
        /// </summary>
        public const string CodeSand = "#BDB76B";

        /// <summary>
        /// rgb(189, 99, 197)
        /// </summary>
        public const string CodePink = "#BD63C5";

        /// <summary>
        /// rgb(87, 166, 74)
        /// </summary>
        public const string CodeGreen = "#57A64A";

        /// <summary>
        /// rgb(214, 157, 133)
        /// </summary>
        public const string CodeSkin = "#D69D85";

        /// <summary>
        /// rgb(255, 0, 0)
        /// </summary>
        public const string CodeRed = "#FF0000";

        public static Color Transparent => new Color(0, 0, 0, 0);

        public static Color White => new Color(220, 220, 220);
        public static Color Gray => new Color(169, 169, 169);
        public static Color Blue => new Color(86, 156, 214);
        public static Color Orange => new Color(255, 128, 0);
        public static Color Yellow => new Color(255, 215, 0);
        public static Color Azure => new Color(58, 214, 186);
        public static Color AzureBright => new Color(0, 255, 255);
        public static Color Sand => new Color(189, 183, 107);
        public static Color Pink => new Color(189, 99, 197);
        public static Color Green => new Color(87, 166, 74);
        public static Color Skin => new Color(214, 157, 133);
        public static Color Red => new Color(255, 0, 0);

        public static Color WithOpacity(this Color color, float opacity)
        {
            opacity = Mathf.Clamp01(opacity);
            color.a = opacity;
            return color;
        }
    }
}