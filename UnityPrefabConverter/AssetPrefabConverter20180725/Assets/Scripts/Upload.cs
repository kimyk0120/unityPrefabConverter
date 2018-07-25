using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upload : MonoBehaviour {

	public string  UploadFilePath = "///Users/youngkwangkim/unity-project/Asset Bundle Test/Assets/AssetBundles/";
	public string  uploadURL;
	public string name;

	// Use this for initialization
    public void ClickTest(){
			Debug.Log("Click Test");
			Uploading(UploadFilePath, uploadURL, GetName());
			//Debug.Log("Click Test2");
	}


	void Start ()
    {
        //Uploading(UploadFilePath, uploadURL, GetName());
	
    }

    private string GetName()
    {
        return name;
    }

    // Update is called once per frame
    void Update () {
		
	}


	IEnumerator Uploading(string UploadFilePath,string uploadURL,string name)
	{
		Debug.Log("upload");
		WWW localFile = new WWW("file:///" +UploadFilePath );
		yield return localFile;
		WWWForm postForm = new WWWForm();
		postForm.AddBinaryData(name, localFile.bytes, UploadFilePath,"Upload");
		localFile.Dispose();

		WWW upload = new WWW(uploadURL, postForm);		// 업로드

		yield return upload;

		if (upload.error == null)
		{

			print ("업로드 완료");
			Debug.Log ("upload error null");
		
			upload.Dispose();
		}
		else
		{
			Debug.Log("Error during upload: " + upload.error);
		}
	}

}//endClass
