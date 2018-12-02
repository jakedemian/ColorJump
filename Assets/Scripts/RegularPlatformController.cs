using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularPlatformController : MonoBehaviour {

	// we still need to notify the global tracker that the player has
	// landed on a surface
	void OnCollisionEnter(Collision other){
		if(Vector3.Dot(other.contacts[0].normal, Vector3.up) != -1f || other.gameObject.tag != "Player"){
			return;
		}

		GlobalPlatformData.NotifyLanded(gameObject);
	}
}
