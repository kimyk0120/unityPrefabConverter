namespace UnityEditor{


	public class CreateNewAssetBundle{

#if UNITY_5		
		[MenuItem("Tests/Create/Create New")]
		public static void Create(){
			//BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);		
			BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);		
		}

		[MenuItem("Tests/Create/Create New Android")]
		public static void CreateAndroid(){
			//BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);		
			BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);		
		}

		[MenuItem("Tests/Create/Create New ios")]
		public static void CreateIos(){
			//BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);		
			BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.iOS);		
		}

# else
		[MenuItem("Tests/Create/Create Old")]
		public static void Create2(){
			BuildPipeline.PushAssetDependencies();
			var assets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets/Snapshot/1.png");
			BuildAssetBundleOptions buildAssetOptions = BuildAssetBundleOptions.CollectDependencies;		
			BuildPipeline.BuildAssetBundle(assets[0], assets, "Assets/Snapshot/1a.unity3d", buildAssetOptions, EditorUserBuildSettings.activeBuildTarget);
		}
# endif
		
	}
	
	public class SimpleRecorder : EditorWindow {
		int count = 0;
		string countStr = "0";
		static bool isRunning = false;
		static bool isAndroid = false;
		static bool isIos = false;
		
		void Update(){
			count = count+1;
			
			
			if((EditorApplication.isPlaying == false)&&(isRunning == true)&&(isAndroid == false)&&(isIos == false)){			
				// play mode stopped
				isRunning = false;
				#if UNITY_5
				EditorApplication.ExecuteMenuItem("Tests/Create/Create New");
				# else
				EditorApplication.ExecuteMenuItem("Tests/Create/Create Old");
				# endif
				EditorApplication.ExecuteMenuItem("Tests/Hide Window");
				AssetDatabase.Refresh();
				EditorApplication.ExecuteMenuItem("Edit/Play");
			}

			if((EditorApplication.isPlaying == false)&&(isRunning == true)&&(isAndroid == true)&&(isIos == false)){			
				// play mode stopped
				isRunning = false;
				#if UNITY_5
				EditorApplication.ExecuteMenuItem("Tests/Create/Create New Android");
				# else
				EditorApplication.ExecuteMenuItem("Tests/Create/Create Old");
				# endif
				EditorApplication.ExecuteMenuItem("Tests/Hide Window");
				AssetDatabase.Refresh();
				EditorApplication.ExecuteMenuItem("Edit/Play");
			}

			if((EditorApplication.isPlaying == false)&&(isRunning == true)&&(isAndroid == false)&&(isIos == true)){			
				// play mode stopped
				isRunning = false;
				#if UNITY_5
				EditorApplication.ExecuteMenuItem("Tests/Create/Create New ios");
				# else
				EditorApplication.ExecuteMenuItem("Tests/Create/Create Old");
				# endif
				EditorApplication.ExecuteMenuItem("Tests/Hide Window");
				AssetDatabase.Refresh();
				EditorApplication.ExecuteMenuItem("Edit/Play");
			}

		}
		
		[MenuItem("Tests/Show Window")]
		static void ShowW() {
			SimpleRecorder window  = (SimpleRecorder)EditorWindow.GetWindow(typeof(SimpleRecorder), true, "Asset Bundle Builder");
			window.Show();
			isRunning = true;
			isAndroid = false;
			isIos = false;
		}

		[MenuItem("Tests/Show Window Android")]
		static void ShowWAndroid() {
			SimpleRecorder window  = (SimpleRecorder)EditorWindow.GetWindow(typeof(SimpleRecorder), true, "Asset Bundle Builder");
			window.Show();
			isRunning = true;
			isAndroid = true;
			isIos = false;
		}


		[MenuItem("Tests/Show Window ios")]
		static void ShowWIos() {
			SimpleRecorder window  = (SimpleRecorder)EditorWindow.GetWindow(typeof(SimpleRecorder), true, "Asset Bundle Builder");
			window.Show();
			isRunning = true;
			isAndroid = false;
			isIos = true;
		}

		
		[MenuItem("Tests/Hide Window")]
		static void HideW() {
			SimpleRecorder window  = (SimpleRecorder)EditorWindow.GetWindow(typeof(SimpleRecorder), true, "Asset Bundle Builder");
			window.Close();
		}
		
		void OnGUI () {
			countStr = EditorGUILayout.TextField (count.ToString(),countStr);
			EditorGUILayout.LabelField ("Status: ", count.ToString());
		}
	}
	
}//.class