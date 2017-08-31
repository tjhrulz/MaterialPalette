using System;
using System.Runtime.InteropServices;
using Rainmeter;
using System.Collections.Generic;
using System.Drawing;

namespace MaterialPalette
{
    class Swatch
    {
        //For creating colors from HSB
        public static Color FromHSB(float hue, float saturation, float brightness)
        {

            //Cap colors since this is not intended to be color safe
            if (0f > hue)
            {
                hue = 0f;
            }
            else if (360f < hue)
            {
                hue = 360f;
            }

            if (0f > saturation)
            {
                saturation = 0f;
            }
            else if (1f < saturation)
            {
                saturation = 1f;
            }

            if (0f > brightness)
            {
                brightness = 0f;
            }
            else if (1f < brightness)
            {
                brightness = 1f;
            }

            if (0 == saturation)
            {
                return Color.FromArgb(
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255),
                                    Convert.ToInt32(brightness * 255));
            }

            float fMax, fMid, fMin;
            int iSextant, iMax, iMid, iMin;

            if (0.5 < brightness)
            {
                fMax = brightness - (brightness * saturation) + saturation;
                fMin = brightness + (brightness * saturation) - saturation;
            }
            else
            {
                fMax = brightness + (brightness * saturation);
                fMin = brightness - (brightness * saturation);
            }

            iSextant = (int)Math.Floor(hue / 60f);
            if (300f <= hue)
            {
                hue -= 360f;
            }

            hue /= 60f;
            hue -= 2f * (float)Math.Floor(((iSextant + 1f) % 6f) / 2f);
            if (0 == iSextant % 2)
            {
                fMid = (hue * (fMax - fMin)) + fMin;
            }
            else
            {
                fMid = fMin - (hue * (fMax - fMin));
            }

            iMax = Convert.ToInt32(fMax * 255);
            iMid = Convert.ToInt32(fMid * 255);
            iMin = Convert.ToInt32(fMin * 255);

            switch (iSextant)
            {
                case 1:
                    return Color.FromArgb(iMid, iMax, iMin);
                case 2:
                    return Color.FromArgb(iMin, iMax, iMid);
                case 3:
                    return Color.FromArgb(iMin, iMid, iMax);
                case 4:
                    return Color.FromArgb(iMid, iMin, iMax);
                case 5:
                    return Color.FromArgb(iMax, iMin, iMid);
                default:
                    return Color.FromArgb(iMax, iMid, iMin);
            }
        }

        //Where addedColorr is your starting point at 0.0 amountOfBase and baseColor is the color a 1.0 amountOfBase
        public static Color Blend(Color baseColor, Color addedColor, double amountOfBase)
        {
            byte r = (byte)((baseColor.R * amountOfBase) + addedColor.R * (1 - amountOfBase));
            byte g = (byte)((baseColor.G * amountOfBase) + addedColor.G * (1 - amountOfBase));
            byte b = (byte)((baseColor.B * amountOfBase) + addedColor.B * (1 - amountOfBase));
            return Color.FromArgb(r, g, b);
        }
        public static Color MakeDarkBase(Color primaryColor, float minContrast)
        {
            //0x2FF6F00
            Color darkColor = Color.FromArgb(
                primaryColor.R * primaryColor.R / 255,
                primaryColor.G * primaryColor.G / 255, 
                primaryColor.B * primaryColor.B / 255);

            float pHue = primaryColor.GetHue();
            float pSat = primaryColor.GetSaturation();
            float pBright = primaryColor.GetBrightness();

            float dHue = darkColor.GetHue();
            float dSat = darkColor.GetSaturation();
            float dBright = darkColor.GetBrightness();

            float diff = 
                (primaryColor.GetHue() - darkColor.GetHue()) /360 +
                (primaryColor.GetSaturation() - darkColor.GetSaturation()) +
                (primaryColor.GetBrightness() - darkColor.GetBrightness());

            if(diff < minContrast * 3)
            {
                //@TODO maybe split between bright and saturation if that fits in both?

                //If whole min contrast can come from brightness do that 
                if(dBright - minContrast > 0 && minContrast > 0.001)
                {
                    return FromHSB(dHue, dSat, dBright - minContrast);
                }
                else if(minContrast > 0.001)
                {
                    minContrast -= dBright;
                    dBright = 0;
                }

                if (dSat - minContrast > 0 && minContrast > 0.001)
                {
                    return FromHSB(dHue, dSat - minContrast, dBright);
                }

                else if (minContrast > 0.001)
                {
                    minContrast -= dSat;
                    dSat = 0;
                }

                if(dHue > 180.0)
                {
                    dHue += Math.Max(minContrast * 360, 180);
                }
                else if (minContrast > 0.001)
                {
                    dHue -= Math.Max(minContrast * 360, 180);
                }

                //Should never be hit unless minContrast is way too big or none
                return FromHSB(dHue, dSat - minContrast, dBright);
            }

            return darkColor;
        }

