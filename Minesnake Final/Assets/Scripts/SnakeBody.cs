using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
	private GameObject head;
	private Transform headTransform;
	private SnakeMovement headScript;
	private int index;
	private int colliderTimer = 50;

	public Vector3 oldPosition = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		head = GameObject.FindGameObjectWithTag ("Player").gameObject;
		headTransform = head.transform;
		headScript = head.GetComponent<SnakeMovement> ();
		index = headScript.bodyTransformList.Count - 1;
	}

	private Vector3 movementVelocity;
	public float overTime = 0.5f;

	// Update is called once per frame
	void FixedUpdate ()
	{
		if (colliderTimer > 0)
			HandleColliderTimer ();
		
		if (index == 0)
		{
			transform.position = Vector3.SmoothDamp (transform.position, headTransform.position, ref movementVelocity, overTime);
			transform.LookAt (headTransform.transform.position);
		} else
		{
			transform.position = Vector3.SmoothDamp (transform.position, headScript.bodyTransformList[index - 1].position, ref movementVelocity, overTime);
			transform.LookAt (headTransform.transform.position);
		}
	}

	void HandleColliderTimer ()
	{
		--colliderTimer;
		if (colliderTimer == 0)
			gameObject.GetComponent<SphereCollider> ().isTrigger = false;
	}


	//	void FixedUpdate ()
	//	{
	//		oldPosition = transform.position;
	//
	//		if (index == 0)
	//		{
	//			transform.position = headScript.oldPosition;
	//		} else
	//		{
	//			//transform.position = headScript.bodyObjectList [index - 1];
	//		}
	//	}
}
