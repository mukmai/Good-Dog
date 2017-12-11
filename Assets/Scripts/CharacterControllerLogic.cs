using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerLogic : MonoBehaviour {

	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float directionDampTime = .25f;
	[SerializeField]
	private ThirdPersonCamera gamecam;
	[SerializeField]
	private float directionSpeed = 3.0f;
	[SerializeField]
	private float rotationDegreePerSecond = 180f;

	private float speed = 0.0f;
	private float direction = 0f;
	private float horizontal = 0.0f;
	private float vertical = 0.0f;
	private bool peeing = false;

	private AnimatorStateInfo stateInfo;

	private int m_LocomotionId = 0;

	public float peeRate = 1f;
	public float drinkRate = 1f;
	public float animationSpeed = 1f;

	public Animator Animator {
		get {
			return this.animator;
		}
	}

	public float Speed {
		get {
			return this.speed;
		}
	}

	public float LocomotionThreshold {
		get {
			return 0.2f;
		}
	}

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		animator.speed = animationSpeed;

		if (animator.layerCount >= 2) {
			animator.SetLayerWeight(1, 1);
		}

		// Hash all animation names for performance
		m_LocomotionId = Animator.StringToHash("Base Layer.Locomotion");
	}

	// Update is called once per frame
	void Update () {
		if (animator) {
			stateInfo = animator.GetCurrentAnimatorStateInfo(0);

			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				peeing = true;
			}

			if (Input.GetKeyUp (KeyCode.Mouse0)) {
				peeing = false;
			}

			if (peeing) {
				horizontal = 0.0f;
				vertical = 0.0f;
				SendMessage ("useBar", peeRate);
			} else {
				horizontal = Input.GetAxis ("Horizontal");
				vertical = Input.GetAxis ("Vertical");
			}

			// Translate controls stick coordinates into world/cam/character space
			StickToWorldspace(this.transform, gamecam.transform, ref direction, ref speed);

			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction, directionDampTime, Time.deltaTime);
		}
	}

	void FixedUpdate() {
		// Rotate character model if pressed left or right, but only if character is moving in that direction
		if (IsInLocomotion() && ((direction >= 0 && horizontal >= 0) || (direction < 0 && horizontal < 0))) {
			Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreePerSecond * (horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(horizontal));
			Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
			this.transform.rotation = (this.transform.rotation * deltaRotation);
		}
	}

	public void StickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut) {
    Vector3 rootDirection = root.forward;
    Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

		speedOut = stickDirection.sqrMagnitude;

	    // Get camera rotation
	    Vector3 CameraDirection = camera.forward;
	    CameraDirection.y = 0.0f; // kill Y
	    Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

	    // Convert joystick input in Worldspace coordinates
	    Vector3 moveDirection = referentialShift * stickDirection;
		Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), moveDirection, Color.green);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), rootDirection, Color.magenta);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2f, root.position.z), stickDirection, Color.blue);
		Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.5f, root.position.z), axisSign, Color.black);

		float sign = 0f;
		if (axisSign.y > 0) sign = -1f;
		if (axisSign.y < 0) sign = 1f;
		float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * sign;
		angleRootToMove /= 180f;

		directionOut = angleRootToMove * directionSpeed;
	}

	public bool IsInLocomotion() {
		return stateInfo.fullPathHash == m_LocomotionId;
	}
}
