using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;


using SOT_add;
using SOT_align;
using SOT_duplicate;
using SOT_group;
using SOT_info;
using SOT_lib;
using SOT_replace;
using SOT_toSurface;

public class ObjectsTools : EditorWindow {
	int activeToolbar = 0;
	int lastActiveToolbar = 0;
	int uiMode = -1;
	Texture2D btAdd = null;
	Texture2D btPlace = null;
	Texture2D btInfo = null;
	Texture2D btAlign = null;
	Texture2D btClone = null;
	Texture2D btReplace = null;
	Texture2D btHierarchy = null;
	GUIContent[] toolbarImages = null;

	GameObject projectActiveSelection;
	GameObject sceneActiveSelection;
	GameObject[] projectSelection;
	GameObject[] sceneSelection;
			
	[MenuItem ("Window/Objects Tools")]

	public static void init ()
	{
		ObjectsTools window = (ObjectsTools)EditorWindow.GetWindow(typeof(ObjectsTools));
		window.titleContent = new GUIContent("Objects Tools");
		window.Show();
		window.minSize = new Vector2 (300f, 120f);
	}
	void OnInspectorUpdate() { Repaint(); }
	void OnEnable() { SceneView.onSceneGUIDelegate += SceneGUI; }
	void OnDisable() { SceneView.onSceneGUIDelegate -= SceneGUI; }

	// Detect mouse events in the scene
	private void SceneGUI( SceneView sceneview )
	{
		//bool cancelEvent = false;
		Event e = Event.current;
		if (activeToolbar == 1) { // Add
			if(SOT_add.lib.editorMouseEvent (e, projectActiveSelection)) {
				//cancelEvent = true;
				HandleUtility.AddDefaultControl (GUIUtility.GetControlID (GetHashCode (), FocusType.Passive));
			}
			SOT_add.lib.sceneGUI ();
		}
		if (activeToolbar == 4) { // Clone
			SOT_duplicate.lib.sceneGUI ();

		}
		HandleUtility.Repaint ();
		if (activeToolbar != lastActiveToolbar) {
			HandleUtility.Repaint ();
			lastActiveToolbar = activeToolbar;
		}
			
	}	
			


	void OnGUI ()
	{
		int width = Screen.width;
		int vpos = 10;

		if (btAdd == null || uiMode !=( EditorGUIUtility.isProSkin ? 1 : 0)) {
			btAdd = LoadTextureFromFile ("iSOT_add" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btPlace = LoadTextureFromFile ("iSOT_toSurface" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btInfo = LoadTextureFromFile ("iSOT_info" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btAlign = LoadTextureFromFile ("iSOT_align" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btClone = LoadTextureFromFile ("iSOT_duplicate" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btReplace = LoadTextureFromFile ("iSOT_replace" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");
			btHierarchy = LoadTextureFromFile ("iSOT_group" +(EditorGUIUtility.isProSkin ? "_w":"_b")+ ".png");

			GUIContent btHierarchyContent = new GUIContent("", btHierarchy, "");
			GUIContent btAddContent = new GUIContent("", btAdd, "");
			GUIContent btPlaceContent = new GUIContent("", btPlace, "");
			GUIContent btInfoContent = new GUIContent("", btInfo, "");
			GUIContent btCloneContent = new GUIContent("", btClone, "");
			GUIContent btReplaceContent = new GUIContent("", btReplace, "");
			GUIContent btAlignContent = new GUIContent("", btAlign, "");

			toolbarImages = new GUIContent[] { 
				btHierarchyContent, btAddContent, btPlaceContent, btAlignContent, btCloneContent, btReplaceContent, btInfoContent
			};

			uiMode = EditorGUIUtility.isProSkin ? 1 : 0;
		}

		if (Selection.activeGameObject) {
			if (AssetDatabase.Contains (Selection.activeGameObject)) {
				projectSelection = Selection.gameObjects;
				projectActiveSelection = Selection.activeGameObject;
			} else {
				sceneSelection = Selection.gameObjects;
				sceneActiveSelection = Selection.activeGameObject;
			}
		} else {
			sceneSelection = null;
			sceneActiveSelection = null;
		}

		GUIStyle styleInfoText = new GUIStyle(GUI.skin.label);
		styleInfoText.wordWrap = true;

		float barWidth = 7 * 35;
		if(barWidth > width - 15) barWidth = width - 15;
		activeToolbar = GUI.Toolbar(new Rect(width / 2 - barWidth / 2, vpos, barWidth, 24), activeToolbar, toolbarImages);
		vpos += 40;

		if (activeToolbar == 0) SOT_group.lib.renderGUI (vpos, sceneSelection);
		if (activeToolbar == 1) SOT_add.lib.renderGUI (vpos, projectActiveSelection);
		if (activeToolbar == 2) SOT_toSurface.lib.renderGUI (vpos, sceneActiveSelection);
		if (activeToolbar == 3) SOT_align.lib.renderGUI (vpos, sceneSelection);
		if (activeToolbar == 4) SOT_duplicate.lib.renderGUI (vpos, sceneSelection, sceneActiveSelection);
		if (activeToolbar == 5) SOT_replace.lib.replaceRenderGUI (vpos, sceneSelection, projectActiveSelection);
		if (activeToolbar == 6) SOT_info.lib.renderGUI (vpos, sceneSelection, projectSelection);
	}


	Texture2D LoadTextureFromFile (string filename){
		string textureURL = "file://" + Application.dataPath + "/ObjectsTools/Editor/" + filename;
		WWW www = new WWW(textureURL);
		Texture2D tempTexture = new Texture2D(16, 16);
		www.LoadImageIntoTexture(tempTexture);
		return(tempTexture);
	}

}
