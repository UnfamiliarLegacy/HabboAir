namespace HabBridge.Api.V1.Responses
{
    public class LoginErrorCode
    {
        /// <summary>
        ///     Display a message that the given combination was invalid.
        /// </summary>
        public const string InvalidPassword = "login.invalid_password";

        /// <summary>
        ///     Related to showing the user a captcha or saying that it was wrong,
        ///     depending on the <see cref="LoginErrorResponse.Captcha"/> property.
        /// </summary>
        public const string InvalidCaptcha = "invalid-captcha";
    }
}
