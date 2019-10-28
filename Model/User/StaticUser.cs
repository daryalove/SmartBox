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
using GeoGeometry.Model.Auth;

namespace GeoGeometry.Model.User
{
    class StaticUser
    {
        /// <summary>
        /// Код клиента.
        /// </summary>
        public static string UserId { get; set; }

        /// <summary>
        /// Email клиента.
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public static string FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>
        public static string LastName { get; set; }

        /// <summary>
        /// Отчество клиента.
        /// </summary>
        public static string MiddleName { get; set; }

        /// <summary>
        /// Отряд клиента.
        /// </summary>
        public static string Team { get; set; }

        /// <summary>
        /// ID отряда клиента.
        /// </summary>
        public static string TeamId { get; set; }

        /// <summary>
        /// Направление отряда.
        /// </summary>
        public static string Section { get; set; }

        /// <summary>
        /// Ранг клиента.
        /// </summary>
        public static string Rang { get; set; }

        /// <summary>
        /// Фото клиента.
        /// </summary>
        public static string Imagines { get; set; }

        /// <summary>
        /// Телефон клиента.
        /// </summary>
        public static string PhoneNumber { get; set; }

        /// <summary>
        /// Почта клиента.
        /// </summary>
        public static string Email { get; set; }

        /// <summary>
        /// Подтверждение почты.
        /// </summary>
        public static bool IsEmailConfirmed { get; set; }

        /// <summary>
        /// Является ли лидером клиент.
        /// </summary>
        public static bool IsLeader { get; set; }

        /// <summary>
        /// Долгота
        /// </summary>
        public static double Longitude { get; set; }

        /// <summary>
        /// Широта
        /// </summary>
        public static double Latitude { get; set; }

        /// <summary>
        /// Дистанция
        /// </summary>
        public static double Distance { get; set; }

        /// <summary>
        /// Добавляю информацию о клиенте
        /// </summary>
        /// <param name="o_auth">Объект авторизации/регистрации</param>
        public static void AddInfoAuth(AuthResponseData o_auth)
        {
            UserId = o_auth.UserId;
            UserName = o_auth.UserName;
            FirstName = o_auth.FirstName;
            LastName = o_auth.LastName;
            Longitude = o_auth.Longitude;
            Latitude = o_auth.Latitude;
            Distance = o_auth.Distance;
        }

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
    }
}