using System;
using System.Runtime.InteropServices;
using Rainmeter;
using System.Collections.Generic;

namespace MaterialPalette
{
    class Swatch
    {
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

        public struct ColorSwatch
        {
            public readonly SwatchNames Name;
            public readonly String[] SwatchPrimaryColors;
            public readonly String[] SwatchAltColors;


            //Constructor including alt colors
            public ColorSwatch(SwatchNames name, String hexP500, String hexP50, String hexP100, String hexP200, String hexP300, String hexP400,
                   String hexP600, String hexP700, String hexP800, String hexP900, String hexA100, String hexA200, String hexA400, String hexA700)
            {
                Name = name;

                //I may make this in the future use the enum values to declare length and position
                SwatchPrimaryColors = new String[] { hexP500, hexP50, hexP100, hexP200, hexP300, hexP400, hexP600, hexP700, hexP800, hexP900 };
                SwatchAltColors = new String[] { hexA100, hexA200, hexA400, hexA700 };
            }

            //Constructor for swatches with no alt colors
            public ColorSwatch(SwatchNames name, String hexP500, String hexP50, String hexP100, String hexP200, String hexP300, String hexP400,
                   String hexP600, String hexP700, String hexP800, String hexP900)
            {
                Name = name;
                SwatchPrimaryColors = new String[] { hexP500, hexP50, hexP100, hexP200, hexP300, hexP400, hexP600, hexP700, hexP800, hexP900 };
                SwatchAltColors = new String[] { };
            }

        }

        public static readonly ColorSwatch[] ColorSwatches;

