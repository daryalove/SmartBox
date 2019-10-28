using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using GeoGeometry.Activity.Menu;
using GeoGeometry.Model.User;
using Newtonsoft.Json;
using Android.Gms.Location;
using GeoGeometry.Model.GPSLocation;
using System.Threading.Tasks;

namespace GeoGeometry.Activity.Home {
	[Activity(Label = "HomeActivity")]
    public class HomeActivity : AppCompatActivity, IOnMapReadyCallback
    {
		/// <summary>
		/// Имя клиента.
		/// </summary>
		public TextView s_user;
		RelativeLayout home_container;

		GoogleMap _googleMap;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_home);

            StaticMenu.id_page = 1;

            home_container = FindViewById<RelativeLayout>(Resource.Id.home_container);
            s_user = FindViewById<TextView>(Resource.Id.s_user);

            s_user.Text = StaticUser.FirstName + " " + StaticUser.LastName;

            MapFragment mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.fragmentMap);
            mapFragment.GetMapAsync(this);

            BuildLocationRequest();
            BuildLocationCallBack();

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);

            ResetUser();
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest,
                locationCallback, Looper.MyLooper());

			//Task.Delay(1000).ContinueWith(t =>
			//{
			//	OnMapReady(_googleMap);
			//}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		FusedLocationProviderClient fusedLocationProviderClient;
		LocationRequest locationRequest;
		LocationCallback locationCallback;

		/// <summary>
		/// Подключение к карте api ключ. 
		/// https://console.developers.google.com/apis/credentials?project=geogeometry&hl=RU&supportedpurview=project
		/// информация https://docs.microsoft.com/ru-ru/xamarin/android/platform/maps-and-location/maps/maps-api#google-maps-api-prerequisites
		/// </summary>
		public void OnMapReady(GoogleMap googleMap) {
			_googleMap = googleMap;

			MarkerOptions markerOptions = new MarkerOptions();
			LatLng location = new LatLng(StaticUser.Latitude, StaticUser.Longitude);
			markerOptions.SetPosition(location);
			markerOptions.SetTitle("Я здесь");
			googleMap.AddMarker(markerOptions);

			CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
			builder.Target(location);
			builder.Zoom(18);
			builder.Bearing(0);
			builder.Tilt(65);

			CameraPosition cameraPosition = builder.Build();
			CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

			googleMap.UiSettings.ZoomControlsEnabled = true;
			googleMap.UiSettings.CompassEnabled = true;
			googleMap.MoveCamera(cameraUpdate);
		}

		/// <summary>
		/// Подгружаю информацию о клиенте.
		/// </summary>
		private async void ResetUser() {
			// Получаю информацию о клиенте.
			UserModel userJson = new UserModel {
				UserId = StaticUser.UserId
			};

			var myHttpClient = new HttpClient();

			var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/loggedin/");

			//json структура.
			var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
			{
				{ "userId", userJson.UserId }
			});

			HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

			string s_result;
			using (HttpContent responseContent = response.Content) {
				s_result = await responseContent.ReadAsStringAsync();
			}

			UserApiData o_data = JsonConvert.DeserializeObject<UserApiData>(s_result);
			UserResponseData o_user_data = new UserResponseData();
			o_user_data = o_data.ResponseData;

			StaticUser.AddInfoUser(o_user_data);

			// Получаю данные об настройках клиента.
			uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/usersettings?userId=" + userJson.UserId);

			HttpResponseMessage responseUserSettings = await myHttpClient.GetAsync(uri);

			string s_resultUserSettings = "";
			using (HttpContent responseContent1 = responseUserSettings.Content) {
				s_resultUserSettings = await responseContent1.ReadAsStringAsync();
			}

			UserSettingsApiData o_dataSettings = JsonConvert.DeserializeObject<UserSettingsApiData>(s_resultUserSettings);
			UserSettingsResponseData o_user_dataSettings = new UserSettingsResponseData();
			o_user_dataSettings = o_dataSettings.ResponseData;

			StaticUser.AddInfoUserSettings(o_user_dataSettings);
		}

		private void BuildLocationCallBack()
        {
            locationCallback = new HomeLocationCallBack(this);
        }

        private void BuildLocationRequest()
        {
            locationRequest = new LocationRequest();
            locationRequest.SetPriority(LocationRequest.PriorityBalancedPowerAccuracy);
            locationRequest.SetInterval(1000);
            locationRequest.SetFastestInterval(3000);
            locationRequest.SetSmallestDisplacement(10f);
        }
    }

    internal class HomeLocationCallBack : LocationCallback
    {
        private HomeActivity homeActivity;

        public HomeLocationCallBack(HomeActivity homeActivity)
        {
            this.homeActivity = homeActivity;
        }

        public override async void OnLocationResult(LocationResult result)
        {
            base.OnLocationResult(result);

            StaticUser.Latitude = result.LastLocation.Latitude;
            StaticUser.Longitude = result.LastLocation.Longitude;

            // Получаю информацию о клиенте.
            GPSLocationModel gpsLocation = new GPSLocationModel
            {
                UserId = StaticUser.UserId,
                Lat1 = StaticUser.Latitude,
                Lon1 = StaticUser.Longitude
            };

            var myHttpClient = new HttpClient();
            var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/setuserlocation");

            //json структура.
            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "UserId", gpsLocation.UserId },
                { "Lon1", gpsLocation.Lon1.ToString()},
                { "Lat1", gpsLocation.Lat1.ToString()}
            });

            HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);
        }
    }
}