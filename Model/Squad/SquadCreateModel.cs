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
    class SquadCreateModel
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Название отряда.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание отряда.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Город отряда.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Дата рождения.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Отделение
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Ссылка на соц.сеть.
        /// </summary>
        public string URL { get; set; }
    }
}