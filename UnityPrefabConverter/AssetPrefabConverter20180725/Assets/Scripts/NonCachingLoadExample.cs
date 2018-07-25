using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;


public class NonCachingLoadExample : MonoBehaviour {

//    public string BundleURL ="file:///Users/youngkwangkim/unity-project/Asset%20Bundle%20Test/Assets/AssetBundles/";
//    public string AssetName;
//    IEnumerator Start() {
//      // Download the file from the URL. It will not be saved in the Cache
//      using (WWW www = new WWW(BundleURL)) {
//          yield return www;
//          if (www.error != null)
//              throw new Exception("WWW download had an error:" + www.error);
//          AssetBundle bundle = www.assetBundle;

//          string[] asd = bundle.GetAllAssetNames();

//          for(int i=0;i<asd.Length;i++){
//              if(asd[i] == null) Debug.Log("null");
//              else Debug.Log(asd[i]);
//          }

//         //  if (AssetName == "")
//         //      Instantiate(bundle.mainAsset);
//         //  else
//         //      Instantiate(bundle.LoadAsset(AssetName));
//         //            // Unload the AssetBundles compressed contents to conserve memory
//         //            bundle.Unload(false);

//      } // memory is freed from the web stream (www.Dispose() gets called implicitly)
//    }

    IEnumerator Start()
    {
        Debug.Log("in");
        //string uri = "file:///" + Application.dataPath + "/AssetBundles/" + assetbundle_0;
        string uri = "file:///Users/youngkwangkim/unity-project/Asset%20Bundle%20Test/Assets/AssetBundles/assetbundle_0";
        

        UnityWebRequest request = UnityWebRequest.GetAssetBundle(uri, 0);
        yield return request.Send();

        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        Debug.Log(bundle);
        GameObject cube = bundle.LoadAsset<GameObject>("Cube 1");
        //GameObject sprite = bundle.LoadAsset<GameObject>("Sprite");
        
        GameObject obj = Instantiate(cube);
        obj.transform.position = new Vector3(0,0,0);
        //Instantiate(sprite);
        Debug.Log("out");
    }





}//endClass
