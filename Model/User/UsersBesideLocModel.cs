using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Model.User
{
  public class UsersBesideLocModel
    {   
        ///<summary>
        /// ID 
        /// </summary>   
        public string UserId { get; set; }

        ///<summary>
        /// широта
        /// </summary>
        public double Latitude { get; set; }

        ///<summary>
        /// долгота
        /// </summary>
        public double Longitude { get; set; }
    }
}