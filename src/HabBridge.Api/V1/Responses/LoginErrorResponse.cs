namespace HabBridge.Api.V1.Responses
{
    public class LoginErrorResponse
    {
        /// <summary>
        ///     The key of the message to show.
        ///     Available:
        ///         - login.invalid_password
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Whether to show a captcha or not.
        /// </summary>
        public bool Captcha { get; set; }
    }
}
