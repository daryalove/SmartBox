using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.User;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.Auth {
	[Activity(Label = "AuthActivity")]
    public class AuthActivity : AppCompatActivity
    {
        /// <summary>
        /// Почта клиента
        /// </summary>
        private EditText s_login;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private EditText s_pass;

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        private CheckBox is_remember;

        /// <summary>
        /// Конпка регистрации.
        /// </summary>
        private Button btn_register;

        /// <summary>
        /// Конпка авторизации.
        /// </summary>
        private Button btn_auth;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

		private ProgressBar preloader;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_auth);

            btn_register = FindViewById<Button>(Resource.Id.btn_register);
            btn_auth = FindViewById<Button>(Resource.Id.btn_auth);
            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            s_login = FindViewById<EditText>(Resource.Id.s_login);
            s_pass = FindViewById<EditText>(Resource.Id.s_pass);
			is_remember = FindViewById<CheckBox>(Resource.Id.is_remember);
			preloader = FindViewById<ProgressBar>(Resource.Id.loader);

			btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            string dir_path = "/storage/emulated/0/Android/data/GeoGeometry.GeoGeometry/files/";
            string file_data_remember = "0";
            // Проверяю запомнил ли пользователя.
            if (File.Exists(@"" + dir_path + "user_data.txt"))
            {
                using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.Open, FileAccess.Read))
                {
                    // преобразуем строку в байты
                    byte[] array = new byte[file.Length];
                    // считываем данные
                    file.Read(array, 0, array.Length);
                    // декодируем байты в строку
                    file_data_remember = Encoding.Default.GetString(array);
                    file.Close();
                }
            }

            if (file_data_remember.Substring(0, 1) == "1")
            {
				preloader.Visibility = Android.Views.ViewStates.Visible;
				file_data_remember = file_data_remember.Remove(0, 1);
                AuthResponseData o_data = JsonConvert.DeserializeObject<AuthResponseData>(file_data_remember);

                StaticUser.AddInfoAuth(o_data);

				preloader.Visibility = Android.Views.ViewStates.Gone;

				// Переход на главную страницу.
				Intent homeActivity = new Intent(this, typeof(Home.HomeActivity));
                StartActivity(homeActivity);
                //this.Finish();
            }
            else
            {
                // Переход к форме регистрации.
                btn_register.Click += (s, e) =>
                {
                    Intent registerActivity = new Intent(this, typeof(Auth.RegisterActivity));
                    StartActivity(registerActivity);
                };

                btn_auth.Click += async delegate
                {
                    try
                    {
						preloader.Visibility = Android.Views.ViewStates.Visible;
						// Авторизируюсь клиентом.
						AuthModel auth = new AuthModel
                        {
                            Email = s_login.Text,
                            Password = s_pass.Text
                        };

                        var myHttpClient = new HttpClient();
                        var _authHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", auth.Email, auth.Password))));

                        myHttpClient.DefaultRequestHeaders.Authorization = _authHeader;

                        var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/login/");

                        //json структура.
                        var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "Email", auth.Email },
                            { "Password", auth.Password }
                        });
                        // Поучаю ответ об авторизации [успех или нет]
                        HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

                        string s_result;
                        using (HttpContent responseContent = response.Content)
                        {
                            s_result = await responseContent.ReadAsStringAsync();
                        }

                        AuthApiData<AuthResponseData> o_data = JsonConvert.DeserializeObject<AuthApiData<AuthResponseData>>(s_result);

                        ClearField();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            if (o_data.Status == "0")
                            {
                                Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                                AuthResponseData o_user_data = new AuthResponseData();
                                o_user_data = o_data.ResponseData;

                                StaticUser.AddInfoAuth(o_user_data);

                                string is_check = is_remember.Checked ? "1" : "0";

                                using (FileStream file = new FileStream(dir_path + "user_data.txt", FileMode.Create, FileAccess.Write))
                                {
                                    // преобразуем строку в байты
                                    byte[] array = Encoding.Default.GetBytes(is_check + JsonConvert.SerializeObject(o_user_data));
                                    // запись массива байтов в файл
                                    file.Write(array, 0, array.Length);
                                }

								//Начинаю собирать информацию о клиенте
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
            }
        }

		/// <summary>
		/// Removes activity from history after navigating to new activity.
		/// </summary>
		protected override void OnStop() {
			base.OnStop();
			Finish();
		}

		/// <summary>
		/// Closes app if back button is pressed.
		/// </summary>
		public override void OnBackPressed()
        {
            if (FragmentManager.BackStackEntryCount > 0)
            {
                FragmentManager.PopBackStack();
            }
            else
            {
                base.OnBackPressed();
            }
        }

        /// <summary>
        /// Метод очистки полей.
        /// </summary>
        void ClearField()
        {
            s_login.Text = "";
            s_pass.Text = "";
        }
    }
}