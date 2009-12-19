using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using Microsoft.Build.Framework;
using System.Net;
using System.Reflection;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// Uploads a file to a codeplex project
	/// </summary>
	public class CodePlexUpload : Task, IProxyTask {

		/// <summary>
		/// 
		/// </summary>
		public enum FileResourceType {
			/// <summary>
			/// 
			/// </summary>
			RuntimeBinary,
			/// <summary>
			/// 
			/// </summary>
			SourceCode,
			/// <summary>
			/// 
			/// </summary>
			Documentation,
			/// <summary>
			/// 
			/// </summary>
			Example
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodePlexUpload"/> class.
		/// </summary>
		public CodePlexUpload ( ) {
			//IsRecommended = false;
			ResourceType = FileResourceType.RuntimeBinary;
			FileType = ResourceType.ToString ( );
			Timeout = 10 * 60;
		}

		/// <summary>
		/// Gets or sets the project.
		/// </summary>
		/// <value>The project.</value>
		[Required]
		public string Project { get; set; }

		/// <summary>
		/// Gets or sets the name of the release.
		/// </summary>
		/// <value>The name of the release.</value>
		[Required]
		public string ReleaseName { get; set; }

		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		[Required]
		public string File { get; set; }

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
		/// Gets or sets the type of the file.
		/// </summary>
		/// <value>The type of the file.</value>
		public string FileType { get; set; }

		/// <summary>
		/// Gets or sets the type of the resource.
		/// </summary>
		/// <value>The type of the resource.</value>
		private FileResourceType ResourceType { get; set; }

		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		public int Timeout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is recommended.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is recommended; otherwise, <c>false</c>.
		/// </value>
		//public bool IsRecommended { get; set; }


		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			try {
				if ( string.IsNullOrEmpty ( File ) ) {
					throw new ArgumentException ( "File cannot be null or empty" );
				}

				if ( !System.IO.File.Exists ( File ) ) {
					throw new FileNotFoundException ( string.Format ( "The specified file was not found - ({0})", File ) );
				}

				if ( string.IsNullOrEmpty ( Username ) ) {
					throw new ArgumentException ( "Username cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( Password ) ) {
					throw new ArgumentException ( "Password cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( Project ) ) {
					throw new ArgumentException ( "Project cannot be null or empty" );
				}

				if ( string.IsNullOrEmpty ( ReleaseName ) ) {
					throw new ArgumentException ( "ReleaseName cannot be null or empty" );
				}

				ResourceType = (FileResourceType)Enum.Parse ( typeof ( FileResourceType ), FileType );


				Log.LogMessage ( "Uploading file: {0}", File );
				Upload ( );
				Log.LogMessage ( "Successfully uploaded file: {0}", Path.GetFileName ( File ) );
				return true;
			} catch ( Exception ex ) {
				Log.LogError ( ex.ToString ( ) );
				return false;
			}
		}

		/// <summary>
		/// Uploads this instance.
		/// </summary>
		private void Upload ( ) {
			ReleaseService service = new ReleaseService ( );
			service.Timeout = Timeout * 1000;
			service.UserAgent = String.Format ( "MSBuild Deployment Tasks (CodePlexUpload) {0}", Assembly.GetExecutingAssembly ( ).GetName ( ).Version.ToString ( ) );
			if ( !string.IsNullOrEmpty ( ProxyHost ) ) {
				service.Proxy = new WebProxy ( ProxyHost, ProxyPort );
				( service.Proxy as WebProxy ).BypassProxyOnLocal = true;
				if ( !string.IsNullOrEmpty ( ProxyUser ) ) {
					service.Proxy.Credentials = new NetworkCredential ( ProxyUser, ProxyPassword );
				}
			}

			byte[] data;
			using ( FileStream fs = new FileStream ( File, FileMode.Open, FileAccess.Read ) ) {
				int bytesRead = 0;
				byte[] buffer = new byte[1024];
				using ( MemoryStream ms = new MemoryStream ( ) ) {
					while ( ( bytesRead = fs.Read ( buffer, 0, buffer.Length ) ) > 0 ) {
						ms.Write ( buffer, 0, bytesRead );
					}

					data = ms.ToArray ( );
				}
			}

			ReleaseFile rfile = new ReleaseFile ( );
			rfile.Name = Path.GetFileNameWithoutExtension ( File );
			rfile.FileName = File;
			rfile.FileType = ResourceType.ToString ( );
			rfile.FileData = data;

			service.UploadTheReleaseFiles ( Project, ReleaseName, new ReleaseFile[] { rfile },
			 string.Empty, Username, Password );
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
	}
}
