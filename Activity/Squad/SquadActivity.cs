using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using GeoGeometry.Model.Squad;
using GeoGeometry.Model.User;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.Squad
{
    [Activity(Label = "SquadActivity")]
    public class SquadActivity : AppCompatActivity
    {
        /// <summary>
        /// Название отряда.
        /// </summary>
        private TextView name_squad;

        /// <summary>
        /// Картинка отряда.
        /// </summary>
        private ImageView img_squad;

        /// <summary>
        /// Командир отряда.
        /// </summary>
        private TextView s_leader;

        /// <summary>
        /// Кнопка настроек отряда.
        /// </summary>
        private ImageButton btn_settings_squad;

        /// <summary>
        /// Описание отряда.
        /// </summary>
        private TextView s_desc;

        /// <summary>
        /// Дата основания.
        /// </summary>
        private TextView s_date;

        /// <summary>
        /// Город отряда.
        /// </summary>
        private TextView s_city;
        private TextView label_city;

        /// <summary>
        /// Соц сети.
        /// </summary>
        private TextView s_social;

        protected override void  OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_squad);

            name_squad = FindViewById<TextView>(Resource.Id.name_squad);
            s_desc = FindViewById<TextView>(Resource.Id.s_desc);
            s_leader = FindViewById<TextView>(Resource.Id.s_leader);
            s_date = FindViewById<TextView>(Resource.Id.s_date);
            s_city = FindViewById<TextView>(Resource.Id.s_city);
            label_city = FindViewById<TextView>(Resource.Id.label_city);
            s_social = FindViewById<TextView>(Resource.Id.s_social);

            img_squad = FindViewById<ImageView>(Resource.Id.img_squad);

            btn_settings_squad = FindViewById<ImageButton>(Resource.Id.btn_settings_squad);

            btn_settings_squad.Click += (s, e) =>
            {
                Intent squadSettingsActivity = new Intent(this, typeof(SquadSettingsActivity));
                StartActivity(squadSettingsActivity);
            };

            string id_page = Intent.GetStringExtra("idPage");

            switch (id_page)
            {
                case "1": //Регистрация.
                case "2": //Клиент.
                    {
                        var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/getteam?Id=" + StaticUser.TeamId + "&userId=" + StaticUser.UserId);

                        GetInfoAboutSquad(uri);
                    }
                    break;
                case "3": //Список команд.
                    {
                        string teamId = Intent.GetStringExtra("teamId");
                        var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/getteam?Id=" + teamId + "&userId=" + StaticUser.UserId);

                        GetInfoAboutSquad(uri);
                    }
                    break;
            }
        }

        /// <summary>
        /// Метод собирает информацию об отряде.
        /// </summary>
        /// <param name="uri"></param>
        private async void GetInfoAboutSquad(Uri uri)
        {
            var myHttpClient = new HttpClient();
            
            HttpResponseMessage response = await myHttpClient.GetAsync(uri);

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            SquadApiData o_data = JsonConvert.DeserializeObject<SquadApiData>(s_result);
            SquadResponseData o_data_squad = new SquadResponseData();
            o_data_squad = o_data.ResponseData;

            if(o_data_squad.Symbol!=null)
            {
                //Загрузка картинки.
                byte[] imageAsBytes = Base64.Decode(o_data_squad.Symbol, Base64Flags.Default);
                Bitmap squad_logo = BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
                img_squad.SetImageBitmap(squad_logo);
            }
           
            name_squad.Text = o_data_squad.TeamName;
            s_desc.Text = o_data_squad.Description;
            s_leader.Text = o_data_squad.LeaderName;
            s_date.Text = o_data_squad.Date.ToString("d");
            s_social.Text = o_data_squad.Url;

            if (!String.IsNullOrEmpty(o_data_squad.City))
            {
                s_city.Visibility = ViewStates.Visible;
                label_city.Visibility = ViewStates.Visible;
                s_city.Text = o_data_squad.City;
            }
            else
            {
                label_city.Visibility = ViewStates.Gone;
                s_city.Visibility = ViewStates.Gone;
            }

            if(o_data_squad.LeaderId==StaticUser.UserId)
                btn_settings_squad.Visibility = ViewStates.Visible;
            else
                btn_settings_squad.Visibility = ViewStates.Invisible;
        }
    }
}