        public enum SwatchNames
        {
            Red,
            Pink,
            Purple,
            DeepPurple,
            Indigo,
            Blue,
            LightBlue,
            Cyan,
            Teal,
            Green,
            LightGreen,
            Lime,
            Yellow,
            Amber,
            Orange,
            DeepOrange,
            //These dont have alt colors and thus have null values
            Brown,
            Grey,
            BlueGrey
        }

        public enum SwatchPrimaryColors
        {
            P500,
            P50,
            P100,
            P200,
            P300,
            P400,
            P600,
            P700,
            P800,
            P900
        }

        public enum SwatchAltColors
        {
            A100,
            A200,
            A400,
            A700
        }

        public enum SwatchVariantColors
        {
            VDARK,
            VLIGHT
        }

        public class ColorSwatch
        {
            public readonly Color[] SwatchPrimaryColors;
            public readonly Color[] SwatchAltColors;
            public readonly Color[] SwatchVariantColors;

            //Fallback constructor for fails, makes every color black
            public ColorSwatch()
            {
                //I may make this in the future use the enum values to declare length and position
                SwatchPrimaryColors = new Color[] { Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0),
                    Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0) };
                SwatchAltColors = new Color[] { Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0), Color.FromArgb(0) };

                Color[] variantColors = { Color.FromArgb(0), Color.FromArgb(0) };

                SwatchVariantColors = variantColors;
            }

            //Constructor including alt colors, only use for known good setups such as google's swatches
            public ColorSwatch(SwatchNames name, int hexP500, int hexP50, int hexP100, int hexP200, int hexP300, int hexP400,
                   int hexP600, int hexP700, int hexP800, int hexP900, int hexA100, int hexA200, int hexA400, int hexA700)
            {
                //I may make this in the future use the enum values to declare length and position
                SwatchPrimaryColors = new Color[] { Color.FromArgb(hexP500), Color.FromArgb(hexP50), Color.FromArgb(hexP100), Color.FromArgb(hexP200), 
                    Color.FromArgb(hexP300), Color.FromArgb(hexP400), Color.FromArgb(hexP600), Color.FromArgb(hexP700), Color.FromArgb(hexP800), Color.FromArgb(hexP900) };
                SwatchAltColors = new Color[] { Color.FromArgb(hexA100), Color.FromArgb(hexA200), Color.FromArgb(hexA400), Color.FromArgb(hexA700) };

                Color[] variantColors = { Color.FromArgb(hexP500), Color.FromArgb(hexP500) };

                variantColors[(int)Swatch.SwatchVariantColors.VDARK] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetBrightness() - 0.2f);

                variantColors[(int)Swatch.SwatchVariantColors.VLIGHT] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetBrightness() + 0.2f);

                SwatchVariantColors = variantColors;
            }

            //Constructor for swatches with no alt colors, only use for known good setups such as google's swatches
            public ColorSwatch(SwatchNames name, int hexP500, int hexP50, int hexP100, int hexP200, int hexP300, int hexP400,
                   int hexP600, int hexP700, int hexP800, int hexP900)
            {
                SwatchPrimaryColors = new Color[] { Color.FromArgb(hexP500), Color.FromArgb(hexP50), Color.FromArgb(hexP100), Color.FromArgb(hexP200),
                    Color.FromArgb(hexP300), Color.FromArgb(hexP400), Color.FromArgb(hexP600), Color.FromArgb(hexP700), Color.FromArgb(hexP800), Color.FromArgb(hexP900) };
                //@TODO implement this to calculate actual alt colors
                SwatchAltColors = new Color[] { Color.FromArgb(hexP500), Color.FromArgb(hexP500), Color.FromArgb(hexP500), Color.FromArgb(hexP500) };

                Color[] variantColors = { Color.FromArgb(hexP500), Color.FromArgb(hexP500) };

                variantColors[(int)Swatch.SwatchVariantColors.VDARK] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetBrightness() - 0.2f);

                variantColors[(int)Swatch.SwatchVariantColors.VLIGHT] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetBrightness() + 0.2f);

                SwatchVariantColors = variantColors;
            }
            
            //Full swatch generation from just color, does not guarantee that hexp500 is the color you give unless matchPrimaryColor is true
            public ColorSwatch(int hex, bool matchPrimaryColor)
            {
                Color P500 = Color.FromArgb(hex);
                Color lightBase = Color.FromArgb(255, 255, 255);
                Color darkBase = MakeDarkBase(P500, 0.25f);

                //var baseDark = $scope.multiply(tinycolor(hex).toRgb(), tinycolor(hex).toRgb());
                //var baseTriad = tinycolor(hex).tetrad();

                SwatchPrimaryColors = new Color[] {
                    P500,                       //500
                    Blend(P500, lightBase, .12), //P50
                    Blend(P500, lightBase, .30), //P100
                    Blend(P500, lightBase, .50), //P200
                    Blend(P500, lightBase, .70), //P300
                    Blend(P500, lightBase, .85), //P400
                    Blend(P500, darkBase, .87),  //P600
                    Blend(P500, darkBase, .70),  //P700
                    Blend(P500, darkBase, .54),  //P800
                    Blend(P500, darkBase, .25),  //P900
                };

                SwatchAltColors = new Color[] {
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(65), 'A100'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(55), 'A200'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(45), 'A400'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(40), 'A700')

                    P500, //A100
                    P500, //A200
                    P500, //A400
                    P500, //A700
                };

                Color[] variantColors = { P500, P500 };

                variantColors[(int)Swatch.SwatchVariantColors.VDARK] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetBrightness() - 0.2f);

                variantColors[(int)Swatch.SwatchVariantColors.VLIGHT] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetBrightness() + 0.2f);

                SwatchVariantColors = variantColors;
            }

            //Full swatch generation from just color, does not guarantee that hexp500 is the color you give unless matchPrimaryColor is true
            public ColorSwatch(byte R, byte G, byte B, bool matchPrimaryColor)
            {
                Color P500 = Color.FromArgb(R, G, B);
                Color lightBase = Color.FromArgb(255, 255, 255);
                Color darkBase = MakeDarkBase(P500, 0.25f);
                //var baseDark = $scope.multiply(tinycolor(hex).toRgb(), tinycolor(hex).toRgb());
                //var baseTriad = tinycolor(hex).tetrad();

                SwatchPrimaryColors = new Color[] {
                    P500,                       //500
                    Blend(P500, lightBase, .12), //P50
                    Blend(P500, lightBase, .30), //P100
                    Blend(P500, lightBase, .50), //P200
                    Blend(P500, lightBase, .70), //P300
                    Blend(P500, lightBase, .85), //P400
                    Blend(P500, darkBase, .87),  //P600
                    Blend(P500, darkBase, .70),  //P700
                    Blend(P500, darkBase, .54),  //P800
                    Blend(P500, darkBase, .25),  //P900
                };

                SwatchAltColors = new Color[] {
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(65), 'A100'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(55), 'A200'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(45), 'A400'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(40), 'A700')

                    P500, //A100
                    P500, //A200
                    P500, //A400
                    P500, //A700
                };

                Color[] variantColors = { P500, P500 };

                variantColors[(int)Swatch.SwatchVariantColors.VDARK] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetBrightness() - 0.2f);

                variantColors[(int)Swatch.SwatchVariantColors.VLIGHT] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetBrightness() + 0.2f);

                SwatchVariantColors = variantColors;
            }

            //Full swatch generation from just color, does not guarantee that hexp500 is the color you give unless matchPrimaryColor is true
            public ColorSwatch(Color P500, bool matchPrimaryColor)
            {
                Color lightBase = Color.FromArgb(255, 255, 255);
                Color darkBase = MakeDarkBase(P500, 0.25f);
                //var baseDark = $scope.multiply(tinycolor(hex).toRgb(), tinycolor(hex).toRgb());
                //var baseTriad = tinycolor(hex).tetrad();

                SwatchPrimaryColors = new Color[] {
                    P500,                       //500
                    Blend(P500, lightBase, .12), //P50
                    Blend(P500, lightBase, .30), //P100
                    Blend(P500, lightBase, .50), //P200
                    Blend(P500, lightBase, .70), //P300
                    Blend(P500, lightBase, .85), //P400
                    Blend(P500, darkBase, .87),  //P600
                    Blend(P500, darkBase, .70),  //P700
                    Blend(P500, darkBase, .54),  //P800
                    Blend(P500, darkBase, .25),  //P900
                };

                SwatchAltColors = new Color[] {
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(65), 'A100'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(80).lighten(55), 'A200'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(45), 'A400'),
                //getColorObject(tinycolor.mix(baseDark, baseTriad[4], 15).saturate(100).lighten(40), 'A700')

                    P500, //A100
                    P500, //A200
                    P500, //A400
                    P500, //A700
                };

                Color[] variantColors = { P500, P500 };

                variantColors[(int)Swatch.SwatchVariantColors.VDARK] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VDARK].GetBrightness() - 0.2f);

                variantColors[(int)Swatch.SwatchVariantColors.VLIGHT] = FromHSB(
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetHue(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetSaturation(),
                    variantColors[(int)Swatch.SwatchVariantColors.VLIGHT].GetBrightness() + 0.2f);

                SwatchVariantColors = variantColors;
            }

        }

        public static readonly ColorSwatch[] ColorSwatches;

        static Swatch()
        {
            //This is calculating size wrong
            ColorSwatch[] colorSwatches = new ColorSwatch[Enum.GetNames(typeof(SwatchNames)).Length];

            //Sorry for this, it will likely be better in the future
            colorSwatches[(int)SwatchNames.Red] = new ColorSwatch(SwatchNames.Red, 0xF44336,
                0xFFEBEE, 0xFFCDD2, 0xEF9A9A, 0xE57373, 0xEF5350, 0xE53935, 0xD32F2F, 0xC62828, 0xB71C1C,
                0xFF8A80, 0xFF5252, 0xFF1744, 0xD50000);

            colorSwatches[(int)SwatchNames.Pink] = new ColorSwatch(SwatchNames.Pink, 0xE91E63,
                0xFCE4EC, 0xF8BBD0, 0xF48FB1, 0xF06292, 0xEC407A, 0xD81B60, 0xC2185B, 0xAD1457, 0x880E4F,
                0xFF80AB, 0xFF4081, 0xF50057, 0xC51162);

            colorSwatches[(int)SwatchNames.Purple] = new ColorSwatch(SwatchNames.Purple, 0x9C27B0,
                0xF3E5F5, 0xE1BEE7, 0xCE93D8, 0xBA68C8, 0xAB47BC, 0x8E24AA, 0x7B1FA2, 0x6A1B9A, 0x4A148C,
                0xEA80FC, 0xE040FB, 0xD500F9, 0xAA00FF);

            colorSwatches[(int)SwatchNames.DeepPurple] = new ColorSwatch(SwatchNames.DeepPurple, 0x673AB7,
                0xEDE7F6, 0xD1C4E9, 0xB39DDB, 0x9575CD, 0x7E57C2, 0x5E35B1, 0x512DA8, 0x4527A0, 0x311B92,
                0xB388FF, 0x7C4DFF, 0x651FFF, 0x6200EA);

            colorSwatches[(int)SwatchNames.Indigo] = new ColorSwatch(SwatchNames.Indigo, 0x3F51B5,
                0xE8EAF6, 0xC5CAE9, 0x9FA8DA, 0x7986CB, 0x5C6BC0, 0x3949AB, 0x303F9F, 0x283593, 0x1A237E,
                0x8C9EFF, 0x536DFE, 0x3D5AFE, 0x304FFE0);

            colorSwatches[(int)SwatchNames.Blue] = new ColorSwatch(SwatchNames.Blue, 0x2196F3,
                0xE3F2FD, 0xBBDEFB, 0x90CAF9, 0x64B5F6, 0x42A5F5, 0x1E88E5, 0x1976D2, 0x1565C0, 0x0D47A1,
                0x82B1FF, 0x448AFF, 0x2979FF, 0x2962FF);

            colorSwatches[(int)SwatchNames.LightBlue] = new ColorSwatch(SwatchNames.LightBlue, 0x03A9F4,
                0xE1F5FE, 0xB3E5FC, 0x81D4FA, 0x4FC3F7, 0x29B6F6, 0x039BE5, 0x0288D1, 0x0277BD, 0x01579B,
                0x80D8FF, 0x40C4FF, 0x00B0FF, 0x0091EA);

            colorSwatches[(int)SwatchNames.Cyan] = new ColorSwatch(SwatchNames.Cyan, 0x00BCD4,
                0xE0F7FA, 0xB2EBF2, 0x80DEEA, 0x4DD0E1, 0x26C6DA, 0x00ACC1, 0x0097A7, 0x00838F, 0x006064,
                0x84FFFF, 0x18FFFF, 0x00E5FF, 0x00B8D4);

            colorSwatches[(int)SwatchNames.Teal] = new ColorSwatch(SwatchNames.Teal, 0x009688,
                0xE0F2F1, 0xB2DFDB, 0x80CBC4, 0x4DB6AC, 0x26A69A, 0x00897B, 0x00796B, 0x00695C, 0x004D40,
                0xA7FFEB, 0x64FFDA, 0x1DE9B6, 0x00BFA5);

            colorSwatches[(int)SwatchNames.Green] = new ColorSwatch(SwatchNames.Green, 0x4CAF50,
                0xE8F5E9, 0xC8E6C9, 0xA5D6A7, 0x81C784, 0x66BB6A, 0x43A047, 0x388E3C, 0x2E7D32, 0x1B5E20,
                0xB9F6CA, 0x69F0AE, 0x00E676, 0x00C853);

            colorSwatches[(int)SwatchNames.LightGreen] = new ColorSwatch(SwatchNames.LightGreen, 0x8BC34A,
                0xF1F8E9, 0xDCEDC8, 0xC5E1A5, 0xAED581, 0x9CCC65, 0x7CB342, 0x689F38, 0x558B2F, 0x33691E,
                0xCCFF90, 0xB2FF59, 0x76FF03, 0x64DD17);

            colorSwatches[(int)SwatchNames.Lime] = new ColorSwatch(SwatchNames.Lime, 0xCDDC39,
                0xF9FBE7, 0xF0F4C3, 0xE6EE9C, 0xDCE775, 0xD4E157, 0xC0CA33, 0xAFB42B, 0x9E9D24, 0x827717,
                0xF4FF81, 0xEEFF41, 0xC6FF00, 0xAEEA00);

            colorSwatches[(int)SwatchNames.Yellow] = new ColorSwatch(SwatchNames.Yellow, 0xFFEB3B,
                0xFFFDE7, 0xFFF9C4, 0xFFF59D, 0xFFF176, 0xFFEE58, 0xFDD835, 0xFBC02D, 0xF9A825, 0xF57F17,
                0xFFFF8D, 0xFFFF00, 0xFFEA00, 0xFFD600);

            colorSwatches[(int)SwatchNames.Amber] = new ColorSwatch(SwatchNames.Amber, 0xFFC107,
                0xFFF8E1, 0xFFECB3, 0xFFE082, 0xFFD54F, 0xFFCA28, 0xFFB300, 0xFFA000, 0xFF8F00, 0xFF6F00,
                0xFFE57F, 0xFFD740, 0xFFC400, 0xFFAB00);

            colorSwatches[(int)SwatchNames.Orange] = new ColorSwatch(SwatchNames.Orange, 0xFF9800,
                0xFFF3E0, 0xFFE0B2, 0xFFCC80, 0xFFB74D, 0xFFA726, 0xFB8C00, 0xF57C00, 0xEF6C00, 0xE65100,
                0xFFD180, 0xFFAB40, 0xFF9100, 0xFF6D00);

            colorSwatches[(int)SwatchNames.DeepOrange] = new ColorSwatch(SwatchNames.DeepOrange, 0xFF5722,
                0xFBE9E7, 0xFFCCBC, 0xFFAB91, 0xFF8A65, 0xFF7043, 0xF4511E, 0xE64A19, 0xD84315, 0xBF360C,
                0xFF9E80, 0xFF6E40, 0xFF3D00, 0xDD2C00);

            colorSwatches[(int)SwatchNames.Brown] = new ColorSwatch(SwatchNames.Brown, 0x795548,
                0xEFEBE9, 0xD7CCC8, 0xBCAAA4, 0xA1887F, 0x8D6E63, 0x6D4C41, 0x5D4037, 0x4E342E, 0x3E2723);

            colorSwatches[(int)SwatchNames.Grey] = new ColorSwatch(SwatchNames.Grey, 0x9E9E9E,
                0xFAFAFA, 0xF5F5F5, 0xEEEEEE, 0xE0E0E0, 0xBDBDBD, 0x757575, 0x616161, 0x424242, 0x212121);

            colorSwatches[(int)SwatchNames.BlueGrey] = new ColorSwatch(SwatchNames.BlueGrey, 0x607D8B,
                0xECEFF1, 0xCFD8DC, 0xB0BEC5, 0x90A4AE, 0x78909C, 0x546E7A, 0x455A64, 0x37474F, 0x263238);

            ColorSwatches = colorSwatches;
        }
    }

    internal abstract class Measure
    {
        public enum MeasureTypes
        {
            Swatch,
            Palette
        }
        public enum ColorTypes
        {
            Primary,
            Alt,
            Variant
        }

        public static string colorToRainmeterString(Color color)
        {
            return color.R + "," + color.G + "," + color.B; ;
        }

        internal abstract void Dispose();
        internal abstract void Reload(Rainmeter.API api, ref double maxValue);
        internal abstract void ExecuteBang(String args);
        internal abstract double Update();
        internal abstract String GetString();
    }

    internal class ParentMeasure : Measure
    {
        internal static List<ParentMeasure> ParentMeasures = new List<ParentMeasure>();

        internal String Name;
        internal IntPtr Skin;
        internal Swatch.ColorSwatch currSwatch;
        internal MeasureTypes type;

        internal ParentMeasure()
        {
            ParentMeasures.Add(this);
        }

        internal override void Dispose()
        {
            ParentMeasures.Remove(this);
        }


        internal override void Reload(Rainmeter.API api, ref double maxValue)
        {
            Name = api.GetMeasureName();
            Skin = api.GetSkin();

            string typeString = api.ReadString("Type", MeasureTypes.Swatch.ToString());

            if (typeString.Equals(MeasureTypes.Swatch.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                type = MeasureTypes.Swatch;

                //Replce spaces and underscores with blanks so it is easier for users
                String color = api.ReadString("Color", null).Replace(" ", String.Empty).Replace("_", String.Empty);
                Swatch.SwatchNames currSwatchName;
                //If it is a color name known
                if (Enum.TryParse<Swatch.SwatchNames>(color, out currSwatchName))
                {
                    currSwatch = Swatch.ColorSwatches[(int)currSwatchName];
                }
                else
                {
                    //If hex code
                    if(color.Contains("#") || !color.Contains(","))
                    {
                        try
                        {
                            //Take just first 6 numbers from hexcode and skip #
                            currSwatch = new Swatch.ColorSwatch(Convert.ToInt32(color.Substring(1, 6), 16), false);
                        }
                        catch (Exception e)
                        {
                            API.Log(API.LogType.Error, "Error generating swatch from RGB color");
                            API.Log(API.LogType.Debug, "Exception:" + e.ToString());

                            //Fallback to all black
                            currSwatch = new Swatch.ColorSwatch();
                        }
                    }
                    //If comma seperated RGB
                    else
                    {
                        String[] colorArr = color.Split(',');
                        if(colorArr.Length >= 3)
                        {
                            //@TODO Possibly accept HSB & HSL out of the box especially with plans to implement that in the future into rainmeter

                            try
                            {
                                currSwatch = new Swatch.ColorSwatch(Convert.ToByte(colorArr[0]), Convert.ToByte(colorArr[1]), Convert.ToByte(colorArr[2]), false);
                            }
                            catch (Exception e)
                            {
                                API.Log(API.LogType.Error, "Error generating swatch from RGB color");
                                API.Log(API.LogType.Debug, "Exception:" + e.ToString());

                                //Fallback to all black
                                currSwatch = new Swatch.ColorSwatch();
                            }
                        }
                        else
                        {
                        }
                    }
                    //@TODO read color and autogen swatch
                }
            }
            else if (typeString.Equals(MeasureTypes.Palette.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                type = MeasureTypes.Palette;
                //@TODO image Palette creation
            }
            else
            {
                API.Log(API.LogType.Error, "MaterialPalette - Invalid parent type");
            }
        }

        internal override void ExecuteBang(String args)
        {

        }

        internal override double Update()
        {
            return 0.0;
        }

        internal override String GetString()
        {
            if (type == MeasureTypes.Swatch && currSwatch.SwatchPrimaryColors != null)
            {
                return colorToRainmeterString(currSwatch.SwatchPrimaryColors[(int)Swatch.SwatchPrimaryColors.P500]);
            }
            else if (type == MeasureTypes.Palette)
            {
                //@TODO image Palette creation
            }

            return null;
        }
    }

    internal class ChildMeasure : Measure
    {
        private ParentMeasure myParent = null;

        //Default color swatch info
        private ColorTypes myColorType = ColorTypes.Primary;
        private int myColorLoc = (int)Swatch.SwatchPrimaryColors.P500;

        internal ChildMeasure()
        {

        }

        internal override void Dispose()
        {

        }

        internal override void Reload(Rainmeter.API api, ref double maxValue)
        {
            String parentName = api.ReadString("Parent", "");
            IntPtr skin = api.GetSkin();

            // Find parent using name AND the skin handle to be sure that it's the right one.
            myParent = null;
            foreach (ParentMeasure parentMeasure in ParentMeasure.ParentMeasures)
            {
                if (parentMeasure.Skin.Equals(skin) && parentMeasure.Name.Equals(parentName))
                {
                    myParent = parentMeasure;
                }
            }

            if (myParent == null)
            {
                API.Log(API.LogType.Error, "ParentChild.dll: ParentName=" + parentName + " not valid");
            }
            else
            {
                if (myParent.type == MeasureTypes.Swatch)
                {
                    //Go to all uppercase and replace P with empty to make it easier for users
                    String colorName = api.ReadString("Code", null).ToUpperInvariant();

                    //If color name does not have prefix assume Primary
                    if(!colorName.StartsWith("P") && !colorName.StartsWith("A") && !colorName.StartsWith("V"))
                    {
                        colorName = "P" + colorName;
                    }

                    Swatch.SwatchPrimaryColors currPrimaryColor;
                    Swatch.SwatchAltColors currAltColor;
                    Swatch.SwatchVariantColors currVariantColor;

                    //Check if it exists in the list of primary colors or secondaryColors
                    if (Enum.TryParse<Swatch.SwatchPrimaryColors>(colorName, out currPrimaryColor))
                    {
                        myColorType = ColorTypes.Primary;
                        myColorLoc = (int)currPrimaryColor;
                    }
                    else if (Enum.TryParse<Swatch.SwatchAltColors>(colorName, out currAltColor))
                    {
                        myColorType = ColorTypes.Alt;
                        myColorLoc = (int)currAltColor;
                    }
                    else if (Enum.TryParse<Swatch.SwatchVariantColors>(colorName, out currVariantColor))
                    {
                        myColorType = ColorTypes.Variant;
                        myColorLoc = (int)currVariantColor;
                    }

                }
                else if (myParent.type == MeasureTypes.Palette)
                {
                    //@TODO image Palette creation
                }
                else
                {
                    API.Log(API.LogType.Error, "MaterialPalette - Invalid child type");
                }
            }
        }

        internal override void ExecuteBang(String args)
        {

        }

        internal override double Update()
        {
            if (myParent != null)
            {

            }
            return 0.0;
        }

        internal override String GetString()
        {
            if (myParent != null && myParent.currSwatch.SwatchPrimaryColors != null)
            {
                if (myParent.type == MeasureTypes.Swatch)
                {
                    //A little messy but I wanted two seperate arrays
                    if (myColorType == ColorTypes.Primary)
                    {
                        return colorToRainmeterString(myParent.currSwatch.SwatchPrimaryColors[myColorLoc]);
                    }
                    else if (myColorType == ColorTypes.Alt)
                    {
                        return colorToRainmeterString(myParent.currSwatch.SwatchAltColors[myColorLoc]);
                    }
                    else if (myColorType == ColorTypes.Variant)
                    {
                        return colorToRainmeterString(myParent.currSwatch.SwatchVariantColors[myColorLoc]);
                    }
                }
                else if (myParent.type == MeasureTypes.Palette)
                {
                    return "Unimplemented";
                }
            }
            return null;
        }
    }

    public static class Plugin
    {
        static IntPtr StringBuffer = IntPtr.Zero;

        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            Rainmeter.API api = new Rainmeter.API(rm);

            String parent = api.ReadString("Parent", "");
            Measure measure;
            if (String.IsNullOrEmpty(parent))
            {
                measure = new ParentMeasure();
            }
            else
            {
                measure = new ChildMeasure();
            }

            data = GCHandle.ToIntPtr(GCHandle.Alloc(measure));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        { 
            GCHandle.FromIntPtr(data).Free();

            if (StringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.Reload(new Rainmeter.API(rm), ref maxValue);
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            return measure.Update();
        }

        [DllExport]
        public static IntPtr GetString(IntPtr data)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            if (StringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(StringBuffer);
                StringBuffer = IntPtr.Zero;
            }

            String StringValue = measure.GetString();
            if (StringValue != null)
            {
                StringBuffer = Marshal.StringToHGlobalUni(StringValue);
            }

            return StringBuffer;
        }
        [DllExport]
        public static void ExecuteBang(IntPtr data, IntPtr args)
        {
            Measure measure = (Measure)GCHandle.FromIntPtr(data).Target;
            measure.ExecuteBang(Marshal.PtrToStringUni(args));
        }
    }
}
