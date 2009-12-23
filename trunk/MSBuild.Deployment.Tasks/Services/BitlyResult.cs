using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MSBuild.Deployment.Tasks.Services {
	/// <summary>
	/// Represents the results of the bitly request.
	/// </summary>
	[XmlRoot("bitly")]
	public class BitlyResult {
		/// <summary>
		/// Initializes a new instance of the <see cref="BitlyResult"/> class.
		/// </summary>
		public BitlyResult ( ) {
			Results = new List<NodeKeyValue> ( );
		}

		/// <summary>
		/// Gets or sets the error code.
		/// </summary>
		/// <value>The error code.</value>
		[XmlElement ( "errorCode" )]
		public int ErrorCode { get; set; }
		/// <summary>
		/// Gets or sets the error message.
		/// </summary>
		/// <value>The error message.</value>
		[XmlElement ( "errorMessage" )]
		public string ErrorMessage { get; set; }

		/// <summary>
		/// Gets or sets the results.
		/// </summary>
		/// <value>The results.</value>
		[XmlArray("results"),XmlArrayItem("nodeKeyVal")]
		public List<NodeKeyValue> Results { get; set; }

		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
		[XmlElement ( "statusCode" )]
		public string Status { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public class NodeKeyValue {
			/// <summary>
			/// Gets or sets the user hash.
			/// </summary>
			/// <value>The user hash.</value>
			[XmlElement ( "userHash" )]
			public string UserHash { get; set; }
			/// <summary>
			/// Gets or sets the short key word URL.
			/// </summary>
			/// <value>The short key word URL.</value>
			[XmlElement ( "shortKeywordUrl" )]
			public string ShortKeyWordUrl { get; set; }
			/// <summary>
			/// Gets or sets the hash.
			/// </summary>
			/// <value>The hash.</value>
			[XmlElement ( "hash" )]
			public string Hash { get; set; }
			/// <summary>
			/// Gets or sets the node key.
			/// </summary>
			/// <value>The node key.</value>
			[XmlElement ( "nodeKey" )]
			public string NodeKey { get; set; }
			/// <summary>
			/// Gets or sets the short URL.
			/// </summary>
			/// <value>The short URL.</value>
			[XmlElement("shortUrl")]
			public string ShortUrl { get; set; }
		}
	}
}
