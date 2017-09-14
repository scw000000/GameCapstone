using UnityEngine;
using UnityEditor;
using System.Collections;
using SOT_lib;

namespace SOT_info {
	public class lib : MonoBehaviour {

		public static int totalPolygons;
		public static int totalVertex;
		public static int currentLine = 0;
		public static int ptr = 0;
		public static int lastObjectId;
		public static ArrayList allPolygons;
		public static ArrayList allVertex;
		public static ArrayList allNames;
		public static ArrayList xprocessed;

		public static Vector2 scrollPosition = Vector2.zero;

		public static void renderGUI(int vpos, GameObject[] sceneSelection, GameObject[] projectSelection)
		{
			int width = Screen.width;
			int height = Screen.height;

			GUIStyle centeredStyle = new GUIStyle (GUI.skin.label);
			centeredStyle.alignment = TextAnchor.MiddleCenter;

			if (!Application.isPlaying) {
				if (Selection.activeGameObject) {
					int leftColWidth = width - 180;

					GUIStyle styleArrayNumbers = new GUIStyle (GUI.skin.label);
					styleArrayNumbers.alignment = TextAnchor.UpperRight;

					GUIStyle styleHeaderNumbers = new GUIStyle (GUI.skin.label);
					styleHeaderNumbers.alignment = TextAnchor.MiddleRight;

					GUIStyle styleHeaderStrings = new GUIStyle (GUI.skin.label);
					styleHeaderStrings.alignment = TextAnchor.MiddleLeft;

					GUIStyle styleSmallNumbers = new GUIStyle (GUI.skin.label);
					styleSmallNumbers.alignment = TextAnchor.UpperCenter;
					styleSmallNumbers.wordWrap = true;
					
					GUIStyle styleNumbers = new GUIStyle (GUI.skin.label);
					styleNumbers.fontSize = 20;
					styleNumbers.alignment = TextAnchor.UpperCenter;
					
					GUIStyle styleTitle = new GUIStyle (GUI.skin.label);
					styleTitle.fontSize = 14;
					
					int newObjectId = 0;
					foreach (GameObject GO in Selection.gameObjects) {
						newObjectId += GO.GetInstanceID ();
					}
					
					if (lastObjectId != newObjectId) {
						ptr++;
						lastObjectId = newObjectId;
						
						allPolygons = new ArrayList ();
						allVertex = new ArrayList ();
						allNames = new ArrayList ();
						xprocessed = new ArrayList ();
						
						currentLine = 0;
						totalPolygons = 0;
						totalVertex = 0;
						
						foreach (GameObject GO in Selection.gameObjects) {
							scanAChild (GO.transform);
						}
					}
					if (width > 180 && height > 200 && currentLine > 1) { // Hiden if the area is too small 
						if (currentLine > 1000) {
							GUI.Label (new Rect (10, vpos + 90, width - 20, 100), currentLine + " elements are selected. Select less than 1000 elements to view the details.");
						} else {
							GUI.Box (new Rect (0, vpos + 60, width, 20), "");
							GUI.Label (new Rect (20, vpos + 60, 70, 20), "Elements", styleHeaderStrings);
							GUI.Label (new Rect (leftColWidth + (width < 250 ? 80 : 0), vpos + 60, 70, 20), "Polygons", styleHeaderNumbers);
							if (width > 250)
								GUI.Label (new Rect (leftColWidth + 80, vpos + 60, 70, 20), "Vertex", styleHeaderNumbers);

							int deltav = 5;
							scrollPosition = GUI.BeginScrollView (new Rect (0, vpos + 80, width - 5, height - 105 - vpos), scrollPosition, new Rect (0, 0, width - 20, currentLine * 14 + 30 + deltav));

							if (xprocessed != null) {
								for (int subv = 0; subv < currentLine; subv++) {
									if (xprocessed [subv] != null)
										xprocessed [subv] = (int)allPolygons [subv];
								}

								for (int v = 0; v < currentLine; v++) {
									int maxFoundIndex = 0;
									for (int subv = 0; subv < currentLine; subv++) {
										if ((int)xprocessed [subv] > (int)xprocessed [maxFoundIndex]) {
											maxFoundIndex = subv;
										}
									}
									int thisPolycount = (int)allPolygons [maxFoundIndex];
									xprocessed [maxFoundIndex] = 0;
									
									GUI.Label (new Rect (20, v * 14 + deltav, leftColWidth - 10 + (width < 250 ? 80 : 0), 15), (string)allNames [maxFoundIndex]);
									
									if (thisPolycount >= 10000) {
										GUI.Label (new Rect (leftColWidth + (width < 250 ? 80 : 0), v * 14 + deltav, 70, 15), (thisPolycount / 1000) + "K", styleArrayNumbers);
									} else {
										GUI.Label (new Rect (leftColWidth + (width < 250 ? 80 : 0), v * 14 + deltav, 70, 15), "" + thisPolycount, styleArrayNumbers);
									}
									
									if (width > 250) {
										if ((int)allVertex [maxFoundIndex] >= 10000) {
											GUI.Label (new Rect (leftColWidth + 80, v * 14 + deltav, 70, 15), ((int)allVertex [maxFoundIndex] / 1000) + "K", styleArrayNumbers);
										} else {
											GUI.Label (new Rect (leftColWidth + 80, v * 14 + deltav, 70, 15), "" + (int)allVertex [maxFoundIndex], styleArrayNumbers);
										}
									}
								}
							}

							GUI.EndScrollView ();
						}
					}

					int vWidth = 100;
					int vMargin = (width - vWidth * 3) / 2;
					int polyMargin = vMargin + vWidth; 
					int vertexMargin = vMargin + vWidth * 2;

					int scount = Selection.gameObjects.Length;
					GUI.Label (new Rect (vMargin, vpos, vWidth, 30), "" + scount, styleNumbers);
					GUI.Label (new Rect (vMargin, vpos + 25, vWidth, 40), "element" + (scount > 1 ? "s" : "") + "\nselected", styleSmallNumbers);
					GUI.Label (new Rect (polyMargin, vpos + 25, vWidth, 18), "Polygons", styleSmallNumbers);
					if (totalPolygons >= 1000000) {
						int poly = (int)totalPolygons;
						GUI.Label (new Rect (polyMargin, vpos, vWidth, 30), ((float)poly / 1000000).ToString ("F1") + "M", styleNumbers);
					} else {
						if (totalPolygons >= 10000) {
							int poly = (int)totalPolygons;
							GUI.Label (new Rect (polyMargin, vpos, vWidth, 30), (poly / 1000) + "K", styleNumbers);
						} else {
							GUI.Label (new Rect (polyMargin, vpos, vWidth, 30), "" + totalPolygons, styleNumbers);
						}
					}
					GUI.Label (new Rect (vertexMargin, vpos + 25, vWidth, 18), "Vertex", styleSmallNumbers);
					if (totalVertex >= 1000000) {
						int vertx = (int)totalVertex;
						GUI.Label (new Rect (vertexMargin, vpos, vWidth, 30), ((float)vertx / 1000000).ToString ("F1") + "M", styleNumbers);
					} else {
						if (totalVertex >= 10000) {
							int vertx = (int)totalVertex;
							GUI.Label (new Rect (vertexMargin, vpos, vWidth, 30), (vertx / 1000) + "K", styleNumbers);
						} else {
							GUI.Label (new Rect (vertexMargin, vpos, vWidth, 30), "" + totalVertex, styleNumbers);
						}
					}
				} else {
					lastObjectId = 0;
					SOT_lib.SHUX.alertBox ("Info", "Select a prefab or an object in the scene, the hierarchy or the project window to enable this tool.");
				}
			} else {
				lastObjectId = 0;
				SOT_lib.SHUX.alertBox ("Info", "This tool is enabled in editor mode only.");
			}
			if (GUI.Button (new Rect (width / 2 - 100, height - 40, 200, 20), "Help and Support")) {
				Application.OpenURL ("http://shaderland.com/");
			}
		
		}

