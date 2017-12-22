using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour {

	public ScoreBoard board;
	public void LoadNextLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
