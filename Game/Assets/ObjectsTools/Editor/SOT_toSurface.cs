using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using SOT_lib;

namespace SOT_toSurface {
	public class lib : MonoBehaviour {
		public static Vector2 scrollPosition = Vector2.zero;
		public static bool orient = false;
		public static float distanceToGround = 0;
		public static bool moveToSurface = true;
		public static bool randomizeVerticalPos = false;
		public static float minRandomizeVerticalPos = -.1f;
		public static float maxRandomizeVerticalPos = .1f;
		public static bool randomizeOrientation = false;
		public static float minRandomizeOrientation = -180f;
		public static float maxRandomizeOrientation = 180f;
		public static bool randomizeAngle = false;
		public static float minRandomizeAngle = -50f;
		public static float maxRandomizeAngle = 50f;
		public static bool randomizeSize = false;
		public static float minRandomizeSize = .9f;
		public static float maxRandomizeSize = 1.5f;

		public static void renderGUI(int vpos, GameObject sceneActiveSelection)
		{
			int width = Screen.width;
			int height = Screen.height;

			GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
			centeredStyle.alignment = TextAnchor.MiddleCenter;

			if (sceneActiveSelection) {


				int vvpos = 260; 
				if(orient) vvpos += 15;
				if(moveToSurface) vvpos += 60;
				if(randomizeVerticalPos) vvpos += 60;
				if(randomizeOrientation) vvpos += 60;
				if(randomizeAngle) vvpos += 60;
				if(randomizeSize) vvpos += 60;

				bool scrollVisible = height < vpos + vvpos + 25 ? true : false;
				int internalWidth = scrollVisible ? width - 15 : width;

				scrollPosition = GUI.BeginScrollView (new Rect (0, vpos, width - 5, height - vpos - 25), scrollPosition, new Rect (0, 0, width - 30, vvpos));
				vpos = 0;
				vpos += SOT_lib.SHUX.header ("<b>Move to Surface</b>\nMove selected objects to the surface and apply transformations.", vpos, false);


				
				GUIStyle rightStyle = new GUIStyle (GUI.skin.label);
				rightStyle.alignment = TextAnchor.MiddleRight;
				
				GUIStyle leftStyle = new GUIStyle (GUI.skin.label);
				leftStyle.alignment = TextAnchor.MiddleLeft;
				
				// Move to surface
				if(moveToSurface)
				{
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 70), "");
				}
				moveToSurface = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), moveToSurface, " Move to surface");
				vpos += 25;

				if(moveToSurface)
				{
					GUI.Label (new Rect (10, vpos, internalWidth - 20, 20), "Distance to ground");
					vpos += 15;
					distanceToGround = EditorGUI.Slider(new Rect(10, vpos,internalWidth - 20, 20), distanceToGround, -2f, 2f);
					vpos += 40;
					
				}
				
				// Orient to surface
				if(orient)
				{
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 25), "");
				}
				orient = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), orient, " Orient to surface");
				vpos += 25;
				if(orient)
				{
					vpos += 10;
				}
				
				// Vertical position
				if(randomizeVerticalPos)
				{
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 70), "");
				}
				randomizeVerticalPos = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), randomizeVerticalPos, " Randomize Altitude");
				vpos += 25;
				if(randomizeVerticalPos) {
					GUI.Label(new Rect(10, vpos, 100, 20), "-1", leftStyle);
					GUI.Label(new Rect(internalWidth / 2 - 10, vpos + 5, 20, 20), "|", centeredStyle);
					GUI.Label(new Rect(internalWidth - 10 - 100, vpos, 100, 20), "+1", rightStyle);
					vpos += 15;
					EditorGUI.MinMaxSlider(new Rect(10, vpos,internalWidth - 20, 20), ref minRandomizeVerticalPos, ref maxRandomizeVerticalPos, -1f, 1f);
					vpos += 40;
				}
				
				// Horizontal orientation
				if(randomizeOrientation)
				{
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 70), "");
				}
				randomizeOrientation = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), randomizeOrientation, " Randomize Direction");
				vpos += 25;
				if(randomizeOrientation) {
					GUI.Label(new Rect(10, vpos, 100, 20), "-180°", leftStyle);
					GUI.Label(new Rect(internalWidth / 2 - 10, vpos + 5, 20, 20), "|", centeredStyle);
					GUI.Label(new Rect(internalWidth - 10 - 100, vpos, 100, 20), "+180°", rightStyle);
					vpos += 15;
					EditorGUI.MinMaxSlider(new Rect(10, vpos,internalWidth - 20, 20), ref minRandomizeOrientation, ref maxRandomizeOrientation, -180f, 180f);
					vpos += 40;
				}
				
				// Vertical orientation
				if(randomizeAngle) {
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 70), "");
				}
				randomizeAngle = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), randomizeAngle, " Randomize Tilt");
				vpos += 25;
				if(randomizeAngle)
				{
					GUI.Label(new Rect(10, vpos, 100, 20), "-180°", leftStyle);
					GUI.Label(new Rect(internalWidth / 2 - 10, vpos + 5, 20, 20), "|", centeredStyle);
					GUI.Label(new Rect(internalWidth - 10 - 100, vpos, 100, 20), "+180°", rightStyle);
					vpos += 15;
					EditorGUI.MinMaxSlider(new Rect(10, vpos, internalWidth - 20, 20), ref minRandomizeAngle, ref maxRandomizeAngle, -180f, 180f);
					vpos += 40;
				}
				
				// Size
				if(randomizeSize)
				{
					vpos += 5;
					GUI.Box (new Rect (5, vpos - 5, internalWidth - 10, 70), "");
				}
				randomizeSize = GUI.Toggle (new Rect (10, vpos, internalWidth - 20, 20), randomizeSize, " Randomize size");
				vpos += 25;
				
				if(randomizeSize) {
					GUI.Label(new Rect(10, vpos, 100, 20), "1:2", leftStyle);
					GUI.Label(new Rect(internalWidth / 2 - 10, vpos + 5, 20, 20), "|", centeredStyle);
					GUI.Label(new Rect(internalWidth - 10 - 100, vpos, 100, 20), "2:1", rightStyle);
					vpos += 15;
					EditorGUI.MinMaxSlider(new Rect(10, vpos,internalWidth - 20, 20), ref minRandomizeSize, ref maxRandomizeSize, 0.5f, 2f);
					vpos += 40;
				}
				
				
				// --- Proceed ---
				vpos += 15;
				int btWidth = internalWidth < 160 ? internalWidth - 20 : 160;
				if (GUI.Button (new Rect (internalWidth / 2 - btWidth / 2, vpos, btWidth, 25), "Move")) {
					foreach (GameObject GO in Selection.gameObjects) {
						RaycastHit hit;
						bool done = false;
						Undo.RecordObject (GO.transform, "Objects placement");
						// Anything down?
						if (Physics.Raycast (GO.transform.position, Vector3.down, out hit)) {
							// Move to surface
							if(moveToSurface == true)
							{
								GO.transform.Translate (Vector3.down * (hit.distance - distanceToGround));
							}
							done = true;
						} else { // Nothing down, anything up?
							if (Physics.Raycast (GO.transform.position, Vector3.up, out hit)) {
								// Move to surface
								if(moveToSurface == true)
								{
									GO.transform.Translate (Vector3.up * (hit.distance + distanceToGround));
								}
								done = true;
							}
						}
						if(done)
						{
							if(randomizeVerticalPos)
							{
								GO.transform.Translate (Vector3.up * Random.Range (minRandomizeVerticalPos, maxRandomizeVerticalPos));
							}
							if(orient) 
							{
								GO.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
							}
							if(randomizeOrientation)
							{
								GO.transform.rotation = Quaternion.AngleAxis(Random.Range(minRandomizeOrientation, maxRandomizeOrientation), Vector3.up);
							}
							if(randomizeAngle)
							{
								GO.transform.rotation = Quaternion.AngleAxis(Random.Range(minRandomizeAngle, maxRandomizeAngle), Vector3.left);
								GO.transform.rotation = Quaternion.AngleAxis(Random.Range(minRandomizeAngle, maxRandomizeAngle), Vector3.forward);
							}
							if(randomizeSize)
							{
								float newScaleFactor = Random.Range (minRandomizeSize, maxRandomizeSize);
								GO.transform.localScale = new Vector3(GO.transform.localScale.x * newScaleFactor, GO.transform.localScale.y * newScaleFactor,GO.transform.localScale.z * newScaleFactor);
							}
						}
						
					}
				}

				GUI.EndScrollView ();

			} else {			
				SOT_lib.SHUX.alertBox("Move to Surface", "Select an object in the scene or in the hierarchy to enable this tool.");
			}

		}
	}
}

