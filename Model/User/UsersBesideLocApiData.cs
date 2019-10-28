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
    class UsersBesideLocApiData
    {
        /// <summary>
              /// Статус ответа.
        /// </summary>
        /// 
        // для комита
        public string Status { get; set; }

        /// <summary>
        /// Сообщение ответа.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Список клиентов в радиусе 500 метров
        /// </summary>
        public UsersBesideLocResponseData ResponseData { get; set; }
    }
}