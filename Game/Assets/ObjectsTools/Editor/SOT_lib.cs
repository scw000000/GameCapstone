using UnityEngine;
using System.Collections;
using UnityEditor;

namespace SOT_lib {
	public class SHUX : ScriptableObject {
		public static int header(string stringText, int vpos, bool autoHide)
		{
			int width = Screen.width;
			int height = Screen.height;
			int thisH = 0;
			if (width > 150 && height > 200 || !autoHide) {
				GUIStyle richStyle = new GUIStyle (GUI.skin.box);
				richStyle.richText = true;
				richStyle.wordWrap = true;
				richStyle.fontSize = 10;
				richStyle.normal.textColor = GUI.skin.label.normal.textColor;
				richStyle.alignment = TextAnchor.MiddleLeft;
				thisH = (int)richStyle.CalcHeight (new GUIContent (stringText), width - 20) + 4;
				GUI.Box (new Rect (10, vpos, width - 20, thisH), stringText, richStyle);
				thisH += 15;
			}
			return(thisH);
		}

		public static void alertBox(string title, string stringText)
		{
			int vpos = 0;
			int width = Screen.width;
			int height = Screen.height;

			GUIStyle richStyle = new GUIStyle (GUI.skin.label);
			richStyle.richText = true;
			richStyle.wordWrap = true;
			richStyle.fontSize = 11;
			richStyle.normal.textColor = GUI.skin.label.normal.textColor;
			richStyle.alignment = TextAnchor.MiddleCenter;
			string headerTitle = "<b>" + title + "</b>\n\n";
			int lw = width > 300 ? 300 : width - 20;

			GUI.Box (new Rect (width / 2 - lw / 2, vpos, lw, height - vpos > 200 ? 200 : height - vpos), headerTitle + stringText, richStyle);
		}

		public static Bounds getAllBounds(GameObject g_o) {
			MeshFilter objectMeshFilter = g_o.GetComponent<MeshFilter> ();
			Bounds allBounds = new Bounds();
			if (objectMeshFilter != null) {
				Mesh mesh = objectMeshFilter.sharedMesh;
				allBounds = mesh.bounds;
			}
			foreach (Transform child in g_o.transform) {
				MeshFilter SobjectMeshFilter = child.GetComponent<MeshFilter> ();
				if (SobjectMeshFilter != null) {
					Mesh Smesh = SobjectMeshFilter.sharedMesh;
					allBounds.Encapsulate(Smesh.bounds);
				}
				if (child.transform.childCount > 0) {
					allBounds.Encapsulate(getAllBounds (child.gameObject));
				}
			}
			return(allBounds);
		}

		public static void draft(GameObject g_o, Vector3 decal3, Color color) {
			MeshFilter objectMeshFilter = g_o.GetComponent<MeshFilter> ();
			if (objectMeshFilter != null) {
				Mesh mesh = objectMeshFilter.sharedMesh;
				drawDraft (g_o, mesh, decal3, color);
			}

			foreach (Transform child in g_o.transform) {
				MeshFilter SobjectMeshFilter = child.GetComponent<MeshFilter> ();
				if (SobjectMeshFilter != null) {
					Mesh Smesh = SobjectMeshFilter.sharedMesh;
					drawDraft (child.gameObject, Smesh, decal3, color);
				}
				if (child.transform.childCount > 0) {
					
					draft (child.gameObject, decal3, color);
				}
			}
		}


