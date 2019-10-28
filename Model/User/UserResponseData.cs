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
    class UserResponseData
    {
        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Отряд клиента.
        /// </summary>
        public string Team  { get; set; }

        /// <summary>
        /// ID Отряд клиента.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Направление отряда.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Ранг клиента.
        /// </summary>
        public string Rang { get; set; }

        /// <summary>
        /// Фото клиента.
        /// </summary>
        public string Imagines { get; set; }
    }
}