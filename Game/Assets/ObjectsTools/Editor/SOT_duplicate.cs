using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SOT_lib;

namespace SOT_duplicate {
	public class lib : MonoBehaviour {
		public static GameObject[] sceneSelection;
		public static Vector2 scrollPosition = Vector2.zero;
		public static bool countXenabled = true;
		public static bool countYenabled = false;
		public static bool countZenabled = false;
		public static float spacing = 0;
		public static Vector3 v3;
		public static GameObject sceneActiveSelection;

		public static Vector3[] clones;
		public static void sceneGUI () {
			if (sceneActiveSelection != null && sceneSelection.Length == 1) {
				Vector3 Vsize = SOT_lib.SHUX.getAllBounds (sceneActiveSelection).size;
				float size = Vsize.x > Vsize.y ? (Vsize.x > Vsize.z ? Vsize.x : Vsize.z) : (Vsize.y > Vsize.z ? Vsize.y : Vsize.z);
				v3 = Handles.PositionHandle(v3, Quaternion.identity);
				float d = Vector3.Distance(v3,sceneActiveSelection.transform.position);
				d = d + spacing * d;
				int count = (int)(d / size); count = count < 1 ? 1 : count;
				clones = new Vector3[(int)count];
				for (float ptrX = 0f; ptrX < count; ptrX++) {
					Vector3 e = Vector3.Lerp (sceneActiveSelection.transform.position, v3, ptrX / count);
					clones[(int)ptrX] = v3 - e;
					SOT_lib.SHUX.draft (sceneActiveSelection, v3 - e, Color.yellow);
				}
				Handles.color = Color.green;
				Handles.DrawDottedLine (v3, sceneActiveSelection.transform.position, 5);
			}
		}

		public static void renderGUI(int vpos, GameObject[] get_sceneSelection, GameObject get_sceneActiveSelection)
		{
			if(get_sceneActiveSelection && sceneActiveSelection != get_sceneActiveSelection) v3 = get_sceneActiveSelection.transform.position + new Vector3(10,0,0);
			sceneSelection = get_sceneSelection;

			int width = Screen.width;
			sceneActiveSelection = get_sceneActiveSelection;

			if (get_sceneActiveSelection != null) {
				if(sceneSelection.Length != 1) {
					SOT_lib.SHUX.alertBox ("Duplicate", "Select one single object in the hierarchy to enable this tool. Multiple objects are currently selected.");
				} else {
					vpos += SOT_lib.SHUX.header ("<b>Duplicate</b>\nDuplicate the selected object in the scene.", vpos, false);

					GUI.Label (new Rect (10, vpos, 150, 20), "Spacing");
					spacing = EditorGUI.Slider (new Rect (90, vpos, width - 100, 20), spacing, -.3f, 2);
					vpos += 35;
					float btWidth = width < 160 ? width - 20 : 160;
					if (GUI.Button (new Rect (width / 2 - btWidth / 2, vpos, btWidth, 25), "Duplicate")) {

						GameObject newClone = null;
						GameObject p = PrefabUtility.GetPrefabParent(sceneActiveSelection) as GameObject;
						var newSelection = new List<GameObject> ();

						foreach(Vector3 thisClone in clones) {
							if(p != null) {
								newClone = PrefabUtility.InstantiatePrefab(p) as GameObject;
								newClone.transform.position = thisClone + sceneActiveSelection.transform.position;
								newClone.transform.rotation = sceneActiveSelection.transform.rotation;
							} else {
								newClone = Instantiate(sceneActiveSelection.gameObject, thisClone + sceneActiveSelection.transform.position, sceneActiveSelection.transform.rotation) as GameObject;
								newClone.transform.localScale = sceneActiveSelection.transform.localScale;
							}
							newClone.transform.parent = sceneActiveSelection.transform.parent;
							newClone.transform.localScale = sceneActiveSelection.transform.localScale;
							Undo.RegisterCreatedObjectUndo (newClone, "Duplicate");
							newSelection.Add (newClone);
						}
						Selection.objects = newSelection.ToArray ();
					}
				}
			} else {
				SOT_lib.SHUX.alertBox ("Duplicate", "Select an object in the hierarchy to enable this tool.");
			}			
		}







	}
}

