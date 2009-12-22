using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.Reflection;

namespace MSBuild.Deployment.Tasks {
	public static class TaskUserAgent {

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		public static string GetUserAgent ( this Task task ) {
			return String.Format ( "MSBuild Deployment Tasks ({1}) {0}", task.GetType ( ).Assembly.GetName ( ).Version.ToString ( ), task.GetType ( ).Name );
		}

	}
}
