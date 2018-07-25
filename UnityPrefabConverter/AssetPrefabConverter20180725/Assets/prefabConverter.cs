using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson ;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;
using Photon;
using System.Net;
using System.Text;


public class prefabConverter : MonoBehaviour {

	public string serverUrl;

	public string deleteUrl;

	private string resCode = "0";
	private string resMsg = "";
	private byte[] bData = null;
	private string strpath;
	private string targetPath;
	public string serverAddress="";
 	private bool isFinished = false;


	//http://175.116.52.231:8080/dongwoo/files/assets/

	public string bundleUploadPath = ""; 
	private string projectFolderNo = "";
	
	private char[] delimiterChars = { '-' };

	private int bufferLength = 2048;

	public string uploadUrl = "/api/bundleFileSave.api";

	// server file upload
	void Awake(){
		
		//test
		//serverAddress="http://175.116.52.231:8080/dongwoo/";

		StartCoroutine(BundleFileUpload());				
    }
	

	IEnumerator BundleFileUpload(){
		Debug.Log("server file upload start");
		DirectoryInfo di = new DirectoryInfo(Application.dataPath+"/AssetBundles");
		FileInfo[] fi = di.GetFiles("*.*");
		if(fi.Length>0){
			foreach(FileInfo f in fi){
				//Debug.Log("f.FullName : " + f.FullName); // full file path
				//Debug.Log("f.Name : " + f.Name);
				string[] str = f.Name.Split(delimiterChars);
				projectFolderNo = str[0]; // 프로젝트 폴더 번호 
				//Debug.Log("projectFolderNo : " + projectFolderNo);
				//bundleUploadPath = "175.116.52.231:8080/dongwoo/files/assets/";
				bundleUploadPath = serverAddress + "files/assets/";					
				bundleUploadPath = bundleUploadPath+projectFolderNo;
				Debug.Log("bundleUploadPath : " + bundleUploadPath);

				// api upload
				WWW localFile = new WWW("file://"+f.FullName);
				yield return localFile;
				//Debug.Log("localFile.size : " + localFile.size);
				 if (localFile.error == null)
           			 Debug.Log("local file Loaded successfully");
        		else{
					Debug.Log("Open file error: " + localFile.error);
					yield break; // stop the coroutine here
				}
				
				WWWForm postForm = new WWWForm();
				postForm.AddBinaryData("bundleFile",localFile.bytes);
				postForm.AddField("fileName",f.Name);
				postForm.AddField("projectFolderNo",projectFolderNo);				

				uploadUrl = serverAddress+"/api/bundleFileSave.api";

        		WWW upload = new WWW(uploadUrl, postForm);
        		yield return upload;
				 if (upload.error == null)
            		Debug.Log("upload done :" + upload.text);
        		else
            		Debug.Log("Error during upload: " + upload.error);

				// 업로드 파일 삭제 
				f.Delete();								

			}//.foreach					
		}//.if(fi.Length>0)

		Debug.Log ("prefab delete");
		DirectoryInfo di2 = new DirectoryInfo("Assets/Resources/");
		FileInfo[] fi2 = di2.GetFiles("*.prefab");
		if (fi2.Length > 0) {
			foreach (FileInfo prefab in fi2) {
				Debug.Log (prefab.Name);
				prefab.Delete();								
			}
		}

	    yield return null;
	}//.BundleFileUpload

	// Use this for initialization
	void Start () {
		Debug.Log("prefab converter start");
					
		serverUrl = serverAddress+ "api/asseToPrefabtList.api";
		deleteUrl =serverAddress + "api/deleteAsseToPrefab.api";
		//serverUrl = "http://localhost:8080/api/asseToPrefabtList.api";
		//deleteUrl = "http://localhost:8080/api/deleteAsseToPrefab.api";
		targetPath = Application.dataPath+"/Resources/";
		//Debug.Log("targetPath : " + targetPath);
		
		StartCoroutine (PrefabConverting());		

		//TestModelInstantiate ();

	}


//	void TestModelInstantiate(){
//		string assetBundleDirectory = Application.streamingAssetsPath+"/";
//		// test
//		AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(assetBundleDirectory+"190-0.unity3d");
//
//		if (myLoadedAssetBundle == null){
//			Debug.Log("Failed to load AssetBundle!");            
//		}else
//			Debug.Log("Successed to load AssetBundle!");
//
//		string[] asnms = myLoadedAssetBundle.GetAllAssetNames();
//
//		// Debug.Log("asnms.size : " + asnms.Length);
//		// Debug.Log("asnms[0].ToString() : " + asnms[0].ToString());
//
//		foreach(string a in asnms){            
//			//Debug.Log("########### : " + a.ToString());
//
//			GameObject obj = (GameObject)Instantiate(myLoadedAssetBundle.LoadAsset(a));
//
//
//		}        
//		myLoadedAssetBundle.Unload(false);
//	}


