using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GeoGeometry.Model;
using GeoGeometry.Model.User;

namespace GeoGeometry.Container
{
    class StaticBox
    {
        /// <summary>
        /// Id контейнера.
        /// </summary>
        public static string SmartBoxId { get; set; }

        /// <summary>
        /// Разложен ли контейнер.
        /// </summary>
        public static bool IsOpenedBox { get; set; }

        /// <summary>
        /// Открыта ли дверь.
        /// </summary>
        public static bool IsOpenedDoor { get; set; }

        /// <summary>
        /// Освещённость.
        /// </summary>
        public static int Light { get; set; }

        /// <summary>
        /// Пин-код для открытия двери.
        /// </summary>
        public static string Code { get; set; }

        /// <summary>
        /// Температура.
        /// </summary>
        public static double Temperature { get; set; }

        /// <summary>
        /// Влажность.
        /// </summary>
        public static double Wetness { get; set; }

        /// <summary>
        /// Заряд батареи.
        /// </summary>
        public static double BatteryPower { get; set; }

        /// <summary>
        /// Наименование контейнера.
        /// </summary>
        public static string Name { get; set; }

        /// <summary>
        /// Вес контейнера.
        /// </summary>
        public static double Weight { get; set; }

        public static List<ContainerResponse> Objects { get; set; }

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект авторизации/регистрации</param>
        public static void AddInfoAuth(ContainerResponse o_auth)
        {
            SmartBoxId = o_auth.SmartBoxId;
            Name = o_auth.Name;
        }

        public static void AddInfoObjects(ListResponse<ContainerResponse> boxes)
        {
            Objects = new List<ContainerResponse>();
            Objects = boxes.Objects;
        }


        /*

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект настройки</param>
        public static void AddInfoUserSettings(UserSettingsResponseData o_user_settings)
        {
            PhoneNumber = o_user_settings.PhoneNumber;
            Email = o_user_settings.Email;
            IsEmailConfirmed = o_user_settings.IsEmailConfirmed;
            IsLeader = o_user_settings.IsLeader;
        }

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект клиент</param>
        public static void AddInfoUser(UserResponseData o_user)
        {
            MiddleName = o_user.MiddleName;
            Team = o_user.Team;
            TeamId = o_user.TeamId;
            Section = o_user.Section;
            Rang = o_user.Rang;
            Imagines = o_user.Imagines;
        }
        */
    }
}