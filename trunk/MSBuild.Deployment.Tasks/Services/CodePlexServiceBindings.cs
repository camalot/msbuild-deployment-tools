using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.Net;
using System.ServiceModel;

namespace MSBuild.Deployment.Tasks.Services {
	internal static class CodePlexServiceBindings {
		/// <summary>
		/// Creates the binding.
		/// </summary>
		/// <returns></returns>
		public static Binding CreateBinding ( ) {
			BasicHttpBinding binding = new BasicHttpBinding ( );
			binding.Security.Mode = BasicHttpSecurityMode.Transport;
			return binding;
		}

		/// <summary>
		/// Creates the binding.
		/// </summary>
		/// <param name="proxyHost">The proxy host.</param>
		/// <param name="proxyPort">The proxy port.</param>
		/// <param name="credentials">The credentials.</param>
		/// <returns></returns>
		public static Binding CreateBinding ( string proxyHost, int proxyPort ) {
			BasicHttpBinding binding = CreateBinding ( ) as BasicHttpBinding;

			binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
			binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

			binding.BypassProxyOnLocal = true;
			binding.ProxyAddress = new Uri ( string.Format ( "http://{0}:{1}", proxyHost, proxyPort ) );
			binding.UseDefaultWebProxy = false;
			return binding;
		}

		/// <summary>
		/// Creates the endpoint.
		/// </summary>
		/// <returns></returns>
		public static EndpointAddress CreateEndpoint ( ) {
			//Specify the address to be used for the client.
			return new EndpointAddress ( "https://www.codeplex.com/Services/ReleaseService.asmx" );
		}
		
	}
}
