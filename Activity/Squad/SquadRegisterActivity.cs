using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Model.Squad;
using GeoGeometry.Model.User;
using Newtonsoft.Json;
using static Android.App.DatePickerDialog;

namespace GeoGeometry.Activity.Squad
{
    [Activity(Label = "SquadRegisterActivity")]
    public class SquadRegisterActivity : AppCompatActivity, IOnDateSetListener
    {
        /// <summary>
        /// Название отряда.
        /// </summary>
        private EditText s_name;

        /// <summary>
        /// Описание отряда.
        /// </summary>
        private EditText s_desc;

        /// <summary>
        /// Город отряда.
        /// </summary>
        private EditText s_city;

        /// <summary>
        /// Выбор даты.
        /// </summary>
        private TextView dt_date;

        /// <summary>
        /// Специализация отряда.
        /// </summary>
        private Spinner s_section;

        /// <summary>
        /// Соц сети.
        /// </summary>
        private EditText s_social;

        /// <summary>
        /// Конпка регистрации отряда.
        /// </summary>
        private Button btn_create_squad;

        /// <summary>
        /// Конпка назад.
        /// </summary>
        private ImageButton btn_back_a;

        private const int DATE_DIALOG = 1;

        private int year, month, day;

        /// <summary>
        /// Данные 
        /// </summary>
        private string s_section_select;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_squad_register);

            btn_create_squad = FindViewById<Button>(Resource.Id.btn_create_squad);
            s_name = FindViewById<EditText>(Resource.Id.s_name);
            s_desc = FindViewById<EditText>(Resource.Id.s_desc);
            s_city = FindViewById<EditText>(Resource.Id.s_city);
            s_social = FindViewById<EditText>(Resource.Id.s_social);
            s_section = FindViewById<Spinner>(Resource.Id.s_section);
            dt_date = FindViewById<TextView>(Resource.Id.dt_date);

            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };

            dt_date.Click += (s, e) =>
            {
                ShowDialog(DATE_DIALOG);
            };

            s_section.Prompt = "Направление отряда";
            s_section.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.a_section, Android.Resource.Layout.SimpleSpinnerItem);

            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            s_section.Adapter = adapter;

            btn_create_squad.Click += async delegate
            {
                try
                {
                    SquadCreateModel squadReg = new SquadCreateModel
                    {
                        UserId = StaticUser.UserId,
                        Name = s_name.Text.Replace(System.Environment.NewLine, " "),
                        Description = s_desc.Text,
                        City = s_city.Text.Replace(System.Environment.NewLine, " "),
                        Section = s_section_select,
                        URL = s_social.Text,
                        Date = DateTime.Parse(dt_date.Text)                    
                    };

                    var myHttpClient = new HttpClient();
                    var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/createteam/");

                    //json структура.
                    var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "UserId", squadReg.UserId },
                        { "Name", squadReg.Name },
                        { "Description", squadReg.Description },
                        { "City", squadReg.City },
                        { "Section", squadReg.Section },
                        { "Date", squadReg.Date.ToString() },
                        { "URL", squadReg.URL }
                    });

                    HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    SquadCreateApiData o_data = JsonConvert.DeserializeObject<SquadCreateApiData>(s_result);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        if (o_data.Status == "0")
                        {
                            Toast.MakeText(this, o_data.Message, ToastLength.Long).Show();

                            SquadCreateResponseData o_user_squad = new SquadCreateResponseData();
                            o_user_squad = o_data.ResponseData;

                            StaticUser.Team = o_user_squad.Name;
                            StaticUser.TeamId = o_user_squad.TeamId;

                            // Переход на страницу отряда.
                            Intent squadActivity = new Intent(this, typeof(Squad.SquadActivity));
                            squadActivity.PutExtra("idPage", "1");
                            StartActivity(squadActivity);
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

        /// <summary>
        /// Считываю данные со spiner "dropDown"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = sender as Spinner;
            s_section_select = spinner.GetItemAtPosition(e.Position).ToString();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG:
                    {
                        return new DatePickerDialog(this, this, year, month, day);
                    }
                default:
                    return new DatePickerDialog(this);
            } 
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            this.year = year;
            this.month = month+1;
            this.day = dayOfMonth;

            dt_date.Text = day+"." + (month+1) + "."+ year;
        }
    }
}