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
    class UserSettingsMidNameModel
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Отество клиента.
        /// </summary>
        public string MiddleName { get; set; }
    }
}