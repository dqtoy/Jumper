using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

	public GameObject gameController;

	// Use this for initialization
	void Awake () 
	{
		if (GameController.instance == null)
		{
			Instantiate(gameController);
		}
	}
}