	IEnumerator PrefabConverting(){
		
		while(true){

			Debug.Log("prefab converter running...");
						
			WWWForm form = new WWWForm ();
			WWW request = new WWW (serverUrl);			
			yield return request;			
			//Debug.Log ("request :"+request.text);
			
			//request.
			JsonData jsonData = JsonMapper.ToObject (request.text);
			resCode = jsonData["resCode"].ToString();
			resMsg = jsonData["resMsg"].ToString();
			
			//Debug.Log ("projectAssetToPrefab Count:"+jsonData["projectAssetToPrefab"].Count);
			
			if(jsonData["projectAssetToPrefab"].Count>0){
				
				for(int i=0;i<jsonData["projectAssetToPrefab"].Count;i++){
					

					strpath = jsonData["projectAssetToPrefab"][i]["asset_file_url"].ToString();					
					//string subStr = strpath.Replace("/usr/local/apache-tomcat-8.0.41/webapps/","");
					Debug.Log(strpath);

					string allFileName = jsonData["projectAssetToPrefab"][i]["asset_file_name"].ToString();
					int dotPos = allFileName.LastIndexOf(".");
					string fileName = allFileName.Substring(0,dotPos);
					//strpath = "file://"+ strpath;
					
					// WWW g_gWWW = new WWW(strpath);
					// yield return g_gWWW;
					// bData = g_gWWW.bytes;				
					
					// string szPath = Application.dataPath;				
					//bData = File.ReadAllBytes(serverAddress+subStr);
					//Debug.Log("bData.Length : " + bData.Length);
					
					
					// 파일카피					
					Debug.Log(serverAddress+strpath);
					
					UnityWebRequest downRequest = UnityWebRequest.Get(serverAddress+strpath);
					yield return downRequest.Send();						
					//System.IO.File.Copy(serverAddress+subStr, targetPath + jsonData["projectAssetToPrefab"][i]["asset_file_name"], true);
					FileStream fs = new FileStream (targetPath+"/"+allFileName,System.IO.FileMode.Create);
					fs.Write (downRequest.downloadHandler.data,0,(int)downRequest.downloadedBytes);
					fs.Close ();

					
					// 에디터 갱신 
					#if UNITY_EDITOR
					UnityEditor.AssetDatabase.Refresh();
					#endif
					//Debug.Log(jsonData["projectAssetToPrefab"][i]["asset_file_name"]);
					
					//Debug.Log(targetPath);
					GameObject loadObj;
					GameObject instObj;
					GameObject createdPrefabObj;
					bool catchFlag = false;

					try{
						loadObj = (GameObject) Resources.Load(fileName);	// Do something that can throw an exception		
					}
					catch(System.Exception e){
						Debug.LogError (e.ToString());
						request = new WWW (deleteUrl+"?asset_key="+jsonData["projectAssetToPrefab"][i]["asset_key"]);
						catchFlag = true;
						yield break;
					}

					
					loadObj = (GameObject) Resources.Load(fileName);	// Do something that can throw an exception						
					instObj = GameObject.Instantiate(loadObj);


					string savePath  = "Assets/Resources/"+fileName+".prefab";
					#if UNITY_EDITOR					
					createdPrefabObj = UnityEditor.PrefabUtility.CreatePrefab(savePath,instObj);	


					Destroy(instObj);
					UnityEditor.AssetDatabase.Refresh();


					// 어셋번들 만들기										
					string assetPath = AssetDatabase.GetAssetPath(createdPrefabObj.GetInstanceID());	

					// 번들 이름 설정	ex 프로젝트키 - 에셋 depth - modelName ex 폴더 자체에도 번들네임 설정 가능
					string bundleNm = jsonData["projectAssetToPrefab"][i]["project_key"]+"-"+jsonData["projectAssetToPrefab"][i]["asset_depth"];
					if(jsonData["projectAssetToPrefab"][i]["asset_depth"].ToString ().Equals ("1")){ // user model 
						bundleNm = jsonData["projectAssetToPrefab"][i]["project_key"]+"-"+jsonData["projectAssetToPrefab"][i]["asset_depth"]+"-"+fileName;
					}

					if(jsonData["projectAssetToPrefab"][i]["build_target"].ToString ().Equals("android")){
						Debug.Log("android name set");
						bundleNm += "-android";
					}

					if(jsonData["projectAssetToPrefab"][i]["build_target"].ToString ().Equals("ios")){
						Debug.Log("ios name set");
						bundleNm += "-ios";
					}


					AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(bundleNm,"unity3d");  					


					// ASSET_TO_PREFAB 디비에서 삭제 
					form = new WWWForm ();
					request = new WWW (deleteUrl+"?asset_key="+jsonData["projectAssetToPrefab"][i]["asset_key"]);
					yield return request;


					// on/off trigger
					if(jsonData["projectAssetToPrefab"][i]["build_target"].ToString ().Equals("android")){
						TrigerEditorMenuBuildBundleAndroid();
					}else if(jsonData["projectAssetToPrefab"][i]["build_target"].ToString ().Equals("ios")){
						TrigerEditorMenuBuildBundleIos();
					}else{
						TrigerEditorMenuBuildBundle();
					}

					#endif
				
				

					
					// 서버에 업로드 (유니티-포메터, 는 다른 컴퓨터에 심는다. ) 
					// string oriPath = Application.dataPath+"/Resources/"+fileName+".prefab"; 						
					// string subPath = jsonData["projectAssetToPrefab"][i]["asset_file_path"].ToString();
					// string copyPath = subPath.Replace(jsonData["projectAssetToPrefab"][i]["asset_file_name"].ToString(),"") + "/"+fileName+".prefab";						
					// Debug.Log("oriPath : " + oriPath);
					// Debug.Log("copyPath : " + copyPath);
					//System.IO.File.Copy(oriPath, copyPath, true);


					//Debug.Log ("request :"+request.text);

								
				}//.for			

//				#if UNITY_EDITOR
//				TrigerEditorMenuBuildBundle();
//				#endif
			}//.if			
			yield return new WaitForSeconds(1.0f);
		}//.while
	}//.coroutine

