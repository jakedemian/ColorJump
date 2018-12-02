using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	public List<GameObject> colorPlatforms;
	public List<Material> requiredColors;
	
	void Update () {
		for(int i = 0; i < colorPlatforms.Count; i++){
			if(GetMaterial(i) != requiredColors[i]){
				return;
			}
		}

		PuzzleComplete();
	}

	private Material GetMaterial(int i){
		return colorPlatforms[i].GetComponent<MeshRenderer>().sharedMaterial;
	}

	private void PuzzleComplete(){
		for(int i = 0; i < colorPlatforms.Count; i++){
			colorPlatforms[i].GetComponent<ColorPlatformController>().Lock();
		}

		Destroy(gameObject);
	}
}
