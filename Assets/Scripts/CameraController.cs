using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private GameObject player;
	public float maxCamSpeed;
	public float camAcceleration; // 0 to 1
	public float camMovementMinimumOffsetDistance;
	public float verticalDistanceFocalPoint; // distance above the player to position the camera
	public float camFixedZPos;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		transform.LookAt(player.transform);

		Vector2 camPos = transform.position;
		Vector2 playerPos = player.transform.position;

		// if there is more than a tiny difference in cam and player x..
		if(Mathf.Abs(playerPos.x - camPos.x) > camMovementMinimumOffsetDistance){
			float rawDiff = playerPos.x - camPos.x;
			float xToMove = rawDiff * camAcceleration * Time.deltaTime;

			if(Mathf.Abs(camAcceleration) > maxCamSpeed){
				xToMove = (xToMove/Mathf.Abs(xToMove)) * maxCamSpeed; // set to max cam speed while preserving pos/neg
			}

			transform.Translate(xToMove, 0f, 0f);
			if(transform.position.z != camFixedZPos){
				transform.position = new Vector3(transform.position.x, transform.position.y, camFixedZPos);
			}
		}

		// if there is more than a tiny difference in cam and player y..
		if(Mathf.Abs(playerPos.y - camPos.y + verticalDistanceFocalPoint) > camMovementMinimumOffsetDistance){
			float rawDiff = playerPos.y - camPos.y + verticalDistanceFocalPoint;
			float yToMove = rawDiff * camAcceleration * Time.deltaTime;

			if(Mathf.Abs(camAcceleration) > maxCamSpeed){
				yToMove = (yToMove/Mathf.Abs(yToMove)) * maxCamSpeed; // set to max cam speed while preserving pos/neg
			}

			transform.Translate(0f, yToMove, 0f);
			if(transform.position.z != camFixedZPos){
				transform.position = new Vector3(transform.position.x, transform.position.y, camFixedZPos);
			}
		}
	}
}
