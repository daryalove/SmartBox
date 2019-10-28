using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Android.Content;
using Com.Karumi.Dexter;
using Android;
using Com.Karumi.Dexter.Listener.Single;
using Com.Karumi.Dexter.Listener;

namespace GeoGeometry.Activity
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : AppCompatActivity
    {
        RelativeLayout main_form;

        /// <summary>
        /// Конпка прехода на форму авторизации.
        /// </summary>
        private Button btn_auth_form;

        /// <summary>
        /// Конпка прехода на форму регистрации.
        /// </summary>
        private Button btn_reg_form;

        /// <summary>
        /// Кнопка на регстрацию контейнера.
        /// </summary>
        private Button btn_box_form;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            btn_auth_form = FindViewById<Button>(Resource.Id.btn_auth_form);
            btn_reg_form = FindViewById<Button>(Resource.Id.btn_reg_form);
            btn_box_form = FindViewById<Button>(Resource.Id.btn_box_form);

            Dexter.WithActivity(this)
                .WithPermission(Manifest.Permission.AccessFineLocation)
                .WithListener(new CompositePermissionListener(new SamplePermissionListener(this))).Check();

            // Переход к форме регистрации.
            btn_reg_form.Click += (s, e) =>
            {
                Intent registerActivity = new Intent(this, typeof(Auth.RegisterActivity));
                StartActivity(registerActivity);
            };

            // Переход к форме авторизация.
            btn_auth_form.Click += (s, e) =>
            {
                Intent authActivity = new Intent(this, typeof(Auth.AuthActivity));
                StartActivity(authActivity);
            };

            // Переход к регистрации контейнера.
            btn_box_form.Click += (s, e) =>
            {
                Intent registerBoxActivity = new Intent(this, typeof(Auth.RegisterActivity));
                StartActivity(registerBoxActivity);
            };
        }

        private class SamplePermissionListener : Java.Lang.Object, IPermissionListener
        {
            MainActivity activity;
            public SamplePermissionListener(MainActivity activity)
            {
                this.activity = activity;
            }

            public void OnPermissionDenied(PermissionDeniedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Denied", Snackbar.LengthShort).Show();
            }

            public void OnPermissionGranted(PermissionGrantedResponse p0)
            {
                //Snackbar.Make(activity.main_form, "Permission Granted", Snackbar.LengthShort).Show();
            }

            public void OnPermissionRationaleShouldBeShown(PermissionRequest p0, IPermissionToken token)
            {
                activity.ShowRequestPermissionRationale(token);
            }
        }

        private void ShowRequestPermissionRationale(IPermissionToken token)
        {
            new Android.Support.V7.App.AlertDialog.Builder(this)
                .SetTitle("Разрешения GPS")
                .SetMessage("Необходимо разрешить использовать ваши gps данные")
                .SetNegativeButton("Отмена", delegate
                {
                    token.ContinuePermissionRequest();
                })
                .SetPositiveButton("Ok", delegate
                {
                    token.ContinuePermissionRequest();
                })
                .SetOnDismissListener(new MyDismissListener(token)).Show();
        }
    }

    internal class MyDismissListener : Java.Lang.Object, IDialogInterfaceOnDismissListener
    {
        private IPermissionToken token;

        public MyDismissListener(IPermissionToken token)
        {
            this.token = token;
        }

        public void OnDismiss(IDialogInterface dialog)
        {
            token.CancelPermissionRequest();
        }
    }
}