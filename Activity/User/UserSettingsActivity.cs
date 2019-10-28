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
using GeoGeometry.Model.User;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.User
{
    [Activity(Label = "UserSettingsActivity")]
    public class UserSettingsActivity : Android.Support.V7.App.AppCompatActivity
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
        private EditText s_middle_name;

        /// <summary>
        /// Отряд клиента
        /// </summary>
        private TextView s_team;

        /// <summary>
        /// Ранг клиента
        /// </summary>
        private TextView s_rang;

        /// <summary>
        /// Почта клиента
        /// </summary>
        private TextView s_email;

        /// <summary>
        /// Телефон клиента
        /// </summary>
        private TextView phone;

        /// <summary>
        /// Направение отряда клиента
        /// </summary>
        private TextView s_section;

        /// <summary>
        /// Метка подтверждения почты
        /// </summary>
        private TextView s_email_check;

        /// <summary>
        /// Метка ожидания подтверждения
        /// </summary>
        private TextView s_email_wait;

        /// <summary>
        /// Кнопка подтверждения почты
        /// </summary>
        private TextView btn_email_check;

        /// <summary>
        /// Конпка создания отряда.
        /// </summary>
        private Button btn_create_squad;

        /// <summary>
        /// Конпка создания отряда.
        /// </summary>
        private ImageButton btn_save_mid_name;

        /// <summary>
        /// Конпка выхода.
        /// </summary>
        private Button btn_exit;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_user_settings);

            btn_create_squad = FindViewById<Button>(Resource.Id.btn_create_squad);
            btn_save_mid_name = FindViewById<ImageButton>(Resource.Id.btn_save_mid);
            btn_exit = FindViewById<Button>(Resource.Id.btn_exit);

            s_first_name = FindViewById<TextView>(Resource.Id.s_first_name);
            s_last_name = FindViewById<TextView>(Resource.Id.s_last_name);
            s_middle_name = FindViewById<EditText>(Resource.Id.s_middle_name);
            s_team = FindViewById<TextView>(Resource.Id.s_team);
            s_rang = FindViewById<TextView>(Resource.Id.s_rang);
            s_section = FindViewById<TextView>(Resource.Id.s_section);
            s_email = FindViewById<TextView>(Resource.Id.s_email);
            phone = FindViewById<TextView>(Resource.Id.s_phone);

            s_email_wait = FindViewById<TextView>(Resource.Id.s_email_wait);
            s_email_check = FindViewById<TextView>(Resource.Id.s_email_check);
            btn_email_check = FindViewById<TextView>(Resource.Id.btn_email_check);

            s_first_name.Text = StaticUser.FirstName;
            s_last_name.Text = StaticUser.LastName;
            s_middle_name.Text = StaticUser.MiddleName;
            s_rang.Text = StaticUser.Rang;
            s_team.Text = StaticUser.Team;
            s_section.Text = StaticUser.Section;
            s_email.Text = StaticUser.Email;

            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            string s_phone = "Номер отсутствует";
            if (!String.IsNullOrEmpty(StaticUser.PhoneNumber))
                s_phone = StaticUser.PhoneNumber;

            phone.Text = s_phone;

            // Переход выхода из аккаунта.
            btn_exit.Click += (s, e) =>
            {
                string dir_path = "/storage/emulated/0/Android/data/GeoGeometry.GeoGeometry/files/";
                File.Delete(dir_path + "user_data.txt");

                Intent mainActivity = new Intent(this, typeof(MainActivity));
                StartActivity(mainActivity);
                this.Finish();
            };

            if (StaticUser.Team == "Не состою")
            {
                btn_create_squad.Visibility = ViewStates.Gone;
                btn_create_squad.Click += (s, e) =>
                {
                    Intent registerSquad = new Intent(this, typeof(Squad.SquadRegisterActivity));
                    StartActivity(registerSquad);
                };
            }

            s_middle_name.Clickable = false;
            s_middle_name.Enabled = false;
            btn_save_mid_name.Visibility = ViewStates.Invisible;
            if (StaticUser.MiddleName== "Не указано")
            {
                s_middle_name.Clickable = true;
                s_middle_name.Enabled = true;
                btn_save_mid_name.Visibility = ViewStates.Visible;

                btn_save_mid_name.Click += async delegate
                {
                    UserSettingsMidNameModel userMidName = new UserSettingsMidNameModel
                    {
                        UserId = StaticUser.UserId,
                        MiddleName = s_middle_name.Text
                    };

                    var myHttpClient = new HttpClient();
                    var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/addmiddlename/");

                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "UserId", userMidName.UserId },
                        { "MiddleName", userMidName.MiddleName }
                    });

                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    UserSettingsMidNameApiData o_data = JsonConvert.DeserializeObject<UserSettingsMidNameApiData>(s_result);

                    Toast.MakeText(this, "" + o_data.Message, ToastLength.Long).Show();

                    s_middle_name.Clickable = false;
                    s_middle_name.Enabled = false;
                    btn_save_mid_name.Visibility = ViewStates.Invisible;
                };
            }

            s_email_wait.Visibility = ViewStates.Invisible;
            if (StaticUser.IsEmailConfirmed)
            {
                s_email_check.Visibility = ViewStates.Visible;
                btn_email_check.Visibility = ViewStates.Invisible;
            }
            else
            {
                btn_email_check.Visibility = ViewStates.Visible;
                s_email_check.Visibility = ViewStates.Invisible;

                btn_email_check.Click += async delegate
                {
                    var myHttpClient = new HttpClient();
                    // Получаю данные об настройках клиента.
                    var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/confirmaccount?userId=" + StaticUser.UserId);

                    HttpResponseMessage response = await myHttpClient.GetAsync(uri);

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    UserSettingsMidNameApiData o_data = JsonConvert.DeserializeObject<UserSettingsMidNameApiData>(s_result);

                    Toast.MakeText(this, "" + o_data.Message, ToastLength.Long).Show();

                    s_email_wait.Visibility = ViewStates.Visible;
                    btn_email_check.Visibility = ViewStates.Invisible;
                };
            }
        }
    }
}