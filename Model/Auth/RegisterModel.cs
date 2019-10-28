namespace GeoGeometry.Model.Auth {
	/// <summary>
	/// Модель регистрации.
	/// </summary>
	public class RegisterModel
    {
        /// <summary>
        /// Email клиента.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя клиента.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия клиента.
        /// </summary>        
        public string LastName { get; set; }

        /// <summary>
        /// Пароль клиента.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Подтвержденный пароль.
        /// </summary>
        public string PasswordConfirm { get; set; }
    }
}