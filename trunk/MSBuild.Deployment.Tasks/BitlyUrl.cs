using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Net;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using MSBuild.Deployment.Tasks.Services;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// Takes a url and uses the bit.ly service to 'shorten' the url
	/// </summary>
	public class BitlyUrl : Task, IProxyTask, ITimeoutTask, ITreatErrorsAsWarningsTask {
		/// <summary>
		/// Initializes a new instance of the <see cref="BitlyUrl"/> class.
		/// </summary>
		public BitlyUrl ( ) {
			Timeout = 10;
			TreatErrorsAsWarnings = false;
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[Required]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the API key.
		/// </summary>
		/// <value>The API key.</value>
		[Required]
		public string ApiKey { get; set; }
		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		[Required]
		public string Url { get; set; }
		/// <summary>
		/// Gets or sets the output URL.
		/// </summary>
		/// <value>The output URL.</value>
		[Output]
		public string OutputUrl { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			try {
				if ( string.IsNullOrEmpty ( Username ) ) {
					throw new ArgumentException ( "Username cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( ApiKey ) ) {
					throw new ArgumentException ( "ApiKey cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( Url ) ) {
					throw new ArgumentException ( "Url cannot be null or empty" );
				}

				BitlyService service = new BitlyService ( );
				service.Timeout = Timeout;
				if ( !string.IsNullOrEmpty ( ProxyHost ) ) {
					service.ProxyHost = ProxyHost;
					service.ProxyPassword = ProxyPassword;
					service.ProxyPort = ProxyPort;
					service.ProxyUser = ProxyUser;
				}

				BitlyResult result = service.Shorten ( Username, ApiKey, Url );
				if ( result.ErrorCode != 0 ) {
					throw new System.Net.WebException ( result.ErrorMessage );
				}
				if ( result.Results.Count > 0 ) {
					this.OutputUrl = result.Results[ 0 ].ShortUrl;
				} else {
					throw new WebException ( "Unable to shorten url." );
				}
				return true;
			} catch ( Exception ex ) {
				if ( TreatErrorsAsWarnings ) {
					Log.LogWarningFromException ( ex );
				} else {
					Log.LogErrorFromException ( ex );
				}
				return TreatErrorsAsWarnings;
			}
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

		#region ITreatErrorsAsWarningsTask Members

		/// <summary>
		/// Gets or sets a value indicating whether to treat errors as warnings.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if treat errors as warnings; otherwise, <c>false</c>.
		/// </value>
		public bool TreatErrorsAsWarnings { get; set; }

		#endregion
	}
}
