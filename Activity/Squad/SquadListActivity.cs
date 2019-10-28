using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using GeoGeometry.Activity.Menu;
using GeoGeometry.Model.Squad;
using Newtonsoft.Json;

namespace GeoGeometry.Activity.Squad
{
    [Activity(Label = "SquadListActivity")]
    public class SquadListActivity : AppCompatActivity
    {
        /// <summary>
        /// Контейнер для созданных команд.
        /// </summary>
        private LinearLayout containerListSquad;
            RelativeLayout itemSquad;

        TextView textName, textLeader;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StaticMenu.id_page = 2;

            SetContentView(Resource.Layout.activity_squad_list);

            containerListSquad = FindViewById<LinearLayout>(Resource.Id.containerListSquad);

            var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/getteams/");

            SquadListResponseData o_data_squad = new SquadListResponseData();
            o_data_squad = await GetListSquad(uri);

            int index = 0;
            int i_color = 0;
            foreach (SquadListTeams a_item in o_data_squad.Teams)
            {
                var textViewParams = new RelativeLayout.LayoutParams(
                    ViewGroup.LayoutParams.WrapContent,
                    ViewGroup.LayoutParams.WrapContent);

                index++;
                i_color++;

                itemSquad = new RelativeLayout(this);
                itemSquad.SetPadding(5, 5, 5, 5);
                itemSquad.Id = 100+index;
                itemSquad.Tag = a_item.TeamId;

                if (i_color % 2 == 0)
                    itemSquad.SetBackgroundColor(Color.White);

                textName = new TextView(this);
                textName.Id = 1;
                textName.Text = a_item.Name;
                textName.SetTextSize(ComplexUnitType.Dip, 20);
                textName.SetTextColor(Color.Black);
                itemSquad.AddView(textName);

                //index++;
                textLeader = new TextView(this);
                textLeader.Id = 2;
                textLeader.Text = a_item.Leader;
                textLeader.SetTextSize(ComplexUnitType.Dip, 15);
                textViewParams.AddRule(LayoutRules.Below, textName.Id);
                itemSquad.AddView(textLeader, textViewParams);

                containerListSquad.AddView(itemSquad);

                itemSquad.Click += (sender, e) =>
                {
                    RelativeLayout o_click = (RelativeLayout)sender;

                    Intent squadActivity = new Intent(this, typeof(Squad.SquadActivity));
                    squadActivity.PutExtra("idPage", "3");
                    squadActivity.PutExtra("teamId", "" + o_click.Tag);
                    StartActivity(squadActivity);
                };
            }
        }

        /// <summary>
        /// Метод собирает информацию об отрядах.
        /// </summary>
        /// <param name="uri"></param>
        private async Task<SquadListResponseData> GetListSquad(Uri uri)
        {
            var myHttpClient = new HttpClient();

            HttpResponseMessage response = await myHttpClient.GetAsync(uri);

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            SquadListApiData o_data = JsonConvert.DeserializeObject<SquadListApiData>(s_result);

            return o_data.ResponseData;
        }
    }
}