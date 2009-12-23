using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using System.Reflection;
using MSBuild.Deployment.Tasks.Services;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// 
	/// </summary>
	public static class TaskUserAgent {

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		public static string GetUserAgent ( this Task task ) {
			return GetUserAgent ( (object)task );
		}

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <returns></returns>
		public static string GetUserAgent ( this BitlyService task ) {
			return GetUserAgent ( (object)task );
		}

		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		private static string GetUserAgent ( object obj ) {
			return String.Format ( "MSBuild Deployment Tasks ({1}) {0}", obj.GetType ( ).Assembly.GetName ( ).Version.ToString ( ), obj.GetType ( ).Name );
		}
	}
}
