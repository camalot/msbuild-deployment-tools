using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="ReleaseServiceSoap", Namespace="http://www.codeplex.com/services/ReleaseService/v1.0")]
public partial class ReleaseService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback CreateAReleaseOperationCompleted;
    
    private System.Threading.SendOrPostCallback CreateReleaseOperationCompleted;
    
    private System.Threading.SendOrPostCallback UploadReleaseFilesOperationCompleted;
    
    private System.Threading.SendOrPostCallback UploadTheReleaseFilesOperationCompleted;

		/// <summary>
		/// Initializes a new instance of the <see cref="ReleaseService"/> class.
		/// </summary>
		/// <remarks/>
    public ReleaseService() {
        this.Url = "https://www.codeplex.com/Services/ReleaseService.asmx";
    }
    
    /// <remarks/>
    public event CreateAReleaseCompletedEventHandler CreateAReleaseCompleted;
    
    /// <remarks/>
    public event CreateReleaseCompletedEventHandler CreateReleaseCompleted;
    
    /// <remarks/>
    public event UploadReleaseFilesCompletedEventHandler UploadReleaseFilesCompleted;
    
    /// <remarks/>
    public event UploadTheReleaseFilesCompletedEventHandler UploadTheReleaseFilesCompleted;

		/// <summary>
		/// Creates the A release.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		/// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.codeplex.com/services/ReleaseService/v1.0/CreateARelease", RequestNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", ResponseNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int CreateARelease(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool isDefaultRelease, string username, string password) {
        object[] results = this.Invoke("CreateARelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    isDefaultRelease,
                    username,
                    password});
        return ((int)(results[0]));
    }

		/// <summary>
		/// Begins the create A release.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		/// <remarks/>
    public System.IAsyncResult BeginCreateARelease(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool isDefaultRelease, string username, string password, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CreateARelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    isDefaultRelease,
                    username,
                    password}, callback, asyncState);
    }

		/// <summary>
		/// Ends the create A release.
		/// </summary>
		/// <param name="asyncResult">The async result.</param>
		/// <returns></returns>
		/// <remarks/>
    public int EndCreateARelease(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

		/// <summary>
		/// Creates the A release async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    public void CreateAReleaseAsync(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool isDefaultRelease, string username, string password) {
        this.CreateAReleaseAsync(projectName, name, description, releaseDate, status, showToPublic, isDefaultRelease, username, password, null);
    }

		/// <summary>
		/// Creates the A release async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="userState">State of the user.</param>
		/// <remarks/>
    public void CreateAReleaseAsync(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool isDefaultRelease, string username, string password, object userState) {
        if ((this.CreateAReleaseOperationCompleted == null)) {
            this.CreateAReleaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateAReleaseOperationCompleted);
        }
        this.InvokeAsync("CreateARelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    isDefaultRelease,
                    username,
                    password}, this.CreateAReleaseOperationCompleted, userState);
    }

		/// <summary>
		/// Called when [create A release operation completed].
		/// </summary>
		/// <param name="arg">The arg.</param>
    private void OnCreateAReleaseOperationCompleted(object arg) {
        if ((this.CreateAReleaseCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.CreateAReleaseCompleted(this, new CreateAReleaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }

		/// <summary>
		/// Creates the release.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="showOnHomePage">if set to <c>true</c> [show on home page].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		/// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.codeplex.com/services/ReleaseService/v1.0/CreateRelease", RequestNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", ResponseNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public int CreateRelease(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool showOnHomePage, bool isDefaultRelease, string username, string password) {
        object[] results = this.Invoke("CreateRelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    showOnHomePage,
                    isDefaultRelease,
                    username,
                    password});
        return ((int)(results[0]));
    }

		/// <summary>
		/// Begins the create release.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="showOnHomePage">if set to <c>true</c> [show on home page].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		/// <remarks/>
    public System.IAsyncResult BeginCreateRelease(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool showOnHomePage, bool isDefaultRelease, string username, string password, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("CreateRelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    showOnHomePage,
                    isDefaultRelease,
                    username,
                    password}, callback, asyncState);
    }

		/// <summary>
		/// Ends the create release.
		/// </summary>
		/// <param name="asyncResult">The async result.</param>
		/// <returns></returns>
		/// <remarks/>
    public int EndCreateRelease(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((int)(results[0]));
    }

		/// <summary>
		/// Creates the release async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="showOnHomePage">if set to <c>true</c> [show on home page].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    public void CreateReleaseAsync(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool showOnHomePage, bool isDefaultRelease, string username, string password) {
        this.CreateReleaseAsync(projectName, name, description, releaseDate, status, showToPublic, showOnHomePage, isDefaultRelease, username, password, null);
    }

		/// <summary>
		/// Creates the release async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
		/// <param name="releaseDate">The release date.</param>
		/// <param name="status">The status.</param>
		/// <param name="showToPublic">if set to <c>true</c> [show to public].</param>
		/// <param name="showOnHomePage">if set to <c>true</c> [show on home page].</param>
		/// <param name="isDefaultRelease">if set to <c>true</c> [is default release].</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="userState">State of the user.</param>
		/// <remarks/>
    public void CreateReleaseAsync(string projectName, string name, string description, string releaseDate, string status, bool showToPublic, bool showOnHomePage, bool isDefaultRelease, string username, string password, object userState) {
        if ((this.CreateReleaseOperationCompleted == null)) {
            this.CreateReleaseOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCreateReleaseOperationCompleted);
        }
        this.InvokeAsync("CreateRelease", new object[] {
                    projectName,
                    name,
                    description,
                    releaseDate,
                    status,
                    showToPublic,
                    showOnHomePage,
                    isDefaultRelease,
                    username,
                    password}, this.CreateReleaseOperationCompleted, userState);
    }

		/// <summary>
		/// Called when [create release operation completed].
		/// </summary>
		/// <param name="arg">The arg.</param>
    private void OnCreateReleaseOperationCompleted(object arg) {
        if ((this.CreateReleaseCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.CreateReleaseCompleted(this, new CreateReleaseCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }

		/// <summary>
		/// Uploads the release files.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.codeplex.com/services/ReleaseService/v1.0/UploadReleaseFiles", RequestNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", ResponseNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void UploadReleaseFiles(string projectName, string releaseName, ReleaseFile[] files, string username, string password) {
        this.Invoke("UploadReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    username,
                    password});
    }

		/// <summary>
		/// Begins the upload release files.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		/// <remarks/>
    public System.IAsyncResult BeginUploadReleaseFiles(string projectName, string releaseName, ReleaseFile[] files, string username, string password, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    username,
                    password}, callback, asyncState);
    }

		/// <summary>
		/// Ends the upload release files.
		/// </summary>
		/// <param name="asyncResult">The async result.</param>
		/// <remarks/>
    public void EndUploadReleaseFiles(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }

		/// <summary>
		/// Uploads the release files async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    public void UploadReleaseFilesAsync(string projectName, string releaseName, ReleaseFile[] files, string username, string password) {
        this.UploadReleaseFilesAsync(projectName, releaseName, files, username, password, null);
    }

		/// <summary>
		/// Uploads the release files async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="userState">State of the user.</param>
		/// <remarks/>
    public void UploadReleaseFilesAsync(string projectName, string releaseName, ReleaseFile[] files, string username, string password, object userState) {
        if ((this.UploadReleaseFilesOperationCompleted == null)) {
            this.UploadReleaseFilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadReleaseFilesOperationCompleted);
        }
        this.InvokeAsync("UploadReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    username,
                    password}, this.UploadReleaseFilesOperationCompleted, userState);
    }

		/// <summary>
		/// Called when [upload release files operation completed].
		/// </summary>
		/// <param name="arg">The arg.</param>
    private void OnUploadReleaseFilesOperationCompleted(object arg) {
        if ((this.UploadReleaseFilesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.UploadReleaseFilesCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }

		/// <summary>
		/// Uploads the release files.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="recommendedFileName">Name of the recommended file.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.codeplex.com/services/ReleaseService/v1.0/UploadTheReleaseFiles", RequestNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", ResponseNamespace="http://www.codeplex.com/services/ReleaseService/v1.0", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public void UploadTheReleaseFiles(string projectName, string releaseName, ReleaseFile[] files, string recommendedFileName, string username, string password) {
        this.Invoke("UploadTheReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    recommendedFileName,
                    username,
                    password});
    }

		/// <summary>
		/// Begins the upload the release files.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="recommendedFileName">Name of the recommended file.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="callback">The callback.</param>
		/// <param name="asyncState">State of the async.</param>
		/// <returns></returns>
		/// <remarks/>
    public System.IAsyncResult BeginUploadTheReleaseFiles(string projectName, string releaseName, ReleaseFile[] files, string recommendedFileName, string username, string password, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("UploadTheReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    recommendedFileName,
                    username,
                    password}, callback, asyncState);
    }

		/// <summary>
		/// Ends the upload the release files.
		/// </summary>
		/// <param name="asyncResult">The async result.</param>
		/// <remarks/>
    public void EndUploadTheReleaseFiles(System.IAsyncResult asyncResult) {
        this.EndInvoke(asyncResult);
    }

		/// <summary>
		/// Uploads the release files async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="recommendedFileName">Name of the recommended file.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <remarks/>
    public void UploadTheReleaseFilesAsync(string projectName, string releaseName, ReleaseFile[] files, string recommendedFileName, string username, string password) {
        this.UploadTheReleaseFilesAsync(projectName, releaseName, files, recommendedFileName, username, password, null);
    }

		/// <summary>
		/// Uploads the release files async.
		/// </summary>
		/// <param name="projectName">Name of the project.</param>
		/// <param name="releaseName">Name of the release.</param>
		/// <param name="files">The files.</param>
		/// <param name="recommendedFileName">Name of the recommended file.</param>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		/// <param name="userState">State of the user.</param>
		/// <remarks/>
    public void UploadTheReleaseFilesAsync(string projectName, string releaseName, ReleaseFile[] files, string recommendedFileName, string username, string password, object userState) {
        if ((this.UploadTheReleaseFilesOperationCompleted == null)) {
            this.UploadTheReleaseFilesOperationCompleted = new System.Threading.SendOrPostCallback(this.OnUploadTheReleaseFilesOperationCompleted);
        }
        this.InvokeAsync("UploadTheReleaseFiles", new object[] {
                    projectName,
                    releaseName,
                    files,
                    recommendedFileName,
                    username,
                    password}, this.UploadTheReleaseFilesOperationCompleted, userState);
    }
    
    private void OnUploadTheReleaseFilesOperationCompleted(object arg) {
        if ((this.UploadTheReleaseFilesCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.UploadTheReleaseFilesCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <summary>
/// 
/// </summary>
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.codeplex.com/services/ReleaseService/v1.0")]
public partial class ReleaseFile {

	/// <summary>
	/// Gets or sets the file data.
	/// </summary>
	/// <value>The file data.</value>
	/// <remarks/>
	[System.Xml.Serialization.XmlElementAttribute ( DataType = "base64Binary" )]
	public byte[] FileData { get; set; }

	/// <summary>
	/// Gets or sets the name of the file.
	/// </summary>
	/// <value>The name of the file.</value>
	/// <remarks/>
	public string FileName { get; set; }

	/// <summary>
	/// Gets or sets the type of the file.
	/// </summary>
	/// <value>The type of the file.</value>
	/// <remarks/>
	public string FileType { get; set; }

	/// <summary>
	/// Gets or sets the type of the MIME.
	/// </summary>
	/// <value>The type of the MIME.</value>
	/// <remarks/>
	public string MimeType { get; set; }

	/// <summary>
	/// Gets or sets the name.
	/// </summary>
	/// <value>The name.</value>
	/// <remarks/>
	public string Name { get; set; }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
public delegate void CreateAReleaseCompletedEventHandler(object sender, CreateAReleaseCompletedEventArgs e);

/// <summary>
/// 
/// </summary>
/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CreateAReleaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateAReleaseCompletedEventArgs"/> class.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="cancelled">if set to <c>true</c> [cancelled].</param>
		/// <param name="userState">State of the user.</param>
    internal CreateAReleaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>The result.</value>
		/// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
public delegate void CreateReleaseCompletedEventHandler(object sender, CreateReleaseCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class CreateReleaseCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;

		/// <summary>
		/// Initializes a new instance of the <see cref="CreateReleaseCompletedEventArgs"/> class.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="exception">The exception.</param>
		/// <param name="cancelled">if set to <c>true</c> [cancelled].</param>
		/// <param name="userState">State of the user.</param>
    internal CreateReleaseCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>The result.</value>
		/// <remarks/>
    public int Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((int)(this.results[0]));
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
public delegate void UploadReleaseFilesCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
public delegate void UploadTheReleaseFilesCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