	//public static void Refresh(ImportAssetOptions options = ImportAssetOptions.Default);

	void OnApplicationQuit(){		
		Debug.Log("prefab converter stopped");
	}

	void TrigerEditorMenuBuildBundle(){
		isFinished = true;		
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		EditorApplication.ExecuteMenuItem("Tests/Show Window");
		 
		#endif
	}
	void TrigerEditorMenuBuildBundleAndroid(){
		isFinished = true;		
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		EditorApplication.ExecuteMenuItem("Tests/Show Window Android");

		#endif
	}

	void TrigerEditorMenuBuildBundleIos(){
		isFinished = true;		
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		EditorApplication.ExecuteMenuItem("Tests/Show Window ios");

		#endif
	}


	
}//.class

	// void Update(){
	// 	if(isFinished){
	// 		#if UNITY_EDITOR
	// 		UnityEditor.EditorApplication.isPlaying = false;
	// 		#endif
	// 	 }
	// }

	// ftp upload
	// FtpWebRequest requestFTPUploder = (FtpWebRequest) WebRequest.Create("ftp://"+bundleUploadPath+"/"+f.Name);
	// requestFTPUploder.Credentials = new NetworkCredential("ubuntu14","1234qwer");
	// requestFTPUploder.KeepAlive = false;
	// requestFTPUploder.UseBinary = true;
	// requestFTPUploder.Method = WebRequestMethods.Ftp.UploadFile;
	// FileStream fileStream = f.OpenRead();
	// byte[] buffer = new byte[bufferLength];
	// Stream uploadStream = requestFTPUploder.GetRequestStream();
	// int contentLength = fileStream.Read(buffer,0,bufferLength);
	// while(contentLength != 0){
	// 	uploadStream.Write(buffer,0,contentLength);
	// 	contentLength = fileStream.Read(buffer,0,bufferLength);
	// }
	// uploadStream.Close();
	// fileStream.Close();
	// requestFTPUploder = null;
	// Debug.Log("bundle upload complete");