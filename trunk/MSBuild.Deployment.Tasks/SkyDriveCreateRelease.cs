/*
 * Makes use of SkyDrive .Net API Client
 * http://skydriveapiclient.codeplex.com/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;

using HgCo.WindowsLive.SkyDrive;
using System.Reflection;
using Microsoft.Build.Framework;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// 
	/// </summary>
	public class SkyDriveCreateRelease : Task, IProxyTask, ITimeoutTask, ITreatErrorsAsWarningsTask {

		/// <summary>
		/// Initializes a new instance of the <see cref="SkyDriveCreateRelease"/> class.
		/// </summary>
		public SkyDriveCreateRelease ( ) {
			RootFolder = "Releases";
			InternalShareType = WebFolderItemShareType.Private;
			ShareType = InternalShareType.ToString ( );
			Timeout = 30;
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
		/// Gets or sets the version.
		/// </summary>
		/// <value>The version.</value>
		[Required]
		public string Version { get; set; }
		/// <summary>
		/// Gets or sets the root folder.
		/// </summary>
		/// <value>The root folder.</value>
		public string RootFolder { get; set; }

		/// <summary>
		/// Gets or sets the release path.
		/// </summary>
		/// <value>The release path.</value>
		[Output]
		public string ReleasePath { get; private set; }
		/// <summary>
		/// Gets or sets the type of the share.
		/// <p>
		/// Values:
		/// <ul>
		///		<li>Private (Default)</li>
		///		<li>PeopleSelected</li>
		///		<li>None</li>
		///		<li>MyNetwork</li>
		///		<li>Public</li>
		/// </ul>
		/// </p>
		/// </summary>
		/// <value>The type of the share.</value>
		public string ShareType { get; set; }

		internal WebFolderItemShareType InternalShareType { get; set; }

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {

			if ( string.IsNullOrEmpty ( Username ) ) {
				throw new ArgumentException ( "Username cannot be null or empty" );
			}

			if ( string.IsNullOrEmpty ( Password ) ) {
				throw new ArgumentException ( "Password cannot be null or empty" );
			}

			if ( string.IsNullOrEmpty ( Project ) ) {
				throw new ArgumentException ( "Project cannot be null or empty" );
			}

			if ( string.IsNullOrEmpty ( RootFolder ) ) {
				throw new ArgumentException ( "RootFolder cannot be null or empty" );
			}

			InternalShareType = (WebFolderItemShareType)Enum.Parse ( typeof ( WebFolderItemShareType ), ShareType );

			SkyDriveWebClient client = new SkyDriveWebClient ( );
			WebFolderInfo rootFolder = null;

			client.Timeout = Timeout * 1000;

			try {
				client.LogOn ( this.Username, this.Password );

				client.CreateRootWebFolder ( RootFolder, InternalShareType );
				List<WebFolderInfo> rootFolders = new List<WebFolderInfo> ( client.ListRootWebFolders ( ) );


				foreach ( var item in rootFolders ) {
					if ( string.Compare ( item.Name, RootFolder, true ) == 0 ) {
						rootFolder = item;
						break;
					}
				}

				if ( rootFolder != null ) {
					WebFolderInfo projectFolder = null;
					client.CreateSubWebFolder ( Project, rootFolder );

					List<WebFolderInfo> sfolders = new List<WebFolderInfo> ( client.ListSubWebFolders ( rootFolder ) );
					foreach ( var item in sfolders ) {
						if ( string.Compare ( item.Name, Project, true ) == 0 ) {
							projectFolder = item;
							break;
						}
					}
					if ( projectFolder != null ) {
						WebFolderInfo versionFolder = null;
						client.CreateSubWebFolder ( Version, projectFolder );

						List<WebFolderInfo> svfolders = new List<WebFolderInfo> ( client.ListSubWebFolders ( projectFolder ) );
						foreach ( var item in svfolders ) {
							if ( string.Compare ( item.Name, Version, true ) == 0 ) {
								versionFolder = item;
								client.ChangeWebFolderDescription ( versionFolder, string.Format ( "Created by: {0}", this.GetUserAgent ( ) ) );
								break;
							}
						}

						if ( versionFolder != null ) {
							ReleasePath = string.Format ( @"{0}/{1}/{2}/", RootFolder, Project, Version );
							Log.LogMessage ( "Created Release Folder: {0}", ReleasePath );
							return true;
						} else {
							return LogErrorOrWarning ( "Unable to locate version folder, cannot continue" );
						}
					} else {
						return LogErrorOrWarning ( "Unable to locate project folder, cannot continue" );
					}
				} else {
					return LogErrorOrWarning ( "Unable to locate root folder, cannot continue" );
				}
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

		private bool LogErrorOrWarning ( string message ) {
			if ( TreatErrorsAsWarnings ) {
				Log.LogWarning ( message );
			} else {
				Log.LogError ( message );
			}
			return TreatErrorsAsWarnings;
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
