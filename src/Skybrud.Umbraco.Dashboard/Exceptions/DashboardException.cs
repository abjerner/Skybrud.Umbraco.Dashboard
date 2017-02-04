using System;
using System.Globalization;
using System.Net;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Dashboard.Constants;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Dashboard.Exceptions {

    /// <summary>
    /// In general exceptions shouldn't be sent to the end-user, so the purpose of this class is to
    /// be able to trigger exceptions that can be shown to the user.
    /// </summary>
    public class DashboardException : Exception {

        private readonly string _message;

        #region Properties

        /// <summary>
        /// Gets the status code of the exception.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the error code of the exception.
        /// </summary>
        public string ErrorCode { get; private set; }

        /// <summary>
        /// Gets the title of the error message.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the message of the exception.
        /// </summary>
        public override string Message {
            get { return _message; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new exception based on the specified <code>message</code>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public DashboardException(string message) {
            StatusCode = HttpStatusCode.InternalServerError;
            ErrorCode = StringUtils.ToUnderscore(DashboardError.UnknownServerError);
            _message = message;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="message">The message of the exception.</param>
        public DashboardException(string title, string message) {
            StatusCode = HttpStatusCode.InternalServerError;
            ErrorCode = StringUtils.ToUnderscore(DashboardError.UnknownServerError);
            Title = title;
            _message = message;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        public DashboardException(HttpStatusCode code, string message) {
            StatusCode = code;
            ErrorCode = StringUtils.ToUnderscore(DashboardError.UnknownServerError);
            _message = message;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        public DashboardException(HttpStatusCode code, string title, string message) {
            StatusCode = code;
            ErrorCode = StringUtils.ToUnderscore(DashboardError.UnknownServerError);
            Title = title;
            _message = message;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <paramref name="error"/>.
        /// </summary>
        /// <param name="error">An instance with information about the error.</param>
        public DashboardException(DashboardError error) : this(HttpStatusCode.InternalServerError, error) { }

        /// <summary>
        /// Initializes a new exception based on the specified <paramref name="statusCode"/> and <paramref name="error"/>.
        /// </summary>
        /// <param name="statusCode">The status code of the error.</param>
        /// <param name="error">An instance with information about the error.</param>
        public DashboardException(HttpStatusCode statusCode, DashboardError error) {

            StatusCode = statusCode;
            ErrorCode = StringUtils.ToUnderscore(error);

            // Get a reference to the text service (for translations)
            ILocalizedTextService service = ApplicationContext.Current.Services.TextService;

            // Calculate the translation key
            string translationKey = "dashboard/" + StringUtils.ToCamelCase(error.ToString());

            Title = service.Localize(translationKey + "Title", CultureInfo.CurrentCulture);
            _message = service.Localize(translationKey + "Message", CultureInfo.CurrentCulture);

        }

        #endregion

    }

}