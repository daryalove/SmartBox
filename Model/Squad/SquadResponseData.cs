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
    class SquadResponseData
    {
        /// <summary>
        /// Id отряда.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Id командира.
        /// </summary>
        public string LeaderId { get; set; }

        /// <summary>
        /// Командир.
        /// </summary>
        public string LeaderName { get; set; }

        /// <summary>
        /// Название отряда.
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Описание отряда.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Город отряда.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Дата основания.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Соц сети.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Подразделение.
        /// </summary>
        public string Section { get; set; }

        /// <summary>
        /// Флаг командира.
        /// </summary>
        public bool IsLeader { get; set; }

        /// <summary>
        /// Картинка отряда.
        /// </summary>
        public string Symbol { get; set; }
    }
}