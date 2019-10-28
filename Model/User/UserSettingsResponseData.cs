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
    class UserSettingsResponseData
    {
        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Почта клиента.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Подтверждение почты.
        /// </summary>
        public bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Является ли лидером клиент.
        /// </summary>
        public bool IsLeader { get; set; }

        /// <summary>
        /// Запомнить.
        /// </summary>
        public bool BrowserRemembered { get; set; }
    }
}