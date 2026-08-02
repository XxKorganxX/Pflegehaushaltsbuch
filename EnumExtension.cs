using Pflegehaushaltsbuch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
namespace Pflegehaushaltsbuch
{
    /// <summary>
    /// Provides helper methods for enum Extensions operations used by the application.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the display Name value from the current application state.
        /// </summary>
        public static string GetDisplayName(this Enum e)
        {
            var rm = new ResourceManager(typeof(EnumResources));
            var resourceDisplayName = rm.GetString(e.GetType().Name + "_" + e);
            if (resourceDisplayName == null)
                return e.ToString();
            else
                return resourceDisplayName;
        }
    }
}
