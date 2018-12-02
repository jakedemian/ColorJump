using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPlatformController : MonoBehaviour {
	public Material startingColor;
	public List<Material> platformColors;	
	public AudioClip toneClip;
	[Range(-5f, 5f)]
	public float pitchModifier;

	private AudioSource audioSource;
	private int currentMaterialIndex = 0;

	private bool isLocked = false;

	void Start () {
		if(startingColor != null){
			SetMaterial(startingColor);
		}

		audioSource = GetComponent<AudioSource>();
	}

	void OnCollisionEnter(Collision other){
		if(isLocked){
			return;
		}

		if(Vector3.Dot(other.contacts[0].normal, Vector3.up) != -1f || other.gameObject.tag != "Player"){
			return;
		}

		if(GlobalPlatformData.NotifyLanded(gameObject)){
			if(platformColors.Count > 0){
				if(GetMaterial() != platformColors[currentMaterialIndex]){
					SetMaterial(platformColors[currentMaterialIndex]);

					audioSource.pitch = pitchModifier;
					audioSource.PlayOneShot(toneClip);

					currentMaterialIndex++;
					if(currentMaterialIndex >= platformColors.Count){
						currentMaterialIndex = 0;
					}
				}
			} else {
				Debug.LogWarning("ColorPlatformController.cs::OnCollisionEnter(): No materials added to this platform.");
			}
		}	
	}
	
	void Update () {
		
	}

	private Material GetMaterial(){
		return GetComponent<MeshRenderer>().sharedMaterial;
	}

	private void SetMaterial(Material m){
		GetComponent<MeshRenderer>().sharedMaterial = m;
	}

	public void Lock(){
		isLocked = true;
	}
}
