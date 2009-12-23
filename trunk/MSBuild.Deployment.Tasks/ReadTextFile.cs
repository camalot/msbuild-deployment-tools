using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.IO;
using Microsoft.Build.Framework;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// reads the data from the specified files
	/// </summary>
	/// <example>
	/// <ReadTextFile Files="@(InputFiles)">
	///		<Output PropertyName="OutputText" TaskParameter="OutputText" />
	/// </ReadTextFile>
	/// </example>
	public class ReadTextFile : Task, ITreatErrorsAsWarningsTask {
		/// <summary>
		/// Gets or sets the files.
		/// </summary>
		/// <value>The files.</value>
		[Required]
		public string[] Files { get; set; }
		/// <summary>
		/// Gets or sets the output text.
		/// </summary>
		/// <value>The output text.</value>
		[Output]
		public string[] OutputText { get; private set; }


		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Execute ( ) {
			List<string> outData = new List<string> ( );
			try {
				foreach ( string file in Files ) {
					FileInfo fi = new FileInfo ( file );
					if ( !fi.Exists ) {
						throw new FileNotFoundException ( string.Format ( "The file '{0}' was not found.", file ) );
					} else {
						StringBuilder data = new StringBuilder ( );
						Log.LogMessage ( "Reading file: {0}", fi.FullName );
						using ( FileStream fs = new FileStream ( fi.FullName, FileMode.Open, FileAccess.Read ) ) {
							using ( StreamReader sr = new StreamReader ( fs ) ) {
								while ( !sr.EndOfStream ) {
									data.AppendLine ( sr.ReadLine ( ) );
								}
							}
						}
						outData.Add(data.ToString());
					}
				}

				OutputText = outData.ToArray ( );
				return true;
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
