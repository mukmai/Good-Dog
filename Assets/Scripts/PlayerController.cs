using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Camera
    public Transform playerCam, character, centerPt, directionPt, leftPt, rightPt, frontPt, backPt, bowl;

    public float mouseX, mouseY;
    public float mouseSpeed = 200f;
    public float mouseYPosition = 0.15f;

    private float moveRate;
    public float moveSpeed = 3f;

    public float zoom;
    public float zoomSpeed = 2;

    public float zoomMin = -1.5f;
    public float zoomMax = -3f;

    public float rotationSpeed = 5f;

	private Animator animator;
	private bool peeing;
	private bool eating;
	[SerializeField]
	private bool edible;


	// Use this for initialization
	void Start () {

        zoom = -1.5f;
		animator = character.GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {

        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom > zoomMin)
        {
            zoom = zoomMin;
        }
        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }

	      playerCam.transform.localPosition = new Vector3(0, 0, zoom);

        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");

        mouseY = Mathf.Clamp(mouseY, -10f, 60f);
        playerCam.LookAt(centerPt);
        centerPt.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

		Quaternion temp = Quaternion.Euler(mouseY, mouseX, 0);

		temp.x = 0;
		temp.z = 0;

		directionPt.localRotation = temp;


		checkEdible ();


		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			peeing = true;
			animator.SetBool ("Peeing", true);
		}

		if (Input.GetKeyUp (KeyCode.Mouse0)) {
			peeing = false;
			animator.SetBool ("Peeing", false);
		}

		if (Input.GetKeyDown (KeyCode.Space) && edible) {
			eating = true;
			animator.SetBool ("Eating", true);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			eating = false;
			animator.SetBool ("Eating", false);
		}


		if (eating) {
			moveRate = 0;
			SendMessage ("getBar", 2f);
		} else if (peeing) {
			moveRate = 0;
			SendMessage ("useBar", 1f);
		} else {
			moveRate = Mathf.Clamp ((Mathf.Abs (Input.GetAxis ("Vertical")) + Mathf.Abs (Input.GetAxis ("Horizontal"))), 0f, 1f) * moveSpeed;
		}
			
        Vector3 movement = new Vector3(0, 0, moveRate);
        movement = character.rotation * movement;

		// Add gravity
		if (!character.GetComponent<CharacterController> ().isGrounded) {
			movement.y -= 30f * Time.deltaTime;
		}

		// Walk animation
		Vector3 hVelocity = character.GetComponent<CharacterController> ().velocity;
		hVelocity.y = 0;
		animator.SetFloat("Speed", hVelocity.magnitude);


        character.GetComponent<CharacterController>().Move(movement * Time.deltaTime);



		// Center Point and Direction Point Update
        centerPt.position = new Vector3(character.position.x, character.position.y + mouseYPosition, character.position.z);
		directionPt.position = new Vector3(character.position.x, character.position.y, character.position.z);


		// Look at bowl if eating
		if (eating) {
			Vector3 direction = (bowl.position - character.position).normalized;
			Quaternion lookRotation = Quaternion.LookRotation (direction);
			character.rotation = Quaternion.Slerp (character.rotation, lookRotation, Time.deltaTime * rotationSpeed);
		}

		// Change direction if not eating or peeing
		if (!eating && !peeing) {
			Vector3 fbPosition = character.position;
			Vector3 lrPosition = character.position;
			if (Input.GetAxis ("Vertical") > 0) {
				fbPosition = frontPt.position;
			}
			if (Input.GetAxis ("Vertical") < 0) {
				fbPosition = backPt.position;
			}
			if (Input.GetAxis ("Horizontal") > 0) {
				lrPosition = rightPt.position;
			}
			if (Input.GetAxis ("Horizontal") < 0) {
				lrPosition = leftPt.position;
			}

			Vector3 totalPosition = (fbPosition + lrPosition) / 2;

			if (totalPosition != character.position) {
				Vector3 direction = (totalPosition - character.position).normalized;
				Quaternion lookRotation = Quaternion.LookRotation (direction);
				character.rotation = Quaternion.Slerp (character.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}
		}
    }

	void checkEdible() {
		Vector3 length = transform.position - bowl.position;
		if (length.magnitude < 0.6f) {
			edible = true;
		} else {
			edible = false;
		}
	}
}
