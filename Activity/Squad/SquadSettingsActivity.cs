using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace GeoGeometry.Activity.Squad
{
    [Activity(Label = "SquadSettingsActivity")]
    public class SquadSettingsActivity : AppCompatActivity
    {
        /// <summary>
        /// Кнопка загрузить файл.
        /// </summary>
        private Button btn_upload_img;

        /// <summary>
        /// Кнопка сохранить файл.
        /// </summary>
        private Button btn_save_logo;

        //Pick Id to know the pick item no.
        public static readonly int PickImageId = 1000;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_squad_settings);

            Button btn_upload_img = FindViewById<Button>(Resource.Id.btn_upload_img);
            btn_upload_img.Click += BrowseButtonOnClick;
        }

        void BrowseButtonOnClick(object sender, EventArgs eventArs)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent data = Intent.SetAction(Intent.ActionGetContent);

            try
            {
                StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
            }
            catch (ActivityNotFoundException ex)
            {
                Toast.MakeText(this, "Please install a File Manager.", ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Error occured ", ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
            {
                UploadFile(data);

            }
        }

        private void UploadFile(Intent data)
        {
            throw new NotImplementedException();
        }
    }
}