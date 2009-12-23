using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuild.Deployment.Tasks {
	/// <summary>
	/// 
	/// </summary>
	public interface ITreatErrorsAsWarningsTask {

		/// <summary>
		/// Gets or sets a value indicating whether to treat errors as warnings.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if treat errors as warnings; otherwise, <c>false</c>.
		/// </value>
		bool TreatErrorsAsWarnings { get; set; }

	}
}