		public static void drawDraft(GameObject g_o, Mesh mesh, Vector3 decal3, Color color) {
			Handles.color = color;

			Vector3 v3FrontTopLeft;
			Vector3 v3FrontTopRight;
			Vector3 v3FrontBottomLeft;
			Vector3 v3FrontBottomRight;
			Vector3 v3BackTopLeft;
			Vector3 v3BackTopRight;
			Vector3 v3BackBottomLeft;
			Vector3 v3BackBottomRight;
			Vector3 v3Center = mesh.bounds.center;
			Vector3 v3Extents = mesh.bounds.extents;
			
			v3FrontTopLeft = new Vector3 (v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z- v3Extents.z);
			v3FrontTopRight = new Vector3 (v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z); 
			v3FrontBottomLeft = new Vector3 (v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);
			v3FrontBottomRight = new Vector3 (v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);
			
			v3BackTopLeft = new Vector3 (v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);
			v3BackTopRight = new Vector3 (v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);
			v3BackBottomLeft = new Vector3 (v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);
			v3BackBottomRight = new Vector3 (v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);
			
			v3FrontTopLeft = g_o.transform.TransformPoint (v3FrontTopLeft);
			v3FrontTopRight = g_o.transform.TransformPoint (v3FrontTopRight);
			v3FrontBottomLeft = g_o.transform.TransformPoint (v3FrontBottomLeft);
			v3FrontBottomRight = g_o.transform.TransformPoint (v3FrontBottomRight);
			
			v3BackTopLeft = g_o.transform.TransformPoint (v3BackTopLeft);
			v3BackTopRight = g_o.transform.TransformPoint (v3BackTopRight);
			v3BackBottomLeft = g_o.transform.TransformPoint (v3BackBottomLeft);
			v3BackBottomRight = g_o.transform.TransformPoint (v3BackBottomRight);

			v3FrontTopLeft = new Vector3(v3FrontTopLeft.x + decal3.x, v3FrontTopLeft.y + decal3.y, v3FrontTopLeft.z + decal3.z); 
			v3FrontTopRight = new Vector3(v3FrontTopRight.x + decal3.x, v3FrontTopRight.y + decal3.y, v3FrontTopRight.z + decal3.z); 
			v3FrontBottomLeft = new Vector3(v3FrontBottomLeft.x + decal3.x, v3FrontBottomLeft.y + decal3.y, v3FrontBottomLeft.z + decal3.z); 
			v3FrontBottomRight = new Vector3(v3FrontBottomRight.x + decal3.x, v3FrontBottomRight.y + decal3.y, v3FrontBottomRight.z + decal3.z); 
			
			v3BackTopLeft = new Vector3(v3BackTopLeft.x + decal3.x, v3BackTopLeft.y + decal3.y, v3BackTopLeft.z + decal3.z); 
			v3BackTopRight = new Vector3(v3BackTopRight.x + decal3.x, v3BackTopRight.y + decal3.y, v3BackTopRight.z + decal3.z); 
			v3BackBottomLeft = new Vector3(v3BackBottomLeft.x + decal3.x, v3BackBottomLeft.y + decal3.y, v3BackBottomLeft.z + decal3.z); 
			v3BackBottomRight = new Vector3(v3BackBottomRight.x + decal3.x, v3BackBottomRight.y + decal3.y, v3BackBottomRight.z + decal3.z); 

			Handles.DrawDottedLine (v3FrontTopLeft, v3FrontTopRight, 5);
			Handles.DrawDottedLine (v3FrontTopRight, v3FrontBottomRight, 5);
			Handles.DrawDottedLine (v3FrontBottomRight, v3FrontBottomLeft, 5);
			Handles.DrawDottedLine (v3FrontBottomLeft, v3FrontTopLeft, 5);
			Handles.DrawDottedLine (v3BackTopLeft, v3BackTopRight, 5);
			Handles.DrawDottedLine (v3BackTopRight, v3BackBottomRight, 5);
			Handles.DrawDottedLine (v3BackBottomRight, v3BackBottomLeft, 5);
			Handles.DrawDottedLine (v3BackBottomLeft, v3BackTopLeft, 5);
			Handles.DrawDottedLine (v3FrontTopLeft, v3BackTopLeft, 5);
			Handles.DrawDottedLine (v3FrontTopRight, v3BackTopRight, 5);
			Handles.DrawDottedLine (v3FrontBottomRight, v3BackBottomRight, 5);
			Handles.DrawDottedLine (v3FrontBottomLeft, v3BackBottomLeft, 5);
			

		}

	}
}






/*	DRAW A MESH
						int ulen = objectMeshFilter.sharedMesh.triangles.Length;
						int step = (int)(ulen / 2000f) * 3;
						if (step < 3)
							step = 3;
						for (int i = 0; i < ulen; i += step) {
							Vector3 p1 = g_o.transform.TransformPoint (vertices [triangles [i + 0]]);
							Vector3 p2 = g_o.transform.TransformPoint (vertices [triangles [i + 1]]);
							Vector3 p3 = g_o.transform.TransformPoint (vertices [triangles [i + 2]]);
							Handles.DrawDottedLine (p1, p2, 5);
							Handles.DrawDottedLine (p2, p3, 5);
							Handles.DrawDottedLine (p3, p1, 5);
						}
						*/

