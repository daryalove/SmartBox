using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGeometry.Model.Auth;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using GeoGeometry.Container;
using System.IO;
using GeoGeometry.Model.User;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "RegisterBoxActivity")]
    class RegisterBoxActivity: AppCompatActivity
    {

        /// <summary>
        /// Имя клиента.
        /// </summary>
        private EditText box_name;

        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_registerbox;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

        /// <summary>
        /// Для прокрутки страницы.
        /// </summary>
        private ProgressBar preloader;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_registerbox);


            box_name = FindViewById<EditText>(Resource.Id.box_name);
            btn_registerbox = FindViewById<Button>(Resource.Id.btn_registerbox);
            preloader = FindViewById<ProgressBar>(Resource.Id.loader);
            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            string dir_path = "/storage/emulated/0/Android/data/GeoGeometry.GeoGeometry/files/";

            btn_registerbox.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;

                    //RegisterModel register = new RegisterModel
                    //{
                    //    BoxName = box_name.Text,
                    //};

                    var myHttpClient = new HttpClient();
                    var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/create/");

                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "BoxName", box_name.Text },
                    });

                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    AuthApiData<ContainerResponse> o_data = JsonConvert.DeserializeObject<AuthApiData<ContainerResponse>>(s_result);

                    ClearField();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (o_data.Status == "0")
                        {
                            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                            ContainerResponse o_user_data = new ContainerResponse();
                            o_user_data = o_data.ResponseData;

                            StaticBox.AddInfoAuth(o_user_data);

                            using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.Create, FileAccess.Write))
                            {
                                // преобразуем строку в байты
                                byte[] array = Encoding.Default.GetBytes("0" + JsonConvert.SerializeObject(o_user_data));
                                // запись массива байтов в файл
                                file.Write(array, 0, array.Length);
                            }

                            preloader.Visibility = Android.Views.ViewStates.Invisible;

                            // Переход на главную страницу.
                            Intent homeActivity = new Intent(this, typeof(Home.HomeActivity));
                            StartActivity(homeActivity);
                            this.Finish();
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

            /// <summary>
            /// Метод очистки полей.
            /// </summary>
            void ClearField()
            {
                box_name.Text = "";
            }
        }
    }

}