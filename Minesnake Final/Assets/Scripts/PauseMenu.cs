using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject GameOverText;
	public GameObject PauseText;
	public GameObject PauseButtons;
	public GameObject Player;

	private bool paused = false;
	private bool options = false;
	private bool gameOver = false;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();

	void Start ()
	{
		paused = false;
		options = false;
		gameOver = false;

		GameOverText.SetActive (false);
		PauseText.SetActive (false);
		PauseButtons.SetActive (false);

		AddKeys ();
		ToggleTimeScale ();
	}

	void Update ()
	{
		// OLD CODE --remove
		//	if (Input.GetKeyDown (keys ["Pause"])) {
		//		paused = !paused;
		//	}
		//	if (paused) {
		//		PauseUI.SetActive (true);
		//		Time.timeScale = 0;
		//	}
		//	if (!paused) {
		//		PauseUI.SetActive (false);
		//		Time.timeScale = 1;
		//	}

		if (!gameOver && !options)
		{
			if (Input.GetKeyDown (keys["Pause"]))
			{
				ToggleTimeScale ();
				TogglePaused ();
			}
		}
	}

	public void GameOverRoutine ()
	{
		gameOver = true;

		ToggleTimeScale ();
		ToggleGameOver ();

		RemoveKeys ();
	}



	// Key Functions

	// These are public so that keys can be added and removed dynamically
	// in the pause/options menu.

	void AddKeys ()
	{
		keys.Add ("Pause", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Pause", "Escape")));
	}

	void RemoveKeys ()
	{
		keys.Remove ("Pause");
	}



	// Toggle Functions

	void ToggleTimeScale ()
	{
		if (Time.timeScale == 1.0f)
			Time.timeScale = 0.0f;
		else
			Time.timeScale = 1.0f;
	}

	void TogglePaused ()
	{
		paused = !paused;
		PauseText.SetActive (paused);
		PauseButtons.SetActive (paused);
		Player.GetComponent<SnakeMovement> ().TogglePause ();
	}

	public void ToggleOptions ()
	{
		options = !options;
		if (options)
			RemoveKeys ();
		else
			AddKeys ();
	}

	void ToggleGameOver ()
	{
		GameOverText.SetActive (true);
		PauseText.SetActive (false);
		PauseButtons.SetActive (true);

		// a weird way to access children, but this is the standard technique
		PauseButtons.transform.Find ("Resume").gameObject.SetActive (false);
	}



	// Game Button Functions

	public void Button_Scene_Canvas_PauseUI_Resume ()
	{
		ToggleTimeScale ();
		TogglePaused ();
	}

	public void Button_Scene_Canvas_PauseUI_Restart ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	public void Button_Scene_Canvas_PauseUI_MainMenu ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}

	public void Button_Scene_Canvas_PauseUI_Quit ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif

		Application.Quit ();
	}
}
