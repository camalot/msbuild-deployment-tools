using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using MSBuild.Deployment.Tasks;
using System.IO;

namespace MdtTests {
	public class MimeTypeTests {

		public MimeTypeTests ( ) {

		}

		[Fact]
		public void CreateMimeTypeNoRealFile ( ) {
			MimeType txt = MimeType.Create ( new FileInfo ( @"c:\foo.txt" ) );
			Assert.Equal<string> ( "text/plain", txt.ContentType );
			Assert.Equal<string> ( ".txt", txt.Extension );

			MimeType norealextension = MimeType.Create ( new FileInfo ( @"c:\foo.norealextension" ) );
			Assert.Equal<string> ( MimeType.DefaultContentType, norealextension.ContentType );
			Assert.Equal<string> ( ".norealextension", norealextension.Extension );
		}

		[Fact]
		public void CreateMimeType ( ) {
			MimeType csfile = MimeType.Create ( new FileInfo ( @"G:\Projects\Csharp\MSBuild.Deployment.Tasks\trunk\3rdParty\CodePlexApi.cs" ) );
			Assert.Equal<string> ( "text/plain", csfile.ContentType );
			Assert.Equal<string> ( ".cs", csfile.Extension );

			MimeType bat = MimeType.Create ( new FileInfo ( @"c:\autoexec.bat" ) );
			Assert.Equal<string> ( "application/octet-stream", bat.ContentType );
			Assert.Equal<string> ( ".bat", bat.Extension );

			MimeType msi = MimeType.Create ( new FileInfo ( @"B:\tivoli\GSS3ScanDataServiceSetup.msi" ) );
			Assert.Equal<string> ( "application/octet-stream", msi.ContentType );
			Assert.Equal<string> ( ".msi", msi.Extension );


		}

		[Fact]
		public void CreateMimeTypeFromExtension ( ) {
			MimeType msbuild = MimeType.Create ( ".msbuild" );
			Assert.Equal<string> ( "text/xml", msbuild.ContentType );
			Assert.Equal<string> ( ".msbuild", msbuild.Extension );
		}

	}
}
