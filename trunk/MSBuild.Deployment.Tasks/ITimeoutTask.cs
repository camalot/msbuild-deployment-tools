using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuild.Deployment.Tasks {
	public interface ITimeoutTask {

		/// <summary>
		/// Gets or sets the timeout in seconds.
		/// </summary>
		/// <value>The timeout in seconds.</value>
		int Timeout { get; set; }
	}
}
