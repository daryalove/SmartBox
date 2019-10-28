using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using GeoGeometry.Activity.Menu;
using GeoGeometry.Model.Auth;
using GeoGeometry.Model.User;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Color = Android.Graphics.Color;
using View = Android.Views.View;
using Xamarin.Essentials;


namespace GeoGeometry.Activity.Camera
{
    [Activity(Label = " ")]
    public class CameraActivity : AppCompatActivity, TextureView.ISurfaceTextureListener
    {
        private Android.Hardware.Camera _camera;
        private TextureView _textureView;
        private SurfaceView _surfaceView;
        private ISurfaceHolder holder;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_camera);
            _textureView = (TextureView)FindViewById(Resource.Id.textureView);
            _textureView.SurfaceTextureListener = this;                                 // что тут происходит??

            _surfaceView = (SurfaceView)FindViewById(Resource.Id.surfaceview);
            //set to top layer
            _surfaceView.SetZOrderOnTop(true);
            //set the background to transparent
            _surfaceView.Holder.SetFormat(Android.Graphics.Format.Transparent);         // что тут происходит??
            holder = _surfaceView.Holder;  // что тут происходит??
            _surfaceView.Touch += _surfaceView_Touch;

            // Получаю клиентов в радиусе 500.
            getUsersLoc();

        }

        /// <summary>
        /// Метод получения клиентов в радиусе 500м.
        /// </summary>
        private async void getUsersLoc()
        {
            UsersBesideLocModel for_Send = new UsersBesideLocModel
            {
                UserId = StaticUser.UserId,
                Latitude = StaticUser.Latitude,
                Longitude = StaticUser.Longitude
            };

            var myHttpClient = new HttpClient();
            var uri = new Uri("http://geometry.tmc-centert.ru/api/serviceapi/getusersfromlocation");

            //json структура.
            var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"userId", for_Send.UserId },
                {"Latitude" , 1.ToString()},
                {"Longitude" , 2.ToString()}
            });
            // StaticUser.Latitude
            // StaticUser.Longitude
            HttpResponseMessage response = await myHttpClient.PostAsync(uri.ToString(), formContent);

            string s_result;
            using (HttpContent responseContent = response.Content)
            {
                s_result = await responseContent.ReadAsStringAsync();
            }

            UsersBesideLocApiData o_data = JsonConvert.DeserializeObject<UsersBesideLocApiData>(s_result);

            UsersBesideLocResponseData o_user_data = new UsersBesideLocResponseData();
            o_user_data = o_data.ResponseData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _surfaceView_Touch(object sender, View.TouchEventArgs e)
        {
            float x = e.Event.GetX();
            float y = e.Event.GetY();
            float RawX = e.Event.RawX;
            float RawY = e.Event.RawY;
            if(x>100&&x<500&&y>100&y<500)
            {
                Draw(x, y);
            }
            Android.Views.View touchedView = sender as View;                    // что тут происходит?? приводим sender к типу View??
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        public void Draw(float xpos, float ypos)
        {
            Canvas canvas = holder.LockCanvas();

            if (canvas != null)
            {
                canvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
                Paint paint = new Paint();
                paint.Color = Color.Blue;
                paint.SetStyle(Paint.Style.Stroke);
                paint.StrokeWidth = 4f;

                float radius = 50f;

                canvas.DrawCircle(xpos, ypos, radius, paint);
                
                

                holder.UnlockCanvasAndPost(canvas);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Users_Draw()
        {
            // определяем положение экрана
            SensorSpeed speed = SensorSpeed.Default;  // Normal
        }

        /// <summary>
        /// 
        /// </summary>
        public void OrientationSensorTest()
        {
            // Register for reading changes, be sure to unsubscribe when finished
            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            // Process Orientation quaternion (X, Y, Z, and W)
        }


        /// почему тело не пустое, как у двух следующих, если его не вызывают??7
        //public Task<bool> SendDrawRectageCoords(float x, float y, float RawX, float RawY, Android.Views.View touchedView, MotionEventActions EventAction)
        //{
        //    //try
        //    //{
        //    //    float XInit = 0.0f;
        //    //    float YInit = 0.0f;
        //    //    switch (EventAction)
        //    //    {
        //    //        case MotionEventActions.Down:
        //    //            XInit = touchedView.GetX() - RawX;
        //    //            YInit = touchedView.GetY() - RawY;
        //    //            break;
        //    //        case MotionEventActions.Move:
        //    //            break;
        //    //        default:
        //    //            break;
        //    //    }

        //    //    return true;
        //    //}
        //    //catch (Exception exp)
        //    //{
        //    //    Console.WriteLine("SendDrawRectageCoords - " + exp.Message);
        //        return false;
        //    //}
        //}  // end of  SendDrawRectageCoords


        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
        {
            _camera = Android.Hardware.Camera.Open();
            
            try
            {
                _camera.SetPreviewTexture(surface);
                _camera.SetDisplayOrientation(90);     // возможно тут решение проблемы искажения при переворачивании 
                _camera.StartPreview();
                
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine("OnSurfaceTextureAvailable - " + ex.Message);
            }
        }  // end of OnSurfaceTextureAvailable

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <returns></returns>
        public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
            {
                _camera.StopPreview();
                _camera.Release();

                return true;
            }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
        {
            // скорее всего здесь нужно будет что-то написать для того, чтобы изображение не искажалось при повороте экрана
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surface"></param>
        public void OnSurfaceTextureUpdated(SurfaceTexture surface)
        {
        }

        ///////////////////////////////////////////////////////////////   предоставление разрещения на использование камеры
        ////  https://stackoverflow.com/questions/51877812/how-to-give-permissions-to-xamarin-camera
        ///
        ////  https://devblogs.microsoft.com/xamarin/requesting-runtime-permissions-in-android-marshmallow/


        /// <summary>
        /// Метод из Microsoft Docs для всех Essentials 
        /// </summary>
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}