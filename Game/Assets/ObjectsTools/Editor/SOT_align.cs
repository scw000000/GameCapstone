using UnityEngine;
using UnityEditor;
using System.Collections;
using SOT_lib;

namespace SOT_align {
	public class lib : MonoBehaviour {
		public static void renderGUI(int vpos, GameObject[] sceneSelection)
		{
			GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
			centeredStyle.alignment = TextAnchor.MiddleCenter;
			centeredStyle.wordWrap = true;
			int width = Screen.width;
			int height = Screen.height;

			if (sceneSelection != null && sceneSelection.Length > 1) {
				if(height > 200) {
					if(width > 200) {
						GUI.Label (new Rect (10, vpos, width - 20, 30), "Choose an axe to align the selected objects", centeredStyle);
					}
					vpos += 40;
				} else {
					vpos += height > 160 ? height - 160 : 0;
				}

				int margin = width / 20;
				int size = (width - margin * 4) / 3;
				size = size > 70 ? 70 : size;
				int vsize = 70;// < 200 ? height - vpos - 70 : 70;
				if(vsize > height - vsize - 15) vsize = height - vsize - 15;
				if (GUI.Button (new Rect (width / 2 - size - size / 2 - margin, vpos, size, vsize), "X")) {
					float uadd = 0;
					foreach (GameObject obj in sceneSelection) uadd += obj.transform.position.x;
					float uavg = uadd / sceneSelection.Length;
					foreach (GameObject obj in sceneSelection) {
						Vector3 v3 = new Vector3(uavg, obj.transform.position.y, obj.transform.position.z);
						Undo.RecordObject (obj.transform, "Objects alignment");
						obj.transform.position = v3;
					}
				}
				if (GUI.Button (new Rect (width / 2 - size / 2, vpos, size, vsize), "Y")) {
					float uadd = 0;
					foreach (GameObject obj in sceneSelection) uadd += obj.transform.position.y;
					float uavg = uadd / sceneSelection.Length;
					foreach (GameObject obj in sceneSelection) {
						Vector3 v3 = new Vector3(obj.transform.position.x, uavg, obj.transform.position.z);
						Undo.RecordObject (obj.transform, "Objects alignment");
						obj.transform.position = v3;
					}
				}
				if (GUI.Button (new Rect (width / 2 + size / 2 + margin, vpos, size, vsize), "Z")) {
					float uadd = 0;
					foreach (GameObject obj in sceneSelection) uadd += obj.transform.position.z;
					float uavg = uadd / sceneSelection.Length;
					foreach (GameObject obj in sceneSelection) {
						Vector3 v3 = new Vector3(obj.transform.position.x, obj.transform.position.y, uavg);
						Undo.RecordObject (obj.transform, "Objects alignment");
						obj.transform.position = v3;
					}
				}
			} else {
				if (sceneSelection == null) {
					SOT_lib.SHUX.alertBox("Align", "Select a minimum of 2 objects in the scene or in the hierarchy to enable this tool.");
				} else {
					SOT_lib.SHUX.alertBox("Align", "Select one more object to enable this tool.");
				}
			}
		}
	}
}

