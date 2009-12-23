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
using Microsoft.Build.Framework;
using System.IO;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// 
	/// </summary>
	public class SkyDriveUpload : Task, IProxyTask, ITimeoutTask {
		/// <summary>
		/// Initializes a new instance of the <see cref="SkyDriveUpload"/> class.
		/// </summary>
		public SkyDriveUpload ( ) {
			Client = new SkyDriveWebClient ( );
			Timeout = 60 * 5;
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
		/// Gets or sets the release path.
		/// </summary>
		/// <value>The release path.</value>
		[Required]
		public string ReleasePath { get; set; }
		/// <summary>
		/// Gets or sets the file.
		/// </summary>
		/// <value>The file.</value>
		[Required]
		public string File { get; set; }

		/// <summary>
		/// Gets the download URL.
		/// </summary>
		/// <value>The download URL.</value>
		[Output]
		public string DownloadUrl { get; private set; }
		/// <summary>
		/// Gets or sets the path URL.
		/// </summary>
		/// <value>The path URL.</value>
		[Output]
		public string PathUrl { get; private set; }
		/// <summary>
		/// Gets or sets the client.
		/// </summary>
		/// <value>The client.</value>
		private SkyDriveWebClient Client { get; set; }

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
				Client.LogOn ( this.Username, this.Password );
				Client.Timeout = Timeout * 1000;

				WebFolderInfo uploadFolder = GetReleaseFolder ( );
				if ( uploadFolder != null ) {
					UploadFile ( uploadFolder );
				} else {
					throw new ArgumentException ( "Unable to locate upload folder. Please check ReleasePath." );
				}
				return true;
			} catch ( Exception ex ) {
				Log.LogError ( ex.ToString ( ) );
				return false;
			}
		}

		/// <summary>
		/// Gets the release folder.
		/// </summary>
		/// <returns></returns>
		private WebFolderInfo GetReleaseFolder ( ) {
			string[] folders = ReleasePath.Split ( new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries );
			WebFolderInfo rootFolder = null;
			List<WebFolderInfo> rootFolders = new List<WebFolderInfo> ( Client.ListRootWebFolders ( ) );

			foreach ( var item in rootFolders ) {
				if ( string.Compare ( item.Name, folders[ 0 ], true ) == 0 ) {
					rootFolder = item;
					break;
				}
			}

			if ( rootFolder != null ) {
				WebFolderInfo parent = rootFolder;
				for ( int i = 1; i < folders.Length; i++ ) {
					List<WebFolderInfo> sfolders = new List<WebFolderInfo> ( Client.ListSubWebFolders ( parent ) );
					foreach ( var item in sfolders ) {
						if ( string.Compare ( item.Name, folders[ i ], true ) == 0 ) {
							parent = item;
							break;
						}
					}
				}
				return parent;
			} else {
				Log.LogError ( "Unable to locate root folder, cannot continue" );
				return null;
			}
		}

		/// <summary>
		/// Uploads the file.
		/// </summary>
		/// <param name="parent">The parent.</param>
		private void UploadFile ( WebFolderInfo parent ) {
			Log.LogMessage ( "Uploading file ({0}) to {1}", Path.GetFileName ( File ), ReleasePath );
			Client.UploadWebFile ( File, parent );

			List<WebFileInfo> files = new List<WebFileInfo> ( Client.ListSubWebFolderFiles ( parent ) );
			WebFileInfo uploadedFile = null;
			foreach ( var item in files ) {
				if ( string.Compare ( item.FullName, Path.GetFileName ( File ), true ) == 0 ) {
					uploadedFile = item;
					break;
				}
			}

			if ( uploadedFile != null ) {
				DownloadUrl = uploadedFile.ViewUrl;
				PathUrl = uploadedFile.PathUrl;
				Client.ChangeWebFileDescription ( uploadedFile, string.Format ( "Uploaded by: {0}", this.GetUserAgent ( ) ) );
				Log.LogMessage ( "Download url set: {0}", DownloadUrl );
			} else {
				Log.LogError ( "Unable to locate uplaoded file, cannot continue" );
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

	}
}
