using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// Game Buttons

	public void Button_Main_Canvas_MainMenuUI_Play ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}

	public void Button_Main_Canvas_MainMenuUI_Quit ()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#endif

		Application.Quit ();
	}

	// UNUSED CODE --remove
	//	public void Button_Main_Canvas_MainMenuUI_Options ()
	//	{
	//	}
}
