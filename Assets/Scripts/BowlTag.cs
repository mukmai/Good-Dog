using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlTag : MonoBehaviour {

	public Image tag;
	
	// Update is called once per frame
	void Update () {
		tag.transform.position = Camera.main.WorldToScreenPoint (this.transform.position);
	}
}