        static Swatch()
        {
            //This is calculating size wrong
            ColorSwatch[] colorSwatches = new ColorSwatch[Enum.GetNames(typeof(SwatchNames)).Length];

            //Sorry for this, it will likely be better in the future
            colorSwatches[(int)SwatchNames.Red] = new ColorSwatch(SwatchNames.Red, "#F44336",
                "#FFEBEE", "#FFCDD2", "#EF9A9A", "#E57373", "#EF5350", "#E53935", "#D32F2F", "#C62828", "#B71C1C",
                "#FF8A80", "#FF5252", "#FF1744", "#D50000");

            colorSwatches[(int)SwatchNames.Pink] = new ColorSwatch(SwatchNames.Pink, "#E91E63",
                "#FCE4EC", "#F8BBD0", "#F48FB1", "#F06292", "#EC407A", "#D81B60", "#C2185B", "#AD1457", "#880E4F",
                "#FF80AB", "#FF4081", "#F50057", "#C51162");

            colorSwatches[(int)SwatchNames.Purple] = new ColorSwatch(SwatchNames.Purple, "#9C27B0",
                "#F3E5F5", "#E1BEE7", "#CE93D8", "#BA68C8", "#AB47BC", "#8E24AA", "#7B1FA2", "#6A1B9A", "#4A148C",
                "#EA80FC", "#E040FB", "#D500F9", "#AA00FF");

            colorSwatches[(int)SwatchNames.DeepPurple] = new ColorSwatch(SwatchNames.DeepPurple, "673AB7",
                "EDE7F6", "D1C4E9", "B39DDB", "9575CD", "7E57C2", "5E35B1", "512DA8", "4527A0", "311B92",
                "B388FF", "7C4DFF", "651FFF", "6200EA");

            colorSwatches[(int)SwatchNames.Indigo] = new ColorSwatch(SwatchNames.Indigo, "3F51B5",
                "E8EAF6", "C5CAE9", "9FA8DA", "7986CB", "5C6BC0", "3949AB", "303F9F", "283593", "1A237E",
                "8C9EFF", "536DFE", "3D5AFE", "304FFE");

            colorSwatches[(int)SwatchNames.Blue] = new ColorSwatch(SwatchNames.Blue, "2196F3",
                "E3F2FD", "BBDEFB", "90CAF9", "64B5F6", "42A5F5", "1E88E5", "1976D2", "1565C0", "0D47A1",
                "82B1FF", "448AFF", "2979FF", "2962FF");

            colorSwatches[(int)SwatchNames.LightBlue] = new ColorSwatch(SwatchNames.LightBlue, "03A9F4",
                "E1F5FE", "B3E5FC", "81D4FA", "4FC3F7", "29B6F6", "039BE5", "0288D1", "0277BD", "01579B",
                "80D8FF", "40C4FF", "00B0FF", "0091EA");

            colorSwatches[(int)SwatchNames.Cyan] = new ColorSwatch(SwatchNames.Cyan, "00BCD4",
                "E0F7FA", "B2EBF2", "80DEEA", "4DD0E1", "26C6DA", "00ACC1", "0097A7", "00838F", "006064",
                "84FFFF", "18FFFF", "00E5FF", "00B8D4");

            colorSwatches[(int)SwatchNames.Teal] = new ColorSwatch(SwatchNames.Teal, "009688",
                "E0F2F1", "B2DFDB", "80CBC4", "4DB6AC", "26A69A", "00897B", "00796B", "00695C", "004D40",
                "A7FFEB", "64FFDA", "1DE9B6", "00BFA5");

            colorSwatches[(int)SwatchNames.Green] = new ColorSwatch(SwatchNames.Green, "4CAF50",
                "E8F5E9", "C8E6C9", "A5D6A7", "81C784", "66BB6A", "43A047", "388E3C", "2E7D32", "1B5E20",
                "B9F6CA", "69F0AE", "00E676", "00C853");

            colorSwatches[(int)SwatchNames.LightGreen] = new ColorSwatch(SwatchNames.LightGreen, "8BC34A",
                "F1F8E9", "DCEDC8", "C5E1A5", "AED581", "9CCC65", "7CB342", "689F38", "558B2F", "33691E",
                "CCFF90", "B2FF59", "76FF03", "64DD17");

            colorSwatches[(int)SwatchNames.Lime] = new ColorSwatch(SwatchNames.Lime, "CDDC39",
                "F9FBE7", "F0F4C3", "E6EE9C", "DCE775", "D4E157", "C0CA33", "AFB42B", "9E9D24", "827717",
                "F4FF81", "EEFF41", "C6FF00", "AEEA00");

            colorSwatches[(int)SwatchNames.Yellow] = new ColorSwatch(SwatchNames.Yellow, "FFEB3B",
                "FFFDE7", "FFF9C4", "FFF59D", "FFF176", "FFEE58", "FDD835", "FBC02D", "F9A825", "F57F17",
                "FFFF8D", "FFFF00", "FFEA00", "FFD600");

            colorSwatches[(int)SwatchNames.Amber] = new ColorSwatch(SwatchNames.Amber, "FFC107",
                "FFF8E1", "FFECB3", "FFE082", "FFD54F", "FFCA28", "FFB300", "FFA000", "FF8F00", "FF6F00",
                "FFE57F", "FFD740", "FFC400", "FFAB00");

            colorSwatches[(int)SwatchNames.Orange] = new ColorSwatch(SwatchNames.Orange, "FF9800",
                "FFF3E0", "FFE0B2", "FFCC80", "FFB74D", "FFA726", "FB8C00", "F57C00", "EF6C00", "E65100",
                "FFD180", "FFAB40", "FF9100", "FF6D00");

            colorSwatches[(int)SwatchNames.DeepOrange] = new ColorSwatch(SwatchNames.DeepOrange, "FF5722",
                "FBE9E7", "FFCCBC", "FFAB91", "FF8A65", "FF7043", "F4511E", "E64A19", "D84315", "BF360C",
                "FF9E80", "FF6E40", "FF3D00", "DD2C00");

            colorSwatches[(int)SwatchNames.Brown] = new ColorSwatch(SwatchNames.Brown, "795548",
                "EFEBE9", "D7CCC8", "BCAAA4", "A1887F", "8D6E63", "6D4C41", "5D4037", "4E342E", "3E2723");

            colorSwatches[(int)SwatchNames.Grey] = new ColorSwatch(SwatchNames.Grey, "9E9E9E",
                "FAFAFA", "F5F5F5", "EEEEEE", "E0E0E0", "BDBDBD", "757575", "616161", "424242", "212121");

            colorSwatches[(int)SwatchNames.BlueGrey] = new ColorSwatch(SwatchNames.BlueGrey, "607D8B",
                "ECEFF1", "CFD8DC", "B0BEC5", "90A4AE", "78909C", "546E7A", "455A64", "37474F", "263238");

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
            Alt
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
                String swatchName = api.ReadString("Name", null).Replace(" ", String.Empty).Replace("_", String.Empty);
                Swatch.SwatchNames currSwatchName;
                if (Enum.TryParse<Swatch.SwatchNames>(swatchName, out currSwatchName))
                {
                    currSwatch = Swatch.ColorSwatches[(int)currSwatchName];
                }
                else
                {
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
                    if(!colorName.StartsWith("P") && !colorName.StartsWith("A"))
                    {
                        colorName = "P" + colorName;
                    }

                    Swatch.SwatchPrimaryColors currPrimaryColor;
                    Swatch.SwatchAltColors currAltColor;

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
            if (myParent != null)
            {
                if (myParent.type == MeasureTypes.Swatch)
                {
                    //A little messy but I wanted two seperate arrays
                    if (myColorType == ColorTypes.Primary)
                    {
                        return myParent.currSwatch.SwatchPrimaryColors[myColorLoc];
                    }
                    else if (myColorType == ColorTypes.Alt)
                    {
                        return myParent.currSwatch.SwatchAltColors[myColorLoc];
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
