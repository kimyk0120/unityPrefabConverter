using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class NewBehaviourScript : MonoBehaviour {

	IEnumerator LoadAssetFromLocalDisk()
{
string assetBundleDirectory = "Assets/AssetBundles";
// 저장한 에셋 번들로부터 에셋 불러오기
var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(assetBundleDirectory + "/", "character.unity3d"));
if (myLoadedAssetBundle == null)
{
Debug.Log("Failed to load AssetBundle!");
yield break;
}
else
Debug.Log("Successed to load AssetBundle!");

var prefab = myLoadedAssetBundle.LoadAsset<GameObject>("P_C0001");
Instantiate(prefab, Vector3.zero, Quaternion.identity);
}


}
