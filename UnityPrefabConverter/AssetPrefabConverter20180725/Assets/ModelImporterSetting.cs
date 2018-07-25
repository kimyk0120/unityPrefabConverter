using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif 
#if UNITY_EDITOR
public class MeshPostProcessing : AssetPostprocessor {
 
    void OnPreprocessModel(){  // 모델파일이 임포트 될 때 호출 
		Debug.Log("OnPreprocessModel called");
 
        ModelImporter modImport = assetImporter as ModelImporter; 
        modImport.addCollider = true; // generate colliders auto check true 

    }     
}//.class
#endif 