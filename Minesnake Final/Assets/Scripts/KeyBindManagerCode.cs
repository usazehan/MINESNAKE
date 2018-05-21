using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindManagerCode : MonoBehaviour
{
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();
	public Text up, left, down, right, pause;
	private GameObject currentKey;
	private Color32 normal = new Color32 (39, 171, 249, 255);
	private Color32 selected = new Color32 (239, 116, 36, 255);

	// Use this for initialization
	void Start ()
	{
		keys.Add ("Up", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Up", "UpArrow")));
		keys.Add ("Down", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Down", "DownArrow")));
		keys.Add ("Left", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Left", "LeftArrow")));
		keys.Add ("Right", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Right", "RightArrow")));
		keys.Add ("Pause", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Pause", "Escape")));

		up.text = keys["Up"].ToString ();
		down.text = keys["Down"].ToString ();
		left.text = keys["Left"].ToString ();
		right.text = keys["Right"].ToString ();
		pause.text = keys["Pause"].ToString ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (keys["Up"]))
		{
			//Debug.Log ("Up");
		}

		if (Input.GetKeyDown (keys["Down"]))
		{
			//Debug.Log ("Down");
		}

		if (Input.GetKeyDown (keys["Left"]))
		{		
			//Debug.Log ("Left");
		}

		if (Input.GetKeyDown (keys["Right"]))
		{
			//Debug.Log ("Right");
		}

		if (Input.GetKeyDown (keys["Pause"]))
		{
			//Debug.Log ("Pause");
		}
	}

	void OnGUI ()
	{
		if (currentKey != null)
		{
			Event e = Event.current;
			if (e.isKey)
			{
				keys[currentKey.name] = e.keyCode;
				currentKey.transform.GetChild (0).GetComponent<Text> ().text = e.keyCode.ToString ();
				currentKey.GetComponent<Image> ().color = normal;
				currentKey = null;
			}
		}
	}

	public void ChangeKey (GameObject clicked)
	{
		currentKey = clicked;
		currentKey.GetComponent<Image> ().color = selected;
	}

	public void SaveKeys ()
	{
		foreach (var key in keys)
		{
			PlayerPrefs.SetString (key.Key, key.Value.ToString ());
		}
		PlayerPrefs.Save ();
	}
}
