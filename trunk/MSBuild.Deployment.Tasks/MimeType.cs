using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;

namespace MSBuild.Deployment.Tasks {
	public class MimeType {
		#region Win32Api
		/// <summary>
		/// Finds the mime type from data.
		/// </summary>
		/// <param name="pBC">The p BC.</param>
		/// <param name="pwzUrl">The PWZ URL.</param>
		/// <param name="pBuffer">The p buffer.</param>
		/// <param name="cbSize">Size of the cb.</param>
		/// <param name="pbwMimeProposed">The PBW MIME proposed.</param>
		/// <param name="dwMimeFlags">The dw MIME flags.</param>
		/// <param name="ppwzMimeOut">The PPWZ MIME out.</param>
		/// <param name="dwReserved">The dw reserved.</param>
		/// <returns></returns>
		[DllImport ( @"UrlMon.dll", CharSet = CharSet.Auto )]
		private static extern UInt32 FindMimeFromData (
			UInt32 pBC,
			[MarshalAs ( UnmanagedType.LPStr )]
			string pwzUrl,
			[MarshalAs ( UnmanagedType.LPArray )]
			byte[] pBuffer,
			UInt32 cbSize,
			[MarshalAs ( UnmanagedType.LPStr )]
			string pbwMimeProposed,
			UInt32 dwMimeFlags,
			out IntPtr ppwzMimeOut,
			UInt32 dwReserved );
		#endregion

		#region statics
		/// <summary>
		/// Initializes the <see cref="MimeType"/> class.
		/// </summary>
		static MimeType ( ) {
			DefaultContentType = @"application/octet-stream";
		}

		/// <summary>
		/// Gets the default type of the content.
		/// </summary>
		/// <value>The default type of the content.</value>
		public static string DefaultContentType { get; private set; }


		/// <summary>
		/// Creates a new MimeType from the specified file.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		public static MimeType Create ( FileInfo file ) {
			return new MimeType ( file );
		}

		/// <summary>
		/// Creates a new MimeType from the specified extension.
		/// </summary>
		/// <param name="extension">The extension.</param>
		/// <returns></returns>
		public static MimeType Create ( string extension ) {
			return new MimeType ( extension );
		}
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="MimeType"/> class.
		/// </summary>
		/// <param name="file">The file.</param>
		internal MimeType ( FileInfo file ) {
			this.Extension = Path.GetExtension ( file.FullName );
			ContentType = GetContentType ( file );
		}



		/// <summary>
		/// Initializes a new instance of the <see cref="MimeType"/> class.
		/// </summary>
		/// <param name="extension">The extension.</param>
		internal MimeType ( string extension ) {
			string ext = extension;
			if ( string.IsNullOrEmpty ( ext ) ) {
				ext = "*";
			}

			if ( !ext.StartsWith ( "." ) && ext.CompareTo ( "*" ) != 0 ) {
				if ( ext.IndexOf ( "." ) == -1 ) {
					ext = string.Concat ( ".", ext );
				} else {
					ext = ext.Substring ( ext.LastIndexOf ( "." ) );
				}
			}

			Extension = ext;
			ContentType = GetContentTypeFromRegistry ( Extension );
		}

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <value>The type of the content.</value>
		public string ContentType { get; internal set; }
		/// <summary>
		/// Gets or sets the extension.
		/// </summary>
		/// <value>The extension.</value>
		public string Extension { get; internal set; }

		/// <summary>
		/// Gets the type of the content.
		/// </summary>
		/// <param name="file">The file.</param>
		/// <returns></returns>
		private string GetContentType ( FileInfo file ) {

			string result = DefaultContentType;
			if ( file.Exists ) {
				byte[] buffer = new byte[ 256 ];
				// read the data for retrieving the mimetype
				using ( FileStream fs = new FileStream ( file.FullName, FileMode.Open, FileAccess.Read ) ) {
					// only need 256 bytes of data
					if ( fs.Length >= buffer.Length ) {
						fs.Read ( buffer, 0, buffer.Length );
					} else {
						fs.Read ( buffer, 0, (int)fs.Length );
					}
				}
				result = GetContentTypeFromData ( buffer );
				Console.WriteLine ( "{1} - ContentType from Data: {0}", result, file.Name );
			}

			// if we have the default, then lets "check" the registry
			if ( string.IsNullOrEmpty ( result ) || result.CompareTo ( DefaultContentType ) == 0 ) {
				result = GetContentTypeFromRegistry ( file.Extension );
				Console.WriteLine ( "{1} - ContentType from Registry: {0}", result, file.Name );
			}

			return result;
		}

		/// <summary>
		/// Gets the content type from data.
		/// </summary>
		/// <param name="data">A byte array containing the bytes of data from the file.</param>
		/// <returns></returns>
		private string GetContentTypeFromData ( byte[] data ) {
			try {
				IntPtr mimePointer = IntPtr.Zero;
				FindMimeFromData ( 0, null, data, (UInt32)data.Length, null, 0, out mimePointer, 0 );

				if ( mimePointer == IntPtr.Zero ) {
					return DefaultContentType;
				} else {
					string result = Marshal.PtrToStringUni ( mimePointer );
					Marshal.FreeCoTaskMem ( mimePointer );
					if ( string.IsNullOrEmpty ( result ) ) {
						return DefaultContentType;
					} else {
						return result;
					}
				}
			} catch ( Exception ex ) {
				return DefaultContentType;
			}
		}

		/// <summary>
		/// Gets the content type from registry.
		/// </summary>
		/// <param name="extension">The extension.</param>
		/// <returns></returns>
		private string GetContentTypeFromRegistry ( string extension ) {
			using ( RegistryKey key = Registry.ClassesRoot.OpenSubKey ( extension ) ) {
				if ( key == null ) {
					return DefaultContentType;
				} else {
					string result = (string)key.GetValue ( "Content Type", DefaultContentType );
					return result;
				}
			}
		}
	}
}
