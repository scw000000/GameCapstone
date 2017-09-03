using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageCapture : MonoBehaviour {
    public static string DefaultStorePath;
    public Camera RenderCamera;
    // Use this for initialization
    void Start () {
        DefaultStorePath = Application.persistentDataPath + "/ImageCapture/";
        System.IO.Directory.CreateDirectory(DefaultStorePath);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I)) {
            CaptureImage();
        }
	}

    void CaptureImage() {
        var ppComp = RenderCamera.GetComponent< UnityEngine.PostProcessing.PostProcessingBehaviour >();
        //ppComp.enabled = false;
        // capture the virtuCam and save it as a square PNG.
        int ouputWidth = RenderCamera.pixelWidth;
        int ouputHeight = RenderCamera.pixelHeight;

        RenderTexture tempRT = new RenderTexture(ouputWidth, ouputHeight, 24);

        RenderCamera.targetTexture = tempRT;
        RenderCamera.Render();

        RenderTexture.active = tempRT;
        Texture2D outputTexture = new Texture2D(ouputWidth, ouputHeight, TextureFormat.RGB24, false);
        // false, meaning no need for mipmaps
        outputTexture.ReadPixels(new Rect(0, 0, ouputWidth, ouputHeight), 0, 0);

        RenderTexture.active = null; //can help avoid errors 
        RenderCamera.targetTexture = null;
        Destroy(tempRT);

        byte[] bytes = outputTexture.EncodeToPNG();
        string dateString = System.DateTime.Now.ToShortDateString().ToString();
        dateString = dateString.Replace("/", ",");
        string timeString = System.DateTime.Now.ToLongTimeString().ToString();
        timeString = timeString.Replace(":", "_");
        string newFileName = DefaultStorePath 
            + dateString
            + "-"
            + timeString
            + ".png";
        var createdFile = System.IO.File.Create(newFileName);
        createdFile.Close();
        System.IO.File.WriteAllBytes(newFileName, bytes);
        ppComp.enabled = true;
        Debug.Log("Image Captured!");
    }
}
