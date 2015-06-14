using System;
using System.Net;

namespace Skybrud.Umbraco.Dashboard.Exceptions {

    /// <summary>
    /// In general exceptions shouldn't be sent to the end-user, so the purpose of this class is to
    /// be able to trigger exceptions that can be shown to the user.
    /// </summary>
    public class DashboardException : Exception {

        #region Properties

        /// <summary>
        /// Gets the status code (type) of the exception. 
        /// </summary>
        public HttpStatusCode Code { get; private set; }

        /// <summary>
        /// Gets the title of the error message.
        /// </summary>
        public string Title { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new exception based on the specified <code>message</code>.
        /// </summary>
        /// <param name="message">The message of the exception.</param>
        public DashboardException(string message) : base(message) {
            Code = HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        /// <param name="title">The title of the exception.</param>
        /// <param name="message">The message of the exception.</param>
        public DashboardException(string title, string message) : base(message) {
            Code = HttpStatusCode.InternalServerError;
            Title = title;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        public DashboardException(HttpStatusCode code, string message) : base(message) {
            Code = code;
        }

        /// <summary>
        /// Initializes a new exception based on the specified <code>title</code> and <code>message</code>.
        /// </summary>
        public DashboardException(HttpStatusCode code, string title, string message) : base(message) {
            Code = code;
            Title = title;
        }

        #endregion

    }

}