﻿using System;
using System.Collections.Generic;
using System.Net.Http;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using GeoGeometry.Model.Auth;
using System.Net;
using Android.Support.V7.App;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using GeoGeometry.Model.User;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        /// <summary>
        /// Имя клиента.
        /// </summary>
        private EditText s_first_name;

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        private EditText s_last_name;

        /// <summary>
        /// Почта клиента
        /// </summary>
        private EditText s_email;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private EditText s_pass;

        /// <summary>
        /// Подтвержденный пароль клиента.
        /// </summary>
        private EditText s_pass_check;
        
        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_register;

        private Spinner s_role_name;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

		private ProgressBar preloader;

        private string s_section_role;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_register);

            btn_register = FindViewById<Button>(Resource.Id.btn_register);
            s_first_name = FindViewById<EditText>(Resource.Id.s_first_name);
            s_last_name = FindViewById<EditText>(Resource.Id.s_last_name);
            s_pass = FindViewById<EditText>(Resource.Id.s_pass);
            s_pass_check = FindViewById<EditText>(Resource.Id.s_pass_check);
            s_email = FindViewById<EditText>(Resource.Id.s_email);
            s_role_name = FindViewById<Spinner>(Resource.Id.s_role_name);

            preloader = FindViewById<ProgressBar>(Resource.Id.loader);

			btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            s_role_name.Prompt = "Выбор роли";
            s_role_name.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.a_role, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            s_role_name.Adapter = adapter;


            string dir_path = "/storage/emulated/0/Android/data/GeoGeometry.GeoGeometry/files/";

            btn_register.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;

                    RegisterModel register = new RegisterModel
                    {
                        FirstName = s_first_name.Text,
                        LastName = s_last_name.Text,
                        Email = s_email.Text,
                        Password = s_pass.Text,
                        PasswordConfirm = s_pass_check.Text,
                      
                    };
                    if (s_section_role == "Водитель")
                    {
                        register.RoleName = "driver";
                    }
                    else if (s_section_role == "Пользователь") {
                        register.RoleName = "user";
                    }
                    var myHttpClient = new HttpClient();
                    var uri = new Uri(" http://iot-tmc-cen.1gb.ru/api/auth/register?email=" + register.Email + "&firstname=" + register.FirstName + "&lastname=" + register.LastName + "&password=" + register.Password + "&passwordconfirm=" + register.PasswordConfirm + "&rolename=" + register.RoleName);
                    //var uri = "../register?email="+register.Email+"&firstname="+register.FirstName+"&lastname="+register.LastName+"&password="+register.Password+"&passwordconfirm=" + register.PasswordConfirm +"&rolename=" + register.RoleName;
                    var _authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", register.FirstName, register.LastName, register.Password, register.PasswordConfirm, register.Email, register.RoleName))));
                /*
                //json структура.
                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "FirstName", register.FirstName },
                    { "LastName", register.LastName },
                    { "Email", register.Email },
                    { "Password", register.Password },
                    { "PasswordConfirm", register.PasswordConfirm }
                });
                */

                HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), new StringContent(JsonConvert.SerializeObject(register), Encoding.UTF8, "application/json"));

                string s_result;
                using (HttpContent responseContent = response.Content)
                {
                    s_result = await responseContent.ReadAsStringAsync();
                }

                AuthApiData<AuthResponseData> o_data = JsonConvert.DeserializeObject<AuthApiData<AuthResponseData>>(s_result);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    if (o_data.Status == "0")
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                        AuthResponseData o_user_data = new AuthResponseData();
                        o_user_data = o_data.ResponseData;

                        StaticUser.AddInfoAuth(o_user_data);

                        using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.Create, FileAccess.Write))
                        {
                            // преобразуем строку в байты
                            byte[] array = Encoding.Default.GetBytes("0" + JsonConvert.SerializeObject(o_user_data));
                            // запись массива байтов в файл
                            file.Write(array, 0, array.Length);
                        }
                        if(register.RoleName == "driver")
                        {
                                Intent Driver = new Intent(this, typeof(Auth.DriverActivity));
                                StartActivity(Driver);
                                this.Finish();
                         }
                        /*else if(register.RoleName == "user")
                            {
                                Intent User = new Intent(this, typeof(Auth.UserActivity));
                                StartActivity(User);
                                this.Finish();
                            }
                            */
                        preloader.Visibility = Android.Views.ViewStates.Invisible;
                        // Переход на страницу выбора контейнера.
                        //Intent ContainerSelectionActivity = new Intent(this, typeof(Auth.ContainerSelection));
                        //StartActivity(ContainerSelectionActivity);
                        //this.Finish();
                    }
                    else
                    {
                        Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();
                    }
                }
            }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };

    }
        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            s_section_role = spinner.GetItemAtPosition(e.Position).ToString();
        }
        /// <summary>
        /// Метод очистки полей.
        /// </summary>
        /*void ClearField()
        {
            s_first_name.Text = "";
            s_last_name.Text = "";
            s_email.Text = "";
            s_pass.Text = "";
            s_pass_check.Text = "";
        }
        */
    }
}