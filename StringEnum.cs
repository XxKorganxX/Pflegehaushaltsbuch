using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Provides helper methods for enum Helper operations used by the application.
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Creates the description booking data for the current workflow.
        /// </summary>
        public static string ToDescription(this Enum value)
        {
            var da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return da.Length > 0 ? da[0].Description : value.ToString();
        }
    }
}
