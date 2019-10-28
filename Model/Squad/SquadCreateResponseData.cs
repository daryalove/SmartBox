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

namespace GeoGeometry.Model.Squad
{
    class SquadCreateResponseData
    {
        /// <summary>
        /// Id отряда.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Название отряда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Командир.
        /// </summary>
        public string Leader { get; set; }
    }
}