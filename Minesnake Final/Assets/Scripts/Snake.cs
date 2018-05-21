
// NOTE!!
// pretty sure this file is not needed anymore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{

	Vector2 dir = Vector2.right;
	// Vector2 moveVector;
	List<Transform> tail = new List<Transform> ();
	List<GameObject> tailobject = new List<GameObject> ();
	bool ate = false;
	public GameObject tailPrefab;
	bool vertical = true;
	bool horizontal = false;
	bool explosive = false;
	public float speed;
	public int score;
	public Text scoreText;
	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();

	void Start ()
	{
		
		InvokeRepeating ("Move", 0.3f, speed); 

		keys.Add ("Up", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Up", "UpArrow")));
		keys.Add ("Down", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Down", "DownArrow")));
		keys.Add ("Left", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Left", "LeftArrow")));
		keys.Add ("Right", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Right", "RightArrow")));
	}

	void Update ()
	{

		if (Input.GetKeyDown (keys["Right"]) && horizontal)
		{
			horizontal = false;
			vertical = true;		
			dir = Vector2.right;
		} else if (Input.GetKeyDown (keys["Down"]) && vertical)
		{
			horizontal = true;
			vertical = false;
			dir = -Vector2.up;
		} else if (Input.GetKeyDown (keys["Left"]) && horizontal)
		{
			horizontal = false;
			vertical = true;
			dir = -Vector2.right; 
		} else if (Input.GetKeyDown (keys["Up"]) && vertical)
		{
			horizontal = true;
			vertical = false;
			dir = Vector2.up;
		}
		// moveVector = dir / 3f;
	}

	void Move ()
	{
		
		Vector2 v = transform.position;
		transform.Translate (dir);

		if (ate)
		{
			GameObject g = (GameObject) Instantiate (tailPrefab, v, Quaternion.identity);
			tailobject.Insert (0, g);
			tail.Insert (0, g.transform);
			ate = false;
		} else if (tail.Count > 0)
		{
			tail.Last ().position = v;
			tail.Insert (0, tail.Last ());
			tail.RemoveAt (tail.Count - 1);
			tailobject.Insert (0, tailobject.Last ());
			tailobject.RemoveAt (tailobject.Count - 1);
		}
		if (explosive)
		{
			if (tailobject.Count > 0)
			{
				Destroy (tailobject.Last ());
				tail.RemoveAt (tail.Count - 1);
				tailobject.RemoveAt (tailobject.Count - 1);
				explosive = false;
			} else
			{
				Exit ();
			}
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.name.StartsWith ("food"))
		{
			ate = true;
			score++;
			scoreText.text = score.ToString ();
			Destroy (coll.gameObject);
			if (speed >= 0.05f)
			{
				speed -= 0.01f;
				CancelInvoke ("Move");
				InvokeRepeating ("Move", 0.3f, speed);  
			}
		}
		if (coll.name.StartsWith ("border"))
		{
			CancelInvoke ("Move");
			Exit ();
		}
		if (coll.name.StartsWith ("Tail"))
		{
			CancelInvoke ("Move");
			Exit ();
		}
		if (coll.name.StartsWith ("mine"))
		{
			explosive = true;
			Destroy (coll.gameObject);		
		}
		if (coll.name.StartsWith ("power"))
		{
			Destroy (coll.gameObject);
			if (speed <= 0.20f)
			{
				speed += 0.10f;
				CancelInvoke ("Move");
				InvokeRepeating ("Move", 0.3f, speed);  
			}
		}
	}

	public void Exit ()
	{
		SceneManager.LoadScene (0);
	}
}
