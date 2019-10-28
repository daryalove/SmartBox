using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Activity.Menu;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.User;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.User
{
    [Activity(Label = "UserActivity")]
    public class UserActivity : AppCompatActivity
    {
        /// <summary>
        /// Фото клиента
        /// </summary>
        //private ImageView img_user;

        /// <summary>
        /// Имя клиента
        /// </summary>
        private TextView s_first_name;

        /// <summary>
        /// Фамилия клиента
        /// </summary>
        private TextView s_last_name;

        /// <summary>
        /// Отчество клиента
        /// </summary>
        private TextView s_middle_name;

        /// <summary>
        /// Отряд клиента
        /// </summary>
        private TextView s_team;

        /// <summary>
        /// Ранг клиента
        /// </summary>
        private TextView s_rang;

        /// <summary>
        /// Конпка авторизации.
        /// </summary>
        private Button btn_settings;

        /// <summary>
        /// Кнопка перехода в отряд.
        /// </summary>
        private Button btn_squad;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_user);
            StaticMenu.id_page = 4;

            btn_settings = FindViewById<Button>(Resource.Id.btn_settings);
            btn_squad = FindViewById<Button>(Resource.Id.btn_squad);

            s_first_name = FindViewById<TextView>(Resource.Id.s_first_name);
            s_last_name = FindViewById<TextView>(Resource.Id.s_last_name);
            s_middle_name = FindViewById<TextView>(Resource.Id.s_middle_name);
            s_team = FindViewById<TextView>(Resource.Id.s_team);
            s_rang = FindViewById<TextView>(Resource.Id.s_rang);

            s_first_name.Text = StaticUser.FirstName;
            s_last_name.Text = StaticUser.LastName;

            if (StaticUser.MiddleName == "Не указано")
                s_middle_name.Visibility = ViewStates.Invisible;
            else
                s_middle_name.Text = StaticUser.MiddleName;

            s_rang.Text = StaticUser.Rang;
            s_team.Text = StaticUser.Team;

            // Переход к форме настройки.
            btn_settings.Click += (s, e) =>
            {
                Intent userSettingsActivity = new Intent(this, typeof(UserSettingsActivity));
                StartActivity(userSettingsActivity);
            };

            if (StaticUser.Team=="Не указано")
            {
                btn_squad.Visibility = ViewStates.Gone;
            } 
            else
            {
                btn_squad.Visibility = ViewStates.Visible;
                
            }

            // Переход к форме отряд.
            btn_squad.Click += (s, e) =>
            {
                Intent squadActivity = new Intent(this, typeof(Squad.SquadActivity));
                squadActivity.PutExtra("idPage", "2");
                StartActivity(squadActivity);
            };
        }
    }
}