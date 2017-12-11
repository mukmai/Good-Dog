using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

	public ParticleSystem particleLauncher;
	public ParticleSystem splatterParticles;
	public Animator player;
	public float saturation = 100f;

	[SerializeField]
	private ThirdPersonCamera gamecam;
	SplatMakerExample splatter;

	List<ParticleCollisionEvent> collisionEvents;

	// Use this for initialization
	void Start () {
		collisionEvents = new List<ParticleCollisionEvent>();
		splatter = gamecam.GetComponent<SplatMakerExample> ();
	}

	void OnParticleCollision(GameObject other) {
		ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvents);
		splatter.collisions = collisionEvents;

		for (int i = 0; i < collisionEvents.Count; i++) {
			EmitAtLocation(collisionEvents[i]);
		}

	}

	void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent) {
		splatterParticles.transform.position = particleCollisionEvent.intersection;
		splatterParticles.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
		splatterParticles.Emit(1);
	}

	// Update is called once per frame
	void Update () {
		if (player.GetCurrentAnimatorStateInfo(0).IsName("Pee") && player.GetBool("Peeing") && saturation > 0) {
			particleLauncher.Emit(1);
		}
	}
}
