using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
	public float moveSpeedModifier;
	public float jumpSpeed;

	[Range(0f, 1f)]
	public float moveAcceleration;
	private float currentHorizontalMoveSpeed = 0f;
	private float lastPos = 0f;
	private float thisPos = 0f;

	int standableLayerMask = 1 << 9;

	private Rigidbody rb;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		UpdateHorizontalMoveSpeed();

		if(Input.GetButtonDown("Jump") && CheckGrounded()){
			rb.AddForce(new Vector3(0f, jumpSpeed, 0f));
		}
	}

	private void UpdateHorizontalMoveSpeed(){
		float hMovement = Input.GetAxisRaw("Horizontal");
		float calculatedMaxMoveSpeed = Time.deltaTime * moveSpeedModifier;
		float thisHorizontalTranslate = 0f;

		// if holding down move key
		if(!Mathf.Approximately(hMovement, 0f)){
			RaycastHit collisionCheck = HasCollision(hMovement);
			if(collisionCheck.collider != null){
				// we ran into something, adjust pos calculation
				GameObject collisionObj = collisionCheck.collider.gameObject;
				Vector3 objectPos = collisionObj.transform.position;
				Vector3 currentPos = transform.position;

				float selfCenterToEdgeDistance = gameObject.GetComponent<BoxCollider>().bounds.size.x / 2f;//
				float objectCenterToEdgeDistance = collisionObj.GetComponent<BoxCollider>().bounds.size.x / 2f;//

				float newX = objectPos.x + (-hMovement * (objectCenterToEdgeDistance + selfCenterToEdgeDistance));
				transform.position = new Vector3(newX, transform.position.y, transform.position.z);

				// we want to continue as if we have been stopped, so reset the pos vars
				thisPos = transform.position.x;
				lastPos = transform.position.x;
				currentHorizontalMoveSpeed = 0f;
			} else {
				thisHorizontalTranslate = currentHorizontalMoveSpeed + (hMovement * Time.deltaTime * moveAcceleration);
				if(Mathf.Abs(thisHorizontalTranslate) > calculatedMaxMoveSpeed){
					thisHorizontalTranslate = hMovement * calculatedMaxMoveSpeed;
				}
			}			
		} else { // not holding down move key
			float lastMoveDir = (thisPos - lastPos) / Mathf.Abs(thisPos - lastPos);
			thisHorizontalTranslate = currentHorizontalMoveSpeed - (lastMoveDir * Time.deltaTime * moveAcceleration);
			float updatedMoveDir = thisHorizontalTranslate / Mathf.Abs(thisHorizontalTranslate);
			if(!Mathf.Approximately(lastMoveDir, updatedMoveDir)){
				thisHorizontalTranslate = 0f;
			}
		}
		transform.Translate(thisHorizontalTranslate, 0f, 0f);
		
		lastPos = thisPos;
		thisPos = transform.position.x;
		currentHorizontalMoveSpeed = thisPos - lastPos;
	}

	/// <summary>
	/// OnCollisionEnter is called when this collider/rigidbody has begun
	/// touching another rigidbody/collider.
	/// </summary>
	/// <param name="other">The Collision data associated with this collision.</param>
	void OnCollisionEnter(Collision other){
		if(Mathf.Approximately(Vector3.Dot(other.contacts[0].normal, Vector3.right), -1f) 
		|| Mathf.Approximately(Vector3.Dot(other.contacts[0].normal, Vector3.right), 1f)){
			thisPos = transform.position.x;
			lastPos = transform.position.x;
			currentHorizontalMoveSpeed = 0f;
		}
	}

	private RaycastHit HasCollision(float moveDir){
		RaycastHit collision = new RaycastHit();

		RaycastHit hitTop;
		RaycastHit hitBot;

		Vector3 topOrigin = Vector3.zero;
		Vector3 botOrigin = Vector3.zero;
		
		if(moveDir > 0f){
			topOrigin = transform.position + new Vector3(0.5f, 0.4f, 0f);
			botOrigin = transform.position + new Vector3(-0.5f, 0.4f, 0f);
		} else {
			topOrigin = transform.position + new Vector3(0.5f, -0.4f, 0f);
			botOrigin = transform.position + new Vector3(-0.5f, -0.4f, 0f);
		}

		bool topWallCheck = Physics.Raycast(topOrigin, new Vector3(moveDir, 0, 0), out hitTop, 0.101f, standableLayerMask);
		bool botWallCheck = Physics.Raycast(botOrigin, new Vector3(moveDir, 0, 0), out hitBot, 0.101f, standableLayerMask);

		if(topWallCheck){
			collision = hitTop;
		} else if(botWallCheck){
			collision = hitBot;
		}

		return collision;
	}

	private bool CheckGrounded(){
		bool grounded = false;

		RaycastHit hitLeft;
		RaycastHit hitRight;
		bool leftGroundCheck = Physics.Raycast(transform.position + new Vector3(-0.5f, -0.4f, 0f), Vector3.down, out hitLeft, 0.2f, standableLayerMask);
		bool rightGroundCheck = Physics.Raycast(transform.position + new Vector3(0.5f, -0.4f, 0f), Vector3.down, out hitRight, 0.2f, standableLayerMask);

		if(leftGroundCheck || rightGroundCheck){
			grounded = true;
		}

		return grounded;
	}

	void OnDrawGizmosSelected()
    {
		// Draws a blue line from this transform to the target
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position + new Vector3(-0.5f, -0.5f, 0f), transform.position + new Vector3(-0.5f, -0.6f, 0f));
		Gizmos.DrawLine(transform.position + new Vector3(0.5f, -0.5f, 0f), transform.position + new Vector3(0.5f, -0.6f, 0f));
    }
}
