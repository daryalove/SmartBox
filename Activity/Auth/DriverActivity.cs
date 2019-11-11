using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGeometry.Model.Auth;
using Android.App;
using Android.Content;
using Android.OS;
using System.Text.Json;
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
using GeoGeometry.Model.Box;
using GeoGeometry.Model;

namespace GeoGeometry.Activity.Auth
{

    [Activity(Label = "DriverActivity")]

    public class DriverActivity : AppCompatActivity
    {

        private Button btn_exit_;

        private Button btn_change_container;

        private Button btn_open_close_container;

        private Button btn_lock_unlock_door;

        private Button btn_change_parameters;

        private EditText s_user;

        private EditText container_id;

        private EditText s_situation;

        private EditText s_open_close_container;

        private EditText s_lock_unlock_door;

        private EditText s_pin_access_code;

        private EditText s_weight;

        private EditText s_temperature;

        private EditText s_light;

        private EditText s_humidity; 

        private EditText s_battery;

        private EditText s_signal_strength;

        private EditText s_longitude;

        private EditText s_latitude;

        private EditText s_date;

        private EditText s_time;

        private ProgressBar preloader;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_driver);

            
            btn_change_container = FindViewById<Button>(Resource.Id.btn_change_container);
            btn_exit_ = FindViewById<Button>(Resource.Id.btn_exit_);
            btn_open_close_container = FindViewById<Button>(Resource.Id.btn_open_close_container);
            btn_lock_unlock_door = FindViewById<Button>(Resource.Id.btn_lock_unlock_door);
            btn_change_parameters = FindViewById<Button>(Resource.Id.btn_change_parameters);
            s_user = FindViewById<EditText>(Resource.Id.s_user);
            container_id = FindViewById<EditText>(Resource.Id.container_id);
            s_situation = FindViewById<EditText>(Resource.Id.s_situation);
            s_open_close_container = FindViewById<EditText>(Resource.Id.s_open_close_container);
            s_lock_unlock_door = FindViewById<EditText>(Resource.Id.s_lock_unlock_door);
            s_pin_access_code = FindViewById<EditText>(Resource.Id.s_pin_access_code);
            s_weight = FindViewById<EditText>(Resource.Id.s_weight);
            s_temperature = FindViewById<EditText>(Resource.Id.TemperatureEdit);
            s_light = FindViewById<EditText>(Resource.Id.s_light);
            s_humidity = FindViewById<EditText>(Resource.Id.s_humidity);
            s_battery = FindViewById<EditText>(Resource.Id.s_battery);
            s_signal_strength = FindViewById<EditText>(Resource.Id.s_signal_strength);
            s_longitude = FindViewById<EditText>(Resource.Id.s_longitude);
            s_latitude = FindViewById<EditText>(Resource.Id.s_latitude);
            s_date = FindViewById<EditText>(Resource.Id.s_date);
            s_time = FindViewById<EditText>(Resource.Id.s_time);
            preloader = FindViewById<ProgressBar>(Resource.Id.preloader);
          

            btn_change_container.Click += async delegate
            {
                Intent ContainerSelectionActivty = new Intent(this, typeof(Auth.ContainerSelection));
                StartActivity(ContainerSelectionActivty);
            };
            /*
            btn_change_parameters.Click += async delegate
            {
                try
                {
                    preloader.Visibility = Android.Views.ViewStates.Visible;

                    RegisterBoxModel parameters = new RegisterBoxModel
                    {
                        Temperature = s_temperature.Text,
                        Weight = s_weight.Text,
                        Light = s_light.Text,
                        Humidity = s_humidity.Text,
                          
                    };
                    var myHttpClient = new HttpClient();

                    var uri = new Uri("http://iot-tmc-cen.1gb.ru/api/container/create?name=" + register.Name);
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "" + ex.Message, ToastLength.Long).Show();
                }
            };
            */

        }
    }
}


