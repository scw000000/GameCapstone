using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SOT_lib;

namespace SOT_group {
	public class lib : MonoBehaviour {
		public static string groupName = "New group";
		public static void renderGUI(int vpos, GameObject[] sceneSelection)
		{
			int width = Screen.width;

			GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
			centeredStyle.alignment = TextAnchor.MiddleCenter;

			if (sceneSelection != null) {
				vpos += SOT_lib.SHUX.header ("<b>Group Objects</b>\nGroup selected objects under a newly created game object.", vpos, true);

				if(width < 300) {
					GUI.Label (new Rect (10, vpos, width - 20, 20), "Group name");
					vpos += 15;
					groupName = GUI.TextField(new Rect(10, vpos, width - 20, 20), groupName, 25);
				} else {
					GUI.Label (new Rect (10, vpos, width / 3, 20), "Group name");
					groupName = GUI.TextField(new Rect(width / 3 + 10, vpos, width - width / 3 - 20, 20), groupName, 25);
				}

				vpos += 30;//height - vpos - 65 > 0 ? 40 : height - vpos - 65;

				// GROUP
				int btWidth = width < 160 ? width - 20 : 160;
				if (GUI.Button (new Rect (width / 2 - btWidth / 2, vpos, btWidth, 25), "Group")) {
					GameObject goGroup = new GameObject (groupName);
					goGroup.transform.parent = Selection.activeGameObject.transform.parent;
					Undo.RegisterCreatedObjectUndo (goGroup, "Group objects");
					
					foreach (GameObject GO in Selection.gameObjects) {
						// Only direct children to preserve hierarchy
						if (GO.transform.parent == goGroup.transform.parent) {
							Undo.SetTransformParent (GO.transform, goGroup.transform, "Group objects");
						}
					}
					
					var theParent = new List<GameObject> (1);
					theParent.Add (goGroup);
					Selection.objects = theParent.ToArray ();
				}
			} else {
				SOT_lib.SHUX.alertBox ("Grouping", "Select objects in the hierarchy or in the scene view to enable this tool.");
			}
		}
	}
}

