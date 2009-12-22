using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Net;
using System.Reflection;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// Uploads a file to the google code project site.
	/// </summary>
	public class GoogleCodeUpload : Task, IProxyTask, ITimeoutTask {

		/// <summary>
		/// A Boundry Value
		/// </summary>
		private static readonly string BOUNDARY = Guid.NewGuid ( ).ToString ( );
		/// <summary>
		/// 
		/// </summary>
		private static readonly byte[] NEWLINE = Encoding.ASCII.GetBytes ( "\r\n" );

		/// <summary>
		/// Base Url
		/// </summary>
		private const string BASEURL = "https://{0}.googlecode.com/files";
		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleCodeUpload"/> class.
		/// </summary>
		public GoogleCodeUpload ( ) {
			ProxyPort = 8080;
			Timeout = 5 * 60;
		}

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		[Required]
		public string Username { get; set; }
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[Required]
		public string Password { get; set; }
		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		[Required]
		public string Project { get; set; }
		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		[Required]
		public string File { get; set; }
		/// <summary>
		/// Gets or sets the target file.
		/// </summary>
		/// <value>The target file.</value>
		public string TargetFile { get; set; }
		/// <summary>
		/// Gets or sets the summary.
		/// </summary>
		/// <value>The summary.</value>
		[Required]
		public string Summary { get; set; }
		/// <summary>
		/// Gets or sets the labels.
		/// </summary>
		/// <value>The labels.</value>
		public string[] Labels { get; set; }

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


		#region ITimeoutTask Members

		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		public int Timeout { get; set; }

		#endregion

		/// <summary>
		/// Gets or sets the file URL.
		/// </summary>
		/// <value>The file URL.</value>
		[Output]
		public string FileUrl { get; private set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			try {
				if ( string.IsNullOrEmpty ( Username ) ) {
					throw new ArgumentException ( "Username cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( Password ) ) {
					throw new ArgumentException ( "Password cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( Project ) ) {
					throw new ArgumentException ( "Project cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( File ) ) {
					throw new ArgumentException ( "File cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( TargetFile ) ) {
					TargetFile = Path.GetFileName ( File );
				}

				if ( string.IsNullOrEmpty ( Summary ) ) {
					throw new ArgumentException ( "Summary cannot be null or empty" );
				}

				if ( Username.EndsWith ( "@gmail.com" ) ) {
					Username = Username.Substring ( 0, Username.IndexOf ( "@gmail.com" ) );
				}

				UploadFile ( );

				Log.LogMessage ( "Upload Task completed successfully" );
				return true;
			} catch ( Exception ex ) {
				Log.LogError ( ex.ToString ( ) );
				return false;
			}
		}

		/// <summary>
		/// Uploads the contents of the file to the project's Google Code upload url.
		/// Performs the basic http authentication required by Google Code.
		/// </summary>
		private void UploadFile ( ) {
			HttpWebRequest req = HttpWebRequest.Create ( String.Format ( BASEURL, Project ) ) as HttpWebRequest;
			req.Method = "POST";
			req.ContentType = String.Concat ( "multipart/form-data; boundary=" + BOUNDARY );
			req.UserAgent = this.GetUserAgent ( );
			req.Headers.Add ( "Authorization", String.Format ( "Basic {0}", CreateAuthorizationToken ( Username, Password ) ) );
			req.Timeout = Timeout * 1000;

			if ( !string.IsNullOrEmpty ( ProxyHost ) ) {
				WebProxy proxy = new WebProxy ( ProxyHost, ProxyPort );
				if ( !string.IsNullOrEmpty ( ProxyUser ) ) {
					NetworkCredential proxyCred = new NetworkCredential ( ProxyUser, ProxyPassword );
					proxy.Credentials = proxyCred;
				}
				req.Proxy = proxy;
			}

			Log.LogMessage ( req.UserAgent );

			Log.LogMessage ( "Upload URL: {0}", req.Address.ToString ( ) );
			Log.LogMessage ( "Username: {0}", Username );
			Log.LogMessage ( "File to send: {0}", File );
			Log.LogMessage ( "Target file: {0}", TargetFile );
			Log.LogMessage ( "Summary: {0}", Summary );
			Log.LogMessage ( "Labels: {0}", string.Join ( ",", Labels ) );

			using ( Stream strm = req.GetRequestStream ( ) ) {
				WriteLine ( strm, String.Concat ( "--", BOUNDARY ) );
				WriteLine ( strm, @"content-disposition: form-data; name=""summary""" );
				WriteLine ( strm, "" );
				WriteLine ( strm, Summary );

				foreach ( string label in Labels ) {
					WriteLine ( strm, String.Concat ( "--", BOUNDARY ) );
					WriteLine ( strm, @"content-disposition: form-data; name=""label""" );
					WriteLine ( strm, "" );
					WriteLine ( strm, label );
				}


				WriteLine ( strm, String.Concat ( "--", BOUNDARY ) );
				WriteLine ( strm, String.Format ( @"content-disposition: form-data; name=""filename""; filename=""{0}""", TargetFile ) );
				WriteLine ( strm, string.Format ( "Content-Type: {0}", MimeType.Create ( new FileInfo ( File ) ).ContentType ) );
				WriteLine ( strm, "" );
				WriteFile ( strm, File );
				WriteLine ( strm, "" );
				WriteLine ( strm, String.Concat ( "--", BOUNDARY, "--" ) );
			}

			req.GetResponse ( );
			FileUrl = string.Concat ( string.Format ( BASEURL, Project ), "/", TargetFile );
			Log.LogMessage ( "Download url: {0}", FileUrl );
		}

		/// <summary>
		/// Writes the specified file to the specified stream.
		/// </summary>
		/// <param name="outStream">The out stream.</param>
		/// <param name="file">The file.</param>
		internal void WriteFile ( Stream outStream, string file ) {
			if ( outStream == null ) {
				throw new ArgumentNullException ( "outStream" );
			}

			if ( file == null ) {
				throw new ArgumentNullException ( "file" );
			}

			using ( FileStream fileStream = new FileStream ( File, FileMode.Open ) ) {
				byte[] buffer = new byte[ 1024 ];
				int count = 0;
				while ( ( count = fileStream.Read ( buffer, 0, buffer.Length ) ) > 0 ) {
					outStream.Write ( buffer, 0, count );
				}
			}
		}

		/// <summary>
		/// Writes the string to the specified stream.
		/// </summary>
		/// <param name="outputStream">The output stream.</param>
		/// <param name="value">The value.</param>
		internal static void WriteLine ( Stream outputStream, string value ) {
			if ( value == null ) {
				throw new ArgumentNullException ( "value" );
			}

			List<byte> buffer = new List<byte> ( Encoding.ASCII.GetBytes ( value ) );
			buffer.AddRange ( NEWLINE );
			outputStream.Write ( buffer.ToArray ( ), 0, buffer.Count );
		}

		/// <summary>
		/// Creates the authorization token.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		internal static string CreateAuthorizationToken ( string username, string password ) {
			if ( string.IsNullOrEmpty ( username ) ) {
				throw new ArgumentException ( "username" );
			}

			if ( string.IsNullOrEmpty ( password ) ) {
				throw new ArgumentException ( "password" );
			}

			return Convert.ToBase64String ( Encoding.ASCII.GetBytes ( String.Format ( "{0}:{1}", username, password ) ) );
		}



		#region IUserAgentTask Members

		public string UserAgent {
			get { throw new NotImplementedException ( ); }
		}

		#endregion
	}
}
