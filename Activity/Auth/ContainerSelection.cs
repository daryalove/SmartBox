using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoGeometry.Model.Auth;
using Android.App;
using Android.Content;
using Android.OS;
using Xamarin.CustomControls;
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
using Android.Gms.Common.Apis;
using System.Xml.Serialization;
using System.Xml;
using ServiceStack.Text.Common;

namespace GeoGeometry.Activity.Auth
{
    [Activity(Label = "ContainerSelection")]
    public class ContainerSelection : AppCompatActivity
    {
        //Creating Instance of AutoCompleteTextView and Button  
        private AutoCompleteTextView box_name1;

        private Button box_selection;

        private Button btn_box_registr;

        private ProgressBar preloader;

        private ImageButton btn_back_a;

       

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            //Class which encapsulate webservices.
           /* var _autoCompleteService = new AutoCompleteService();

            _searchView = v.FindViewById<AutoCompleteTextView>(Resource.Id.SearchBox);

            _searchView.TextChanged += async (sender, e) =>
            {
                try
                {
                    //async call which fetch suggestin from webservices.
                    var results = await _autoCompleteService.SearchSuggestionAsync(_searchView.Text);
                    var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, results.ToList());
                    _searchView.Adapter = adapter;
                }
                catch
                {
                }
            };

            _searchView.AfterTextChanged += (sender, e) =>
            {
                if (!_searchView.IsPopupShowing && _searchView.Text.Length > 0)
                {
                    _searchView.ShowDropDown();
                }
            };

            SetContentView(Resource.Layout.Main);
            ////////*/
    
            //setting the view for the Activity  
            ////////
            SetContentView(Resource.Layout.activity_container_selection);
            //Get autoComplete1 AutoCompleteTextView and btn_Hello Button control from the Main.xaml Layout.  

            box_name1 = FindViewById<AutoCompleteTextView>(Resource.Id.box_name1);
            box_selection = FindViewById<Button>(Resource.Id.box_selection);
            btn_box_registr = FindViewById<Button>(Resource.Id.btn_box_registr);
            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);
            preloader = FindViewById<ProgressBar>(Resource.Id.preloader);

            string dir_path = "/storage/emulated/0/Android/data/GeoGeometry.GeoGeometry/files/";
            //string array used for displayling AutoComplete Suggestion   
            /*var names = new string[] { "Anoop", "Arjit", "Akshay", "Ankit", "Rakesh" };
            //Use ArrayAdapter for filling the View in other words AutoCompleteTextView with the list of names(Array).  
            //Use SimpleSpinnerItem(Predefined layout in Android Resources) for displaying the dropdown list  
            ArrayAdapter adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, names);
            box_name1.Adapter = adapter;*/


