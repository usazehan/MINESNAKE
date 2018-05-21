using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeMovement : MonoBehaviour
{
	public float speed = 3.5f;
	public List<Transform> bodyTransformList = new List<Transform> ();
	public List<GameObject> bodyObjectList = new List<GameObject> ();
	public GameObject bodyObject;

	public int scoreValue;
	public Text scoreText;
	public Text highScoreText;

	private Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode> ();

	private enum axes
	{
		horizontal,
		vertical
	}

	private Vector3 dir = Vector3.right;
	private axes axis = axes.horizontal;

	private bool paused = false;
	private bool gameOver = false;

	public AudioSource sfxEatFood;
	public AudioSource sfxEatPowerUp;
	public AudioSource sfxHitMine;
	public AudioSource sfxGameOver;


	// Use this for initialization
	void Start ()
	{
		gameOver = false;

		AddKeys ();

		DisplayScore ();
		DisplayHighScore ();

		// read below at function definition
		SetNextBodyPart (transform.position);
	}
	public Material purple,orange;
	void ColorMySnake(){
		for (int i = 0; i < bodyTransformList.Count; i++) {
			if (i % 2 == 0) {
				bodyTransformList [i].GetComponent<Renderer> ().material = purple;
			} else {
				bodyTransformList [i].GetComponent<Renderer> ().material = orange;
			}
		}
	}


	void Update ()
	{
		if (!gameOver && !paused)
		{
			if (Input.anyKey)
			{
				// this is good for OK movement
				// smoother movement requires more complicated code
	
				switch (axis)
				{
					case axes.horizontal:
						if (Input.GetKey (keys["Up"]))
						{
							dir = Vector3.up;
							axis = axes.vertical;
						}
						if (Input.GetKey (keys["Down"]))
						{
							dir = Vector3.down;
							axis = axes.vertical;
						}
						break;
	
					case axes.vertical:
						if (Input.GetKey (keys["Right"]))
						{
							dir = Vector3.right;
							axis = axes.horizontal;
						}
						if (Input.GetKey (keys["Left"]))
						{
							dir = Vector3.left;
							axis = axes.horizontal;
						}
						break;
				}
			}
		}
		ColorMySnake ();
		if (scoreValue >= 10 && scoreValue < 20) {
			speed = 5f;
		} else if (scoreValue >= 20 && scoreValue < 30) {
			speed = 6f;
		}else if (scoreValue >= 30 && scoreValue < 40) {
			speed = 7f;
		}else if (scoreValue >= 40 && scoreValue < 50) {
			speed = 8f;
		}else if (scoreValue >= 50 && scoreValue < 60) {
			speed = 9f;
		}else if (scoreValue >= 60 && scoreValue < 70) {
			speed = 10f;
		}else if (scoreValue >= 70 && scoreValue < 80) {
			speed = 11f;
		}else if (scoreValue >= 80 && scoreValue < 90) {
			speed = 12f;
		}else if (scoreValue >= 90) {
			speed = 13f;
		}

	}

	void FixedUpdate ()
	{
		MoveHeadForward ();
		RotateHeadForward ();
	}

	public void GameOverRoutine (string log_message)
	{
		gameOver = true;

		RemoveKeys ();

		GameObject.Find ("Main Camera").GetComponent<PauseMenu> ().GameOverRoutine ();

		//Debug.Log (log_message); // comment this line out to remove debugging
		//SceneManager.LoadScene (0);
	}

	public void TogglePause ()
	{
		paused = !paused;
	}



	// Key Functions

	// This is public so that keys can be added and removed dynamically
	// in the pause/options menu.

	public void ResetKeys ()
	{
		RemoveKeys ();
		AddKeys ();
	}

	void AddKeys ()
	{
		keys.Add ("Up", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Up", "UpArrow")));
		keys.Add ("Down", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Down", "DownArrow")));
		keys.Add ("Left", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Left", "LeftArrow")));
		keys.Add ("Right", (KeyCode) System.Enum.Parse (typeof(KeyCode), PlayerPrefs.GetString ("Right", "RightArrow")));
	}

	void RemoveKeys ()
	{
		keys.Remove ("Up");
		keys.Remove ("Down");
		keys.Remove ("Left");
		keys.Remove ("Right");
	}



	// Head Functions

	void MoveHeadForward ()
	{
		transform.Translate (dir * speed * Time.deltaTime, Space.World);
	}

	//  x = 0, y = 0
	//	z = how much rotation left
	//	w = how much rotation right
	//    z,    w
	//  0.7,  0.7	up
	//  0.7, -0.7	down
	//    1,    0	left
	//    0,    1	right
	private Quaternion qUp = new Quaternion (0, 0, 0.7f, 0.7f);
	private Quaternion qDown = new Quaternion (0, 0, 0.7f, -0.7f);
	private Quaternion qLeft = new Quaternion (0, 0, 1, 0);
	private Quaternion qRight = new Quaternion (0, 0, 0, 1);

	void RotateHeadForward ()
	{
		float rotationSpeed = 0.1f;
		Quaternion toRotation = qRight;

		if (dir == Vector3.up)
			toRotation = qUp;				
		if (dir == Vector3.down)
			toRotation = qDown;
		if (dir == Vector3.left)
			toRotation = qLeft;
		if (dir == Vector3.right)
			toRotation = qRight;

		if (bodyObjectList[0].GetComponent<MeshRenderer> ().enabled == false)
			transform.rotation = toRotation;
		else
			transform.rotation = Quaternion.Slerp (transform.rotation, toRotation, rotationSpeed);
		//transform.rotation = Quaternion.Slerp (transform.rotation, qHorizontalMax, rotationSpeed);
	}
		

	void OnCollisionEnter (Collision other)
	{
		if (other.transform.tag == "Food")
			HandleCollision_Food (other);

		if (other.transform.tag == "Mine")
			HandleCollision_Mine (other);
		
		if (other.transform.tag == "Wall")
			HandleCollision_Wall ();

		if (other.transform.tag == "Body")
			HandleCollision_Body (other);
	}

	void HandleCollision_Food (Collision other)
	{
		Destroy (other.gameObject);
		IncrementScore ();
		ShowNextBodyPart ();
		PlaySoundFX (sfxEatFood);
	}

	void HandleCollision_Mine (Collision other)
	{
		Destroy (other.gameObject);

		// body part at index 0 is the first invisible part, so it doesn't count
		if (bodyTransformList.Count > 1)
		{
			PlaySoundFX (sfxHitMine);
			HideLastBodyPart ();
		} else
		{
			PlaySoundFX (sfxGameOver);
			GameOverRoutine ("hit a mine");
		}
	}

	void HandleCollision_Wall ()
	{
		PlaySoundFX (sfxGameOver);
		GameOverRoutine ("hit a wall");
	}

	void HandleCollision_Body (Collision other)
	{
		PlaySoundFX (sfxGameOver);
		GameOverRoutine ("hit a body part");
	}



	// Body Functions

	// To fix a lot of problems due to the design choices made for the physics,
	// I came up with this solution: create an invisible body part from the get-go,
	// which is shown on the first apple eaten. Do the same for the other parts.

	// To start off, we call SetNextBodyPart in the Start function.
	// For every ShowNextBodyPart, SetNextBodyPart is called again.

	void SetNextBodyPart (Vector3 transformPosition)
	{
		GameObject newBodyPart = Instantiate (bodyObject, transformPosition, Quaternion.identity) as GameObject;
		if (bodyTransformList.Count == 0)
			newBodyPart.transform.tag = "Untagged";
		newBodyPart.GetComponent<MeshRenderer> ().enabled = false;
		newBodyPart.GetComponent<SphereCollider> ().enabled = false;

		bodyObjectList.Add (newBodyPart);
		bodyTransformList.Add (newBodyPart.transform);
	}

	void ShowNextBodyPart ()
	{
		int index = bodyObjectList.Count - 1;
		bodyObjectList[index].GetComponent<MeshRenderer> ().enabled = true;
		bodyObjectList[index].GetComponent<SphereCollider> ().enabled = true;
		SetNextBodyPart (bodyTransformList[index].position);
	}

	// To remove a body part, remove the invisible part, then hide the next last.

	void RemoveLastBodyPart ()
	{
		Destroy (bodyObjectList.Last ());
		bodyObjectList.RemoveAt (bodyObjectList.Count - 1);
		bodyTransformList.RemoveAt (bodyTransformList.Count - 1);
	}

	void HideLastBodyPart ()
	{
		RemoveLastBodyPart ();
		int index = bodyObjectList.Count - 1;
		bodyObjectList[index].GetComponent<MeshRenderer> ().enabled = false;
		bodyObjectList[index].GetComponent<SphereCollider> ().enabled = false;
	}



	// Score Functions

	void IncrementScore ()
	{
		++scoreValue;

		DisplayScore ();
		UpdateHighScore ();
	}

	void UpdateHighScore ()
	{
		if (scoreValue > PlayerPrefs.GetInt ("HighScore"))
		{
			PlayerPrefs.SetInt ("HighScore", scoreValue);
			DisplayHighScore ();
		}
	}

	void DisplayScore ()
	{
		scoreText.text = "Score: " + scoreValue.ToString ();	
	}

	void DisplayHighScore ()
	{
		highScoreText.text = "High Score: " + PlayerPrefs.GetInt ("HighScore").ToString ();
	}



	// Sound Functions

	void PlaySoundFX (AudioSource source)
	{
		if (source != null)
			source.PlayOneShot (source.clip);
	}
}
