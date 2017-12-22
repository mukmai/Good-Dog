using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour {

	public Image currentBoard;
	public Text ratioText;

	public float min = 0;
	public float max = 1;
	public Vector4 scores;
	public float totalScores = 0.015f;
	public float score;
	public GameManager gameManager;
	[SerializeField]
	public float cur;
	public bool cont = true;


	// Use this for initialization
	void Start () {
		cur = min;
		currentBoard.rectTransform.localScale = new Vector3 (cur, 1, 1);
	}
	
	// Update is called once per frame
	void Update () {
		if (cont) {
			Vector4 scores = SplatManagerSystem.instance.scores + new Vector4 (0.1f, 0.1f, 0.1f, 0.1f);
			score = scores.x - 0.1f;
			cur = score / totalScores;
			if (cur < min) {
				cur = min;
			} else if (cur >= max) {
				cur = max;
				cont = false;
				gameManager.GameWin ();
			}

			currentBoard.rectTransform.localScale = new Vector3 (cur, 1, 1);
			ratioText.text = (cur * 100).ToString ("0") + '%';
		}
	}
}
