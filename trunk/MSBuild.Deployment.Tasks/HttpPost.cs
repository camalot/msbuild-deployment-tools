using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Net;
using MSBuild.Deployment.Tasks.Helpers;

namespace MSBuild.Deployment.Tasks {
	public class HttpPost : Task, ITreatErrorsAsWarningsTask, IProxyTask, ITimeoutTask {
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonHttpPost"/> class.
		/// </summary>
		public HttpPost ( ) {
			ProxyPort = 8080;
			Timeout = 60;
			TreatErrorsAsWarnings = false;
			ContentType = "application/x-www-form-urlencoded";
		}

		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>
		/// The URL.
		/// </value>
		[Required]
		public String Url { get; set; }
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		[Required]
		public String Content { get; set; }

		public String ContentType { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			try {
				var req = HttpWebRequest.Create ( new Uri ( Url ) ) as HttpWebRequest;
				req.ContentType = this.ContentType;
				req.Method = "POST";

				if ( !string.IsNullOrEmpty ( ProxyHost ) ) {
					WebProxy proxy = new WebProxy ( ProxyHost, ProxyPort );
					if ( !string.IsNullOrEmpty ( ProxyUser ) ) {
						NetworkCredential proxyCred = new NetworkCredential ( ProxyUser, ProxyPassword );
						proxy.Credentials = proxyCred;
					}
					req.Proxy = proxy;
				}

				var bytes = Content.ToByteArray ( );
				req.ContentLength = bytes.Length;
				req.Timeout = Timeout * 1000;
				using ( var rs = req.GetRequestStream ( ) ) {
					rs.Write ( bytes, 0, bytes.Length );
				}

				using ( var resp = req.GetResponse ( ) as HttpWebResponse ) {
					if ( resp.StatusCode != HttpStatusCode.OK ) {
						throw new Exception ( String.Format ( "POST Failed (Error Code: {1}): {0}", resp.StatusDescription, resp.StatusCode ) );
					}
				}

				return true;
			} catch ( Exception ex ) {
				if ( TreatErrorsAsWarnings ) {
					Log.LogWarningFromException ( ex, true );
					return true;
				} else {
					Log.LogErrorFromException ( ex, true );
					return false;
				}
			}

		}

		/// <summary>
		/// Gets or sets a value indicating whether to treat errors as warnings.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if treat errors as warnings; otherwise, <c>false</c>.
		/// </value>
		public bool TreatErrorsAsWarnings { get; set; }

		/// <summary>
		/// Gets or sets the proxy host.
		/// </summary>
		/// <value>The proxy host.</value>
		public string ProxyHost { get; set; }
		/// <summary>
		/// Gets or sets the proxy port.
		/// </summary>
		/// <value>The proxy port.</value>
		public int ProxyPort { get; set; }
		/// <summary>
		/// Gets or sets the proxy user.
		/// </summary>
		/// <value>The proxy user.</value>
		public string ProxyUser { get; set; }
		/// <summary>
		/// Gets or sets the proxy password.
		/// </summary>
		/// <value>The proxy password.</value>
		public string ProxyPassword { get; set; }
		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		public int Timeout { get; set; }
	}
}


