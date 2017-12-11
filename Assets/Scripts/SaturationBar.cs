using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaturationBar : MonoBehaviour {

	public Image currentBar;
	public Text ratioText;
	public ParticleLauncher penis;

	public float min = 0;
	public float max = 100;
	private float cur;

	// Use this for initialization
	void Start () {
		cur = max;
		UpdateBar ();
	}

	void UpdateBar () {
		float ratio = cur / max;
		currentBar.rectTransform.localScale = new Vector3 (ratio, 1, 1);
		ratioText.text = (ratio * 100).ToString ("0") + '%';
	}

	void useBar(float amount) {
		cur -= amount;
		if (cur < min) {
			cur = 0;
		}
		penis.saturation = cur;
		UpdateBar ();
	}

	void getBar(float amount) {
		cur += amount;
		if (cur > max) {
			cur = max;
		}
		UpdateBar ();
	}
}
