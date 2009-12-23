using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace MSBuild.Deployment.Tasks.Services {
	/// <summary>
	/// 
	/// </summary>
	public class BitlyService : IProxyTask, ITimeoutTask {

		/// <summary>
		/// Shortens the specified url.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="apiKey">The API key.</param>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public BitlyResult Shorten ( string username, string apiKey, string url ) {
			try {
				BitlyResult result = null;
				HttpWebRequest req = HttpWebRequest.Create ( BuildBitlyRequestUrl ( username, apiKey, url ) ) as HttpWebRequest;
				req.Timeout = Timeout * 1000;
				req.UserAgent = this.GetUserAgent ( );
				if ( !string.IsNullOrEmpty ( ProxyHost ) ) {
					WebProxy proxy = new WebProxy ( ProxyHost, ProxyPort );
					if ( !string.IsNullOrEmpty ( ProxyUser ) ) {
						NetworkCredential proxyCred = new NetworkCredential ( ProxyUser, ProxyPassword );
						proxy.Credentials = proxyCred;
					}
					req.Proxy = proxy;
				}

				using ( HttpWebResponse resp = req.GetResponse ( ) as HttpWebResponse ) {
					using ( Stream respStream = resp.GetResponseStream ( ) ) {
						XmlSerializer ser = new XmlSerializer ( typeof ( BitlyResult ) );
						result = ser.Deserialize ( respStream ) as BitlyResult;
					}
				}

				return result;
			} catch ( Exception ) {
				throw;
			}
		}

		/// <summary>
		/// Builds the bitly request URL.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="apikey">The apikey.</param>
		/// <param name="longUrl">The long URL.</param>
		/// <returns></returns>
		private string BuildBitlyRequestUrl ( string username, string apikey, string longUrl ) {

			StringBuilder url = new StringBuilder ( );
			url.Append ( "http://api.bit.ly/shorten?login=" );
			url.Append ( username );
			url.Append ( "&apiKey=" );
			url.Append ( apikey );
			url.Append ( "&format=xml&version=2.0.1&longUrl=" );
			url.Append ( longUrl );
			return url.ToString ( );

		}

		#region IProxyTask Members

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

		#endregion

		#region ITimeoutTask Members

		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		public int Timeout { get; set; }
		#endregion
	}
}
