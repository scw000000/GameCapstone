using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSettings : MonoBehaviour {

	//public Toggle[] resolutionToggles;
	public Dropdown dropdown;

	public int[] screenWidths;
	public int[] screenHeights;
	int activeScreenResIndex;

	public void SetScreenResolution(int i){
		if (i == dropdown.value) {
			activeScreenResIndex = i;
			Screen.SetResolution(screenWidths[i], screenHeights[i], false);
		}
	}

	public void SetFullscreen (bool isFullscreen){
			dropdown.interactable = !isFullscreen;

		if(isFullscreen){
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxResolution = allResolutions[allResolutions.Length - 1];
			Screen.SetResolution(maxResolution.width, maxResolution.height, true);
		}else{
			SetScreenResolution(activeScreenResIndex);
		}
	}
}