		public static void scanAChild(Transform parentChild) {
			int polygonsCount = 0;
			int vertexCount = 0;
			
			MeshFilter objectMeshFilter;
			Mesh objectMesh;
			
			objectMeshFilter = parentChild.GetComponent<MeshFilter> ();
			if (objectMeshFilter != null)
			{
				objectMesh = objectMeshFilter.sharedMesh;
				polygonsCount = objectMesh.triangles.Length / 3;
				vertexCount = objectMesh.vertexCount;
				
				totalPolygons = totalPolygons + polygonsCount;
				totalVertex = totalVertex + vertexCount;
				
				allPolygons.Add(polygonsCount);
				xprocessed.Add(polygonsCount);
				allVertex.Add(vertexCount);
				allNames.Add(parentChild.name);
				currentLine++;
			}
			
			foreach (Transform child in parentChild)
			{
				objectMeshFilter = child.GetComponent<MeshFilter> ();
				if (objectMeshFilter != null)
				{
					objectMesh = objectMeshFilter.sharedMesh;
					if(objectMesh != null)
					{
						if(objectMesh.triangles != null)
						{
							polygonsCount = objectMesh.triangles.Length / 3;
							vertexCount = objectMesh.vertexCount;
							
							totalPolygons = totalPolygons + polygonsCount;
							totalVertex = totalVertex + vertexCount;
							
							allPolygons.Add(polygonsCount);
							xprocessed.Add(polygonsCount);
							allVertex.Add(vertexCount);
							allNames.Add(child.name);
							currentLine++;
						}
					}
				}
				
				if(child.transform.childCount > 0) {
					scanAChild(child.transform);
				}
			}
		}
	}

}