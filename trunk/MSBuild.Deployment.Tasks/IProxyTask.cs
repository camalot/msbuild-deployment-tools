using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// 
	/// </summary>
	internal interface IProxyTask {
		/// <summary>
		/// Gets or sets the proxy host.
		/// </summary>
		/// <value>The proxy host.</value>
		string ProxyHost { get; set; }
		/// <summary>
		/// Gets or sets the proxy port.
		/// </summary>
		/// <value>The proxy port.</value>
		int ProxyPort { get; set; }
		/// <summary>
		/// Gets or sets the proxy user.
		/// </summary>
		/// <value>The proxy user.</value>
		string ProxyUser { get; set; }
		/// <summary>
		/// Gets or sets the proxy password.
		/// </summary>
		/// <value>The proxy password.</value>
		string ProxyPassword { get; set; }
	}
}
