using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using MSBuild.Deployment.Tasks.Services;
//using System.Web.Script.Serialization;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// Sends a status update to Twitter
	/// </summary>
	public class Tweet : Task, IProxyTask, ITreatErrorsAsWarningsTask, ITimeoutTask {

		/// <summary>
		/// Initializes a new instance of the <see cref="Tweet"/> class.
		/// </summary>
		public Tweet ( ) {
			Timeout = 5;
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }
		/// <summary>
		/// Gets or sets the message.
		/// </summary>
		/// <value>The message.</value>
		public string Message { get; set; }

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

		#region ITreatErrorsAsWarningsTask Members

		/// <summary>
		/// Gets or sets a value indicating whether to treat errors as warnings.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if treat errors as warnings; otherwise, <c>false</c>.
		/// </value>
		public bool TreatErrorsAsWarnings { get; set; }

		#endregion

		#region ITimeoutTask Members

		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		public int Timeout { get; set; }

		#endregion

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			try {
				Twitter serivce = new Twitter ( );
				serivce.TwitterClient = this.GetUserAgent ( );
				serivce.TwitterClientUrl = "http://mdt.codeplex.com/";
				serivce.TwitterClientVersion = this.GetType ( ).Assembly.GetName ( ).Version.ToString ( );

				string msg = Message;
				if ( msg.Length > 140 ) {
					msg = string.Concat ( msg.Substring ( 0, 139 ), "…" );
				}

				string json = serivce.Update ( Username, Password, msg, Twitter.OutputFormatType.JSON );
				//JavaScriptSerializer jsser = new JavaScriptSerializer ( );
				//var result = jsser.Deserialize<object> ( json );
				Log.LogMessage ( json );
				return true;
			} catch ( Exception ex ) {
				if ( TreatErrorsAsWarnings ) {
					Log.LogWarningFromException ( ex, true );
				} else {
					Log.LogErrorFromException ( ex, true );
				}
				return TreatErrorsAsWarnings;
			}
		}
	}
}
