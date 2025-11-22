using System;

namespace DiscordBotLib.Core.Utilities
{
    internal class ColorHelper
    {
        public static int RgbToInt(int red, int green, int blue)
        {
            return (red << 16) | (green << 8) | blue;
        }

        public static (int red, int green, int blue) IntToRgb(int colorValue)
        {
            int red = (colorValue >> 16) & 0xFF;
            int green = (colorValue >> 8) & 0xFF;
            int blue = colorValue & 0xFF;
            return (red, green, blue);
        }

        public static int HexToInt(string hexcolor)
        {
            if (hexcolor.StartsWith("#"))
                hexcolor = hexcolor.Substring(1);

            if (hexcolor.Length == 6)
            {
                return Convert.ToInt32(hexcolor, 16);
            }
            else if (hexcolor.Length == 3)
            {
                string fullhex = "";
                foreach (char c in hexcolor)
                {
                    fullhex += c;
                    fullhex += c;
                }
                return Convert.ToInt32(fullhex, 16);
            }

            throw new ArgumentException("Invalid hex color format");
        }

        public static string IntToHex(int colorvalue)
        {
            return $"#{colorvalue:X6}";
        }

        public static int RandomColor()
        {
            Random random = new Random();
            return random.Next(0, 0xFFFFFF);
        }

        public static int DiscordDefaultColor => 0x000000;
        public static int DiscordSuccessColor => 0x00FF00;
        public static int DiscordWarningColor => 0xFFFF00;
        public static int DiscordErrorColor => 0xFF0000;
        public static int DiscordInfoColor => 0x0099FF;
    }
}