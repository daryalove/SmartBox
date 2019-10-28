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
    class SquadListResponseData
    {
        /// <summary>
        /// 
        /// </summary>
        public List<String> AuthUsers;

        /// <summary>
        /// 
        /// </summary>
        public List<String> TeamUsers;

        /// <summary>
        /// 
        /// </summary>
        public List<SquadListTeams> Teams;
    }
}