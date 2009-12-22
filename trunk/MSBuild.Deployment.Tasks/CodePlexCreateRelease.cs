using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using MSBuild.Deployment.Tasks.Services;
using System.Net;
using System.ServiceModel.Channels;
using System.Reflection;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// Creates a release on a codeplex project site
	/// </summary>
	public class CodePlexCreateRelease : Task, IProxyTask, ITimeoutTask {
		/// <summary>
		/// 
		/// </summary>
		public enum DevelopmentStatus {
			/// <summary>
			/// 
			/// </summary>
			Alpha,
			/// <summary>
			/// 
			/// </summary>
			Beta,
			/// <summary>
			/// 
			/// </summary>
			Stable,
			/// <summary>
			/// 
			/// </summary>
			Planning
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CodePlexCreateRelease"/> class.
		/// </summary>
		public CodePlexCreateRelease ( ) {
			ReleaseDate = DateTime.Now;
			Status = DevelopmentStatus.Stable.ToString ( );
			ReleaseStatus = DevelopmentStatus.Stable;
			IsShownToPublic = false;
			IsDefaultRelease = false;
			ProxyPort = 8080;
			Timeout = 60;
		}

		/// <summary>
		/// Gets or sets the name of the project.
		/// </summary>
		/// <value>The name of the project.</value>
		[Required]
		public string Project { get; set; }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

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
		/// Gets or sets the name of the project friendly.
		/// </summary>
		/// <value>The name of the project friendly.</value>
		[Required]
		public string ProjectFriendlyName { get; set; }

		/// <summary>
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		[Required]
		public String Version { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[Output]
		public string ReleaseName {
			get {
				return string.Format ( "{0} {1} {2}", ProjectFriendlyName, Version, ReleaseStatus.ToString ( ) );
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is default release.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is default release; otherwise, <c>false</c>.
		/// </value>
		public bool IsDefaultRelease { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is shown to public.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is shown to public; otherwise, <c>false</c>.
		/// </value>
		public bool IsShownToPublic { get; set; }

		/// <summary>
		/// Gets or sets the release status.
		/// </summary>
		/// <value>The status.</value>
		private DevelopmentStatus ReleaseStatus { get; set; }


		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		public string Status { get; set; }

		/// <summary>
		/// Gets or sets the release date.
		/// </summary>
		/// <value>The release date.</value>
		public DateTime ReleaseDate { get; set; }

		/// <summary>
		/// Gets or sets the release id.
		/// </summary>
		/// <value>The release id.</value>
		[Output]
		public int ReleaseId { get; set; }


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
				if ( string.IsNullOrEmpty ( ProjectFriendlyName ) ) {
					throw new ArgumentException ( "ProjectFriendlyName cannot be null or empty" );
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

				if ( string.IsNullOrEmpty ( Version ) ) {
					throw new ArgumentException ( "Version cannot be null or empty" );
				}

				ReleaseStatus = (DevelopmentStatus)Enum.Parse ( typeof ( DevelopmentStatus ), Status );

				Log.LogMessage ( "Creating Release" );
				CreateRelease ( );
				Log.LogMessage ( "Successfully created release: {0} ({1})", ReleaseName, ReleaseId );
				return true;
			} catch ( Exception ex ) {
				Log.LogError ( ex.ToString ( ) );
				return false;
			}
		}

		private void CreateRelease ( ) {
			ReleaseService service = new ReleaseService ( );
			service.UserAgent = this.GetUserAgent ( );
			service.Timeout = Timeout * 1000;

			if ( !string.IsNullOrEmpty(ProxyHost) ) {
				service.Proxy = new WebProxy ( ProxyHost, ProxyPort );
				( service.Proxy as WebProxy ).BypassProxyOnLocal = true;
				if ( !string.IsNullOrEmpty ( ProxyUser ) ) {
					service.Proxy.Credentials = new NetworkCredential ( ProxyUser, ProxyPassword );
				}
			}

			ReleaseId = service.CreateARelease ( Project, ReleaseName,
				Description, ReleaseDate.ToString ( "MM/dd/yyyy" ), Status.ToString ( ),
				IsShownToPublic, IsDefaultRelease, Username, Password );
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

		#region IUserAgentTask Members

		public string UserAgent {
			get { throw new NotImplementedException ( ); }
		}

		#endregion
	}
}
