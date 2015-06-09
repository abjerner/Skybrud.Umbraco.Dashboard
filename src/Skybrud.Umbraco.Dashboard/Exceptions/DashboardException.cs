using System;
using System.Net;

namespace Skybrud.Umbraco.Dashboard.Exceptions {

    /// <summary>
    /// In general exceptions shouldn't be used to the end-user, so the purpose of this class is to
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

        public DashboardException(string message) : base(message) {
            Code = HttpStatusCode.InternalServerError;
        }

        public DashboardException(string title, string message) : base(message) {
            Code = HttpStatusCode.InternalServerError;
            Title = title;
        }

        public DashboardException(HttpStatusCode code, string message) : base(message) {
            Code = code;
        }

        public DashboardException(HttpStatusCode code, string title, string message) : base(message) {
            Code = code;
            Title = title;
        }

        #endregion

    }

}