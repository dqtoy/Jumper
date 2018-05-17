using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour {

	private PlayerController player;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update () 
	{
		if ( player.transform.position.z - transform.position.z > GameController.instance.getDeletionGap() * GameController.instance.getPlatformGap() )
		{
			Destroy (gameObject);
		}
	}
}
