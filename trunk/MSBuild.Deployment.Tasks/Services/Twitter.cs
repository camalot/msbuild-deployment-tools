//
// Yedda Twitter C# Library (or more of an API wrapper) v0.1
// Written by Eran Sandler (eran AT yedda.com)
// http://devblog.yedda.com/index.php/twitter-c-library/
//
// The library is provided on a "AS IS" basis. Yedda is not repsonsible in any way 
// for whatever usage you do with it.
//
// Giving credit would be nice though :-)
//
// Get more cool dev information and other stuff at the Yedda Dev Blog:
// http://devblog.yedda.com
//
// Got a question about this library? About programming? C#? .NET? About anything else?
// Ask about it at Yedda (http://yedda.com) and get answers from real people.
//
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Web;
using System.Collections.Generic;
using System.Text;

namespace MSBuild.Deployment.Tasks.Services {
	/// <summary>
	/// 
	/// </summary>
	public class Twitter {

		/// <summary>
		/// The output formats supported by Twitter. Not all of them can be used with all of the functions.
		/// For more information about the output formats and the supported functions Check the 
		/// Twitter documentation at: http://groups.google.com/group/twitter-development-talk/web/api-documentation
		/// </summary>
		public enum OutputFormatType {
			/// <summary>
			/// 
			/// </summary>
			JSON,
			/// <summary>
			/// 
			/// </summary>
			XML,
			/// <summary>
			/// 
			/// </summary>
			RSS,
			/// <summary>
			/// 
			/// </summary>
			Atom
		}

		/// <summary>
		/// The various object types supported at Twitter.
		/// </summary>
		public enum ObjectType {
			/// <summary>
			/// 
			/// </summary>
			Statuses,
			/// <summary>
			/// 
			/// </summary>
			Account,
			/// <summary>
			/// 
			/// </summary>
			Users
		}

		/// <summary>
		/// The various actions used at Twitter. Not all actions works on all object types.
		/// For more information about the actions types and the supported functions Check the 
		/// Twitter documentation at: http://groups.google.com/group/twitter-development-talk/web/api-documentation
		/// </summary>
		public enum ActionType {
			/// <summary>
			/// 
			/// </summary>
			Public_Timeline,
			/// <summary>
			/// 
			/// </summary>
			User_Timeline,
			/// <summary>
			/// 
			/// </summary>
			Friends_Timeline,
			/// <summary>
			/// 
			/// </summary>
			Friends,
			/// <summary>
			/// 
			/// </summary>
			Followers,
			/// <summary>
			/// 
			/// </summary>
			Update,
			/// <summary>
			/// 
			/// </summary>
			Account_Settings,
			/// <summary>
			/// 
			/// </summary>
			Featured,
			/// <summary>
			/// 
			/// </summary>
			Show,
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Twitter"/> class.
		/// </summary>
		public Twitter ( ) {
			System.Net.ServicePointManager.Expect100Continue = false;
		}

		/// <summary>
		/// Gets or sets the proxy.
		/// </summary>
		/// <value>The proxy.</value>
		public WebProxy Proxy { get; set; }

		/// <summary>
		/// Source is an additional parameters that will be used to fill the "From" field.
		/// Currently you must talk to the developers of Twitter at:
		/// http://groups.google.com/group/twitter-development-talk/
		/// Otherwise, Twitter will simply ignore this parameter and set the "From" field to "web".
		/// </summary>
		public string Source { get; set; } 

		/// <summary>
		/// Sets the name of the Twitter client.
		/// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
		/// the Twitter developers, this will be used in the future (hopefully near) to set more information
		/// in Twitter about the client posting the information as well as future usage in a clients directory.
		/// </summary>
		public string TwitterClient { get; set; } 

		/// <summary>
		/// Sets the version of the Twitter client.
		/// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
		/// the Twitter developers, this will be used in the future (hopefully near) to set more information
		/// in Twitter about the client posting the information as well as future usage in a clients directory.
		/// </summary>
		public string TwitterClientVersion { get; set; } 

