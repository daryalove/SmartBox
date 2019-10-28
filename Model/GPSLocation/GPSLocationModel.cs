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

namespace GeoGeometry.Model.GPSLocation
{
    class GPSLocationModel
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Lon1 { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Lat1 { get; set; }
    }
}