using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnFood : MonoBehaviour
{
	List<GameObject> prefabList = new List<GameObject> ();

	// Food Prefab
	public GameObject foodPrefab;
	public GameObject minePrefab;
	public GameObject powerupPrefab;


	// Borders
	public Transform borderTop;
	public Transform borderBottom;
	public Transform borderLeft;
	public Transform borderRight;
	public Transform TestT;
	public Transform TestB;
	public Transform TestR;
	public Transform TestL;



	// Use this for initialization
	void Start ()
	{
		prefabList.Add (foodPrefab);
		prefabList.Add (minePrefab);
		prefabList.Add (powerupPrefab);
		InvokeRepeating ("Spawn", 2, 1.66f);	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
	// Spawn one piece of food
	void Spawn ()
	{
		// x position between left & right border
		int x = (int) Random.Range (TestL.position.x, TestR.position.x);
		// y position between top & bottom border
		int y = (int) Random.Range (TestB.position.y, TestT.position.y);

		// int prefabIndex = (int)Random.Range (0, 3);

		// Instantiate the food at (x, y)
		//Instantiate(prefabList[prefabIndex],new Vector2(x, y),Quaternion.identity); // default rotation
		if (Random.value <= 0.65)
		{
			Instantiate (foodPrefab, new Vector2 (x, y), Quaternion.identity);
		} else if (Random.value > 0.65)
		{
			Instantiate (minePrefab, new Vector2 (x, y), Quaternion.identity);
		} 
			
	}
}
