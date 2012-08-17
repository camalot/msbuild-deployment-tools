using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Net;
using MSBuild.Deployment.Tasks.Helpers;

namespace MSBuild.Deployment.Tasks {
	public class JsonHttpPost : HttpPost {
		/// <summary>
		/// Initializes a new instance of the <see cref="JsonHttpPost"/> class.
		/// </summary>
		public JsonHttpPost ( ) {
			ProxyPort = 8080;
			Timeout = 60;
			TreatErrorsAsWarnings = false;
			ContentType = "application/json; charset=utf-8";
		}

	}
}
