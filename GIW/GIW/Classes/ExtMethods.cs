﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GIW.Classes
{
    public static class ExtMethods
    {
        //This extension method is used to search the contacts listview
        public static bool Contains (this string source, string toCheck, StringComparison comparisonType)
        {
            return (source.IndexOf(toCheck, comparisonType) >= 0);
        }
    }
}
