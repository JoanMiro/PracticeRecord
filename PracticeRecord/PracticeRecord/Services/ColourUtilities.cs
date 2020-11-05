//------------------------------------------------------------------
//
// Copyright (c) 2012 - 2020 Openfeature Limited. All rights reserved.
//
//------------------------------------------------------------------

namespace PracticeRecord.Services
{
    using System.Collections.Generic;
    using System.Reflection;
    using Xamarin.Forms;

    public static class ColourUtilities
    {
        private static readonly Dictionary<string, Color> ColourDictionary = new Dictionary<string, Color>();

        public static Dictionary<string, Color> Colours
        {
            get
            {
                if (ColourDictionary.Count == 0)
                {
                    foreach (var field in typeof(Color).GetFields(BindingFlags.Static | BindingFlags.Public))
                    {
                        ColourDictionary.Add(field.Name, (Color)field.GetValue(Application.Current));
                    }
                }

                return ColourDictionary;
            }
        }
    }
}