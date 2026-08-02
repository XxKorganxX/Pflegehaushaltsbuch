using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Pflegehaushaltsbuch.Data
{
    /// <summary>
    /// Represents the ID Client Data component used by the application.
    /// </summary>
    public class ID_Client_Data
    {
        public string Name { get; set; }
        public int ID { get; set; }
        /// <summary>
        /// Creates the string booking data for the current workflow.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }
}
