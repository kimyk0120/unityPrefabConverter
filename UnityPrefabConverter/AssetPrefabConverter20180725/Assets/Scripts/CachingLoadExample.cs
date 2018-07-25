using System;
using UnityEngine;
using System.Collections;

public class CachingLoadExample : MonoBehaviour {
    public string BundleURL ="";
    //public string BundleURL ="file:///Users/youngkwangkim/unity-project/AssetPrefabFommater/Assets/AssetBundles/AssetBundles";
    //http://175.116.52.231:8080/resources/assetbundlehan1/143-0.uniyt3d
    //file:///Users/youngkwangkim/Downloads/aseetbundle/145-0
    public string AssetName;
    public int version = 0;

    void Start() {
        StartCoroutine (DownloadAndCache());        
    }

    IEnumerator DownloadAndCache (){
        // Wait for the Caching system to be ready
        while (!Caching.ready){
            yield return null;
        }

        WWW www = WWW.LoadFromCacheOrDownload(this.BundleURL,this.version);
        yield return www;
        if(www.error != null){
            Debug.Log("fail");
        }else{
            Debug.Log("cache down");
            AssetBundle bundle = www.assetBundle;
            
            // check
            string[] asnms = bundle.GetAllAssetNames();
            foreach(string a in asnms){
                Debug.Log(a.ToString());
                
                Instantiate(bundle.LoadAsset(a));
                
            }
            
            
            //\bundle.mainAsset;

            // finally
            //bundle.Unload(false);
            // www.Dispose();
        }        
    }
}//.class