		/// <summary>
		/// Sets the URL of the Twitter client.
		/// Must be in the XML format documented in the "Request Headers" section at:
		/// http://twitter.pbwiki.com/API-Docs.
		/// According to the Twitter Fan Wiki at http://twitter.pbwiki.com/API-Docs and supported by
		/// the Twitter developers, this will be used in the future (hopefully near) to set more information
		/// in Twitter about the client posting the information as well as future usage in a clients directory.		
		/// </summary>
		public string TwitterClientUrl { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected const string TwitterBaseUrlFormat = "http://twitter.com/{0}/{1}.{2}";

		/// <summary>
		/// Gets the object type string.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns></returns>
		protected string GetObjectTypeString(ObjectType objectType) {
			return objectType.ToString().ToLower();
		}

		/// <summary>
		/// Gets the action type string.
		/// </summary>
		/// <param name="actionType">Type of the action.</param>
		/// <returns></returns>
		protected string GetActionTypeString(ActionType actionType) {
			return actionType.ToString().ToLower();
		}

		/// <summary>
		/// Gets the format type string.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		protected string GetFormatTypeString(OutputFormatType format) {
			return format.ToString().ToLower();
		}

		/// <summary>
		/// Executes an HTTP GET command and retrives the information.		
		/// </summary>
		/// <param name="url">The URL to perform the GET operation</param>
		/// <param name="userName">The username to use with the request</param>
		/// <param name="password">The password to use with the request</param>
		/// <returns>The response of the request, or null if we got 404 or nothing.</returns>
		protected string ExecuteGetCommand(string url, string userName, string password) {
			using (WebClient client = new WebClient()) {
				if ( Proxy != null ) {
					client.Proxy = Proxy;
				}
				if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)) {
					client.Credentials = new NetworkCredential(userName, password);
				}

				try {
					using (Stream stream = client.OpenRead(url)) {
						using (StreamReader reader = new StreamReader(stream)) {
							return reader.ReadToEnd();
						}
					}
				}
				catch (WebException ex) {
					//
					// Handle HTTP 404 errors gracefully and return a null string to indicate there is no content.
					//
					if (ex.Response is HttpWebResponse) {
						if ((ex.Response as HttpWebResponse).StatusCode == HttpStatusCode.NotFound) {
							return null;
						}
					}

					throw ex;
				}
			}

			return null;
		}

