using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OwnerMove : MonoBehaviour {

	[SerializeField]
	private Transform target;

	private NavMeshAgent agent;

	public float curSpeed;
	public Vector3 previousPosition;
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		agent = this.GetComponent<NavMeshAgent> ();

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetVector = target.transform.position;
		agent.SetDestination (targetVector);

		Vector3 curMove = transform.position - previousPosition;
		curSpeed = curMove.magnitude / Time.deltaTime;
		previousPosition = transform.position;

		this.GetComponent<Animator> ().SetFloat ("MoveSpeed", curSpeed);
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			gameManager.GameOver ();
		}
	}
}
