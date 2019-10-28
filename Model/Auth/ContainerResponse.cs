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
using GeoGeometry.Model.Auth;

namespace GeoGeometry.Model.User
{
    public class ContainerResponse: BaseResponseObject
    {
        /// <summary>
        /// Id контейнера.
        /// </summary>
        public string SmartBoxId { get; set; }

        

        /// <summary>
        /// Наименование контейнера.
        /// </summary>
        public string Name { get; set; }
       
    }
}