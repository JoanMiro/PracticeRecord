namespace PracticeRecord.Extensions
{
    using System;
    using Services;
    using Xamarin.Forms;

    public static class ColourExtensions
    {
        public static Color ToColour(this string colourRgba)
        {
            var elements = colourRgba.Split(',');
            if (elements.Length == 1)
            {
                return ColourUtilities.Colours[elements[0]];
            }

            var colour = new Color(
                Convert.ToDouble(elements[1].Split('=')[1]), 
                Convert.ToDouble(elements[2].Split('=')[1]), 
                Convert.ToDouble(elements[3].Split('=')[1]), 
                Convert.ToDouble(elements[0].Split('=')[1]));

            return colour;
        }
    }
}