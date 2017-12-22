using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	bool gameEnded = false;

	public float restartDelay = 4f;

	public GameObject completeGameUI;
	public GameObject loseGameUI;

	public void GameWin() {
		if (!gameEnded) {
			gameEnded = true;
			Debug.Log ("Win");
			completeGameUI.SetActive (true);
		}
	}

	public void GameOver() {
		if (!gameEnded) {
			gameEnded = true;
			Debug.Log ("GAME OVER");
			loseGameUI.SetActive (true);
			// Invoke ("Restart", restartDelay);
		}
	}

/*	void Restart() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
*/
}
