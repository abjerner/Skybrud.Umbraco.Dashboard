using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using Skybrud.Essentials.Strings;
using Skybrud.Umbraco.Dashboard.Constants;
using Skybrud.Umbraco.Dashboard.Exceptions;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace Skybrud.Umbraco.Dashboard.Extensions.HttpRequestMessage {

    /// <summary>
    /// Various extension methods for the <see cref="HttpRequestMessage"/> class.
    /// </summary>
    public static class HttpRequestMessageExtensions {

        /// <summary>
        /// Creates a new response based on the specified <code>error</code>.
        /// </summary>
        /// <param name="request">A reference to the <see cref="HttpRequestMessage"/>.</param>
        /// <param name="error">The error.</param>
        /// <returns>Returns an instance of <see cref="HttpResponseMessage"/> representing the response.</returns>
        public static HttpResponseMessage CreateResponse(this System.Net.Http.HttpRequestMessage request, DashboardError error) {
            return CreateResponse(request, HttpStatusCode.InternalServerError, error);
        }

        /// <summary>
        /// Creates a new response based on the specified <code>statusCode</code>, <code>error</code> and
        /// <code>errorMessage</code>.
        /// </summary>
        /// <param name="request">A reference to the <see cref="HttpRequestMessage"/>.</param>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> code to be used for the response.</param>
        /// <param name="error">The <see cref="DashboardError"/>.</param>
        /// <returns>Returns an instance of <see cref="HttpResponseMessage"/> representing the response.</returns>
        public static HttpResponseMessage CreateResponse(this System.Net.Http.HttpRequestMessage request, HttpStatusCode statusCode, DashboardError error) {
            
            Newtonsoft.Json.Linq.JObject meta = new Newtonsoft.Json.Linq.JObject {
                {"code", (int) statusCode},
                {"error", StringUtils.ToUnderscore(error.ToString())}
            };

            // Get a reference to the text service (for translations)
            ILocalizedTextService service = ApplicationContext.Current.Services.TextService;

            // Calculate the translation key
            string translationKey = "dashboard/" + StringUtils.ToCamelCase(error.ToString());

            string title = service.Localize(translationKey + "Title", CultureInfo.CurrentCulture);
            string message = service.Localize(translationKey + "Message", CultureInfo.CurrentCulture);
            if (!String.IsNullOrWhiteSpace(title) && !title.StartsWith("[")) meta.Add("title", title);
            if (!String.IsNullOrWhiteSpace(message) && !message.StartsWith("[")) meta.Add("message", message);

            return request.CreateResponse(statusCode, new {
                meta
            });
        
        }

        public static HttpResponseMessage CreateResponse(this System.Net.Http.HttpRequestMessage request, DashboardException exception) {

            Newtonsoft.Json.Linq.JObject meta = new Newtonsoft.Json.Linq.JObject {
                {"code", (int) exception.StatusCode},
                {"error", exception.ErrorCode}
            };

            if (!String.IsNullOrWhiteSpace(exception.Title) && !exception.Title.StartsWith("[")) meta.Add("title", exception.Title);
            if (!String.IsNullOrWhiteSpace(exception.Message) && !exception.Message.StartsWith("[")) meta.Add("message", exception.Message);

            return request.CreateResponse(exception.StatusCode, new {
                meta
            });

        }

        public static HttpResponseMessage CreateResponse(this System.Net.Http.HttpRequestMessage request, Exception exception) {

            Newtonsoft.Json.Linq.JObject meta = new Newtonsoft.Json.Linq.JObject {
                {"code", 500},
                {"error", "unknown_error"}
            };

            meta.Add("title", "Unknown error");
            if (!String.IsNullOrWhiteSpace(exception.Message)) meta.Add("message", exception.Message);

            return request.CreateResponse(HttpStatusCode.InternalServerError, new {
                meta
            });

        }

    }

}