            ///////////
            /*SetContentView(Resource.Layout.activity_container_selection);

            box_name1 = FindViewById<AutoCompleteTextView>(Resource.Id.box_name1);
            box_selection = FindViewById<Button>(Resource.Id.box_selection);
            btn_back_a = FindViewById<ImageButton>(Resource.Id.btn_back_a);
            preloader = FindViewById<ProgressBar>(Resource.Id.preloader);           

            btn_back_a.Click += (s, e) =>
            {
                Finish();
            };*/
            box_name1.TextChanged += async (sender, e) =>
            {
                try
                {
                    //async call which fetch suggestin from webservices.
                    RegisterBoxModel register = new RegisterBoxModel
                    {
                        Name = box_name1.Text,
                    };

                    var myHttpClient = new HttpClient();

                    var uri = ("http://iot.tmc-centert.ru/api/container/getboxesbyname?name=" + box_name1.Text);


                    HttpResponseMessage response = await myHttpClient.GetAsync(uri);

                    AuthApiData<ListResponse<ContainerResponse>> o_data = new AuthApiData<ListResponse<ContainerResponse>>();

                    string s_result;
                    using (HttpContent responseContent = response.Content)
                    {
                        s_result = await responseContent.ReadAsStringAsync();
                    }

                    o_data = JsonConvert.DeserializeObject<AuthApiData<ListResponse<ContainerResponse>>>(s_result);
                    var o_boxes_data = o_data.ResponseData;
                    StaticBox.AddInfoObjects(o_boxes_data);

                    //запись в файл
                    using (FileStream fs = new FileStream(dir_path + "box_list.txt", FileMode.OpenOrCreate))
                    {

                        await System.Text.Json.JsonSerializer.SerializeAsync<ListResponse<ContainerResponse>>(fs, o_boxes_data);

                    }
                    //чтение данных с файла
                    using (FileStream fs = new FileStream(dir_path + "box_list.txt", FileMode.OpenOrCreate))
                    {
                        ListResponse<ContainerResponse> containers = await System.Text.Json.JsonSerializer.DeserializeAsync<ListResponse<ContainerResponse>>(fs);
                        var name = containers.Objects[0].Name;
                    }

                    var boxes = o_data.ResponseData.Objects.Select(b => new BoxNames
                    {
                        Name = b.Name
                    }).ToList();

                    int count = boxes.Count();
                    string[] names1 = new string[count];

                    for (int i = 0; i < boxes.Count(); i++)
                    {
                        names1[i] = boxes[i].Name;
                    }
                   
                    ArrayAdapter adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, names1);
                    box_name1.Adapter = adapter1;

                }
                catch
                {
                }
            };

            //box_name1.AfterTextChanged += (sender, e) =>
            //{
            //    if (!box_name1.IsPopupShowing && box_name1.Text.Length > 0)
            //    {
          
            //        box_name1.ShowDropDown();
            //    }
            //};

            //SetContentView(Resource.Layout.activity_container_selection);

            box_selection.Click += async delegate
            {
                preloader.Visibility = Android.Views.ViewStates.Visible;
                box_selection.Click += btn_Submit_Click;
                /*
                XmlSerializer serializer = new XmlSerializer(typeof(ListResponse<ContainerResponse>));
                //Сериализация
                using (FileStream fs = new FileStream(dir_path + "box_list.xml", FileMode.OpenOrCreate))
                    serializer.Serialize(fs,);
                /*
                ListResponse<ContainerResponse> boxess = new ListResponse<ContainerResponse>();
                using (FileStream fs = new FileStream(dir_path + "box_list.txt", FileMode.OpenOrCreate))
                {
                   await System.Text.Json.JsonSerializer.DeserializeAsync<ListResponse<ContainerResponse>>(fs);
                }
                */
                /*
               
                */



                ListResponse<ContainerResponse> boxess = new ListResponse<ContainerResponse>();
                var Res = ContainerSelection.DeserializeListResponse(dir_path + "box_list.txt");
                if (Res!=null)
                {
                    boxess = Res;
                }

                var name = box_selection.Text;
                var uri = "http://iot.tmc-centert.ru/api/container/getbox?id=" + box_selection.Id;
            };

            btn_box_registr.Click += async delegate
            {
                Intent ContainerRegisterActivity = new Intent(this, typeof(Auth.RegisterBoxActivity));
                StartActivity(ContainerRegisterActivity);
                this.Finish();
            };

            

      

        }
        //btn_Submit Click Event  
        void btn_Submit_Click(object sender, System.EventArgs e)
        {
            //Checking if autoComplete1.Text is not empty  
            if (box_selection.Text != "")
            {
                Toast.MakeText(this, "Name Entered =" + box_name1.Text, ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Please Enter Name!", ToastLength.Short).Show();
            }
        }
        
        public static bool SerializeListResponse(string PathToSettingssFile, ListResponse<ContainerResponse> ExportedData)
        {
            try
            {
                XmlWriter writer = new XmlTextWriter(PathToSettingssFile, System.Text.Encoding.UTF8);
                XmlSerializer serializer = new XmlSerializer(typeof(ListResponse<ContainerResponse>));
                serializer.Serialize(writer, ExportedData);
                writer.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static ListResponse<ContainerResponse> DeserializeListResponse(string PathToSettingssFile)
        {
            try
            {
                XmlReader reader = new XmlTextReader(PathToSettingssFile);
                XmlSerializer serializer = new XmlSerializer(typeof(ListResponse<ContainerResponse>));
                ListResponse<ContainerResponse> ExportedData = (ListResponse<ContainerResponse>)serializer.Deserialize(reader);
                reader.Close();
                return ExportedData;
            }
            catch
            {
                return null;
            }
        }

    }

}