using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
