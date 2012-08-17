using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MSBuild.Deployment.Tasks.Helpers {
	public static class StringHelper {
		public static byte[] ToByteArray (  this String s) {
			return ToByteArray ( s, Encoding.Default );
		}

		public static byte[] ToByteArray ( this String s,  Encoding encoding ) {
			return encoding.GetBytes ( s );
		}
	}
}
