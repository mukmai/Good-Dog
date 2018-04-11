using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplatMaker : MonoBehaviour {

	Vector4 channelMask = new Vector4(1,0,0,0);

	int splatsX = 1;
	int splatsY = 1;

	public float splatScale = 1.0f;
	public List<ParticleCollisionEvent> collisions;

	// Use this for initialization
	void Start () {
		collisions = new List<ParticleCollisionEvent> ();
	}

	// Update is called once per frame
	void Update () {

		// Get how many splats are in the splat atlas
		splatsX = SplatManagerSystem.instance.splatsX;
		splatsY = SplatManagerSystem.instance.splatsY;

		for (int i = 0; i < collisions.Count; i++) {
			Vector3 leftVec = Vector3.Cross ( collisions[i].normal, Vector3.up );
			float randScale = Random.Range(0.5f,1.5f);

			GameObject newSplatObject = new GameObject();
			newSplatObject.transform.position = collisions[i].intersection;
			if( leftVec.magnitude > 0.001f ){
				newSplatObject.transform.rotation = Quaternion.LookRotation( leftVec, collisions[i].normal );
			}
			newSplatObject.transform.RotateAround( collisions[i].intersection, collisions[i].normal, Random.Range(-180, 180 ) );
			newSplatObject.transform.localScale = new Vector3( randScale, randScale * 0.5f, randScale ) * splatScale;

			Splat newSplat;
			newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
			newSplat.channelMask = channelMask;

			float splatscaleX = 1.0f / splatsX;
			float splatscaleY = 1.0f / splatsY;
			float splatsBiasX = Mathf.Floor( Random.Range(0,splatsX * 0.99f) ) / splatsX;
			float splatsBiasY = Mathf.Floor( Random.Range(0,splatsY * 0.99f) ) / splatsY;

			newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY );

			SplatManagerSystem.instance.AddSplat (newSplat);

			GameObject.Destroy( newSplatObject );
		}
		collisions.Clear ();

	}
}
