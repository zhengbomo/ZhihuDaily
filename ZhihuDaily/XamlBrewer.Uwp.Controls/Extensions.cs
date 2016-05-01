using System;
using static System.Double;

namespace XamlBrewer.Uwp.Controls
{
    internal static class Extensions
    {
        internal static bool IsNaN(this double d)
        {
            return double.IsNaN(d);
        }
    }
}