		/// <summary>
		/// Executes an HTTP POST command and retrives the information.		
		/// This function will automatically include a "source" parameter if the "Source" property is set.
		/// </summary>
		/// <param name="url">The URL to perform the POST operation</param>
		/// <param name="userName">The username to use with the request</param>
		/// <param name="password">The password to use with the request</param>
		/// <param name="data">The data to post</param> 
		/// <returns>The response of the request, or null if we got 404 or nothing.</returns>
		protected string ExecutePostCommand(string url, string userName, string password, string data) {
			HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
			if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)) {
				request.Credentials = new NetworkCredential(userName, password);
				request.ContentType = "application/x-www-form-urlencoded";
				request.Method = "POST";
				request.UserAgent = TwitterClient;
				if ( Proxy != null ) {
					request.Proxy = Proxy;
				}

				if (!string.IsNullOrEmpty(TwitterClient)) {
					request.Headers.Add("X-Twitter-Client", TwitterClient);
				}

				if (!string.IsNullOrEmpty(TwitterClientVersion)) {
					request.Headers.Add("X-Twitter-Version", TwitterClientVersion);
				}

				if (!string.IsNullOrEmpty(TwitterClientUrl)) {
					request.Headers.Add("X-Twitter-URL", TwitterClientUrl);
				}


				if (!string.IsNullOrEmpty(Source)) {
					data += "&source=" + HttpUtility.UrlEncode(Source);
				}

				byte[] bytes = Encoding.UTF8.GetBytes(data);

				request.ContentLength = bytes.Length;
				using (Stream requestStream = request.GetRequestStream()) {
					requestStream.Write(bytes, 0, bytes.Length);

					using (WebResponse response = request.GetResponse()) {
						using (StreamReader reader = new StreamReader(response.GetResponseStream())) {
							return reader.ReadToEnd();
						}
					}
				}
			}

			return null;
		}

		#region Public_Timeline

		/// <summary>
		/// Gets the public timeline.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetPublicTimeline(OutputFormatType format) {
			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Public_Timeline), GetFormatTypeString(format));
			return ExecuteGetCommand(url, null, null);
		}

		/// <summary>
		/// Gets the public timeline as JSON.
		/// </summary>
		/// <returns></returns>
		public string GetPublicTimelineAsJSON() {
			return GetPublicTimeline(OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the public timeline as XML.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public XmlDocument GetPublicTimelineAsXML(OutputFormatType format) {
			if (format == OutputFormatType.JSON) {
				throw new ArgumentException("GetPublicTimelineAsXml supports only XML based formats (XML, RSS, Atom)", "format");
			}

			string output = GetPublicTimeline(format);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		/// <summary>
		/// Gets the public timeline as XML.
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetPublicTimelineAsXML() {
			return GetPublicTimelineAsXML(OutputFormatType.XML);
		}

		/// <summary>
		/// Gets the public timeline as RSS.
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetPublicTimelineAsRSS() {
			return GetPublicTimelineAsXML(OutputFormatType.RSS);
		}

		/// <summary>
		/// Gets the public timeline as atom.
		/// </summary>
		/// <returns></returns>
		public XmlDocument GetPublicTimelineAsAtom() {
			return GetPublicTimelineAsXML(OutputFormatType.Atom);
		}

		#endregion

		#region User_Timeline

		/// <summary>
		/// Gets the user timeline.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetUserTimeline(string userName, string password, string IDorScreenName, OutputFormatType format) {
			string url = null;
			if (string.IsNullOrEmpty(IDorScreenName)) {
				url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.User_Timeline), GetFormatTypeString(format));
			}
			else {
				url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.User_Timeline) + "/" + IDorScreenName, GetFormatTypeString(format));
			}

			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the user timeline.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetUserTimeline(string userName, string password, OutputFormatType format) {
			return GetUserTimeline(userName, password, null, format);
		}

		/// <summary>
		/// Gets the user timeline as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public string GetUserTimelineAsJSON(string userName, string password) {
			return GetUserTimeline(userName, password, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the user timeline as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public string GetUserTimelineAsJSON(string userName, string password, string IDorScreenName) {
			return GetUserTimeline(userName, password, IDorScreenName, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the user timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsXML(string userName, string password, string IDorScreenName, OutputFormatType format) {
			if (format == OutputFormatType.JSON) {
				throw new ArgumentException("GetUserTimelineAsXML supports only XML based formats (XML, RSS, Atom)", "format");
			}

			string output = GetUserTimeline(userName, password, IDorScreenName, format);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		/// <summary>
		/// Gets the user timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsXML(string userName, string password, OutputFormatType format) {
			return GetUserTimelineAsXML(userName, password, null, format);
		}

		/// <summary>
		/// Gets the user timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsXML(string userName, string password, string IDorScreenName) {
			return GetUserTimelineAsXML(userName, password, IDorScreenName, OutputFormatType.XML);
		}

		/// <summary>
		/// Gets the user timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsXML(string userName, string password) {
			return GetUserTimelineAsXML(userName, password, null);
		}

		/// <summary>
		/// Gets the user timeline as RSS.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsRSS(string userName, string password, string IDorScreenName) {
			return GetUserTimelineAsXML(userName, password, IDorScreenName, OutputFormatType.RSS);
		}

		/// <summary>
		/// Gets the user timeline as RSS.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsRSS(string userName, string password) {
			return GetUserTimelineAsXML(userName, password, OutputFormatType.RSS);
		}

		/// <summary>
		/// Gets the user timeline as atom.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsAtom(string userName, string password, string IDorScreenName) {
			return GetUserTimelineAsXML(userName, password, IDorScreenName, OutputFormatType.Atom);
		}

		/// <summary>
		/// Gets the user timeline as atom.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetUserTimelineAsAtom(string userName, string password) {
			return GetUserTimelineAsXML(userName, password, OutputFormatType.Atom);
		}
		#endregion

		#region Friends_Timeline
		/// <summary>
		/// Gets the friends timeline.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetFriendsTimeline(string userName, string password, OutputFormatType format) {
			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Friends_Timeline), GetFormatTypeString(format));

			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the friends timeline as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public string GetFriendsTimelineAsJSON(string userName, string password) {
			return GetFriendsTimeline(userName, password, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the friends timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsTimelineAsXML(string userName, string password, OutputFormatType format) {
			if (format == OutputFormatType.JSON) {
				throw new ArgumentException("GetFriendsTimelineAsXML supports only XML based formats (XML, RSS, Atom)", "format");
			}

			string output = GetFriendsTimeline(userName, password, format);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		/// <summary>
		/// Gets the friends timeline as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsTimelineAsXML(string userName, string password) {
			return GetFriendsTimelineAsXML(userName, password, OutputFormatType.XML);
		}

		/// <summary>
		/// Gets the friends timeline as RSS.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsTimelineAsRSS(string userName, string password) {
			return GetFriendsTimelineAsXML(userName, password, OutputFormatType.RSS);
		}

		/// <summary>
		/// Gets the friends timeline as atom.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsTimelineAsAtom(string userName, string password) {
			return GetFriendsTimelineAsXML(userName, password, OutputFormatType.Atom);
		}

		#endregion

		#region Friends

		/// <summary>
		/// Gets the friends.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetFriends(string userName, string password, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("GetFriends support only XML and JSON output format", "format");
			}

			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Friends), GetFormatTypeString(format));
			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the friends.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetFriends(string userName, string password, string IDorScreenName, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("GetFriends support only XML and JSON output format", "format");
			}

			string url = null;
			if (string.IsNullOrEmpty(IDorScreenName)) {
				url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Friends), GetFormatTypeString(format));
			}
			else {
				url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Friends) + "/" + IDorScreenName, GetFormatTypeString(format));
			}

			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the friends as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public string GetFriendsAsJSON(string userName, string password, string IDorScreenName) {
			return GetFriends(userName, password, IDorScreenName, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the friends as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public string GetFriendsAsJSON(string userName, string password) {
			return GetFriendsAsJSON(userName, password, null);
		}

		/// <summary>
		/// Gets the friends as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsAsXML(string userName, string password, string IDorScreenName) {
			string output = GetFriends(userName, password, IDorScreenName, OutputFormatType.XML);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		/// <summary>
		/// Gets the friends as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFriendsAsXML(string userName, string password) {
			return GetFriendsAsXML(userName, password, null);
		}

		#endregion

		#region Followers

		/// <summary>
		/// Gets the followers.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetFollowers(string userName, string password, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("GetFollowers supports only XML and JSON output format", "format");
			}

			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Followers), GetFormatTypeString(format));
			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the followers as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public string GetFollowersAsJSON(string userName, string password) {
			return GetFollowers(userName, password, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the followers as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFollowersAsXML(string userName, string password) {
			string output = GetFollowers(userName, password, OutputFormatType.XML);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		#endregion

		#region Update

		/// <summary>
		/// Updates the specified user name.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="status">The status.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string Update(string userName, string password, string status, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("Update support only XML and JSON output format", "format");
			}

			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Update), GetFormatTypeString(format));
			string data = string.Format("status={0}", HttpUtility.UrlEncode(status));

			return ExecutePostCommand(url, userName, password, data);
		}

		/// <summary>
		/// Updates as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public string UpdateAsJSON(string userName, string password, string text) {
			return Update(userName, password, text, OutputFormatType.JSON);
		}

		/// <summary>
		/// Updates as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="text">The text.</param>
		/// <returns></returns>
		public XmlDocument UpdateAsXML(string userName, string password, string text) {
			string output = Update(userName, password, text, OutputFormatType.XML);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		#endregion

		#region Featured

		/// <summary>
		/// Gets the featured.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string GetFeatured(string userName, string password, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("GetFeatured supports only XML and JSON output format", "format");
			}

			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Statuses), GetActionTypeString(ActionType.Featured), GetFormatTypeString(format));
			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Gets the featured as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public string GetFeaturedAsJSON(string userName, string password) {
			return GetFeatured(userName, password, OutputFormatType.JSON);
		}

		/// <summary>
		/// Gets the featured as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public XmlDocument GetFeaturedAsXML(string userName, string password) {
			string output = GetFeatured(userName, password, OutputFormatType.XML);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		#endregion

		#region Show

		/// <summary>
		/// Shows the specified user name.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public string Show(string userName, string password, string IDorScreenName, OutputFormatType format) {
			if (format != OutputFormatType.JSON && format != OutputFormatType.XML) {
				throw new ArgumentException("Show supports only XML and JSON output format", "format");
			}

			string url = string.Format(TwitterBaseUrlFormat, GetObjectTypeString(ObjectType.Users), GetActionTypeString(ActionType.Show) + "/" + IDorScreenName, GetFormatTypeString(format));
			return ExecuteGetCommand(url, userName, password);
		}

		/// <summary>
		/// Shows as JSON.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public string ShowAsJSON(string userName, string password, string IDorScreenName) {
			return Show(userName, password, IDorScreenName, OutputFormatType.JSON);
		}

		/// <summary>
		/// Shows as XML.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <param name="IDorScreenName">Name of the I dor screen.</param>
		/// <returns></returns>
		public XmlDocument ShowAsXML(string userName, string password, string IDorScreenName) {
			string output = Show(userName, password, IDorScreenName, OutputFormatType.XML);
			if (!string.IsNullOrEmpty(output)) {
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(output);

				return xmlDocument;
			}

			return null;
		}

		#endregion
	}
}
