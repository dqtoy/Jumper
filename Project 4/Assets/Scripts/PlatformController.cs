using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

	public static int platformNoS;

	private const int PICKUP_GAP = 10;
	private const int MIN_SIZE = 3;
	private const int MAX_SIZE = 6;
	private const int RANDOM_SCALE = 100;
	private const int THRESHOLD = 8;
	private const float PICKUP_Y = 0.5f;

	public GameObject pickUp;

	private PlayerController player;
	public int platformNo;

	// Use this for initialization
	void Start () 
	{
		player = FindObjectOfType<PlayerController>();

		int randomSize = (int)Random.Range ( MIN_SIZE, MAX_SIZE );
		float randomPlatform = Random.Range ( 0, RANDOM_SCALE );

		//randomly scales the platforms so that some of them are smaller
		if ( randomPlatform < RANDOM_SCALE / THRESHOLD )
		{
			transform.localScale -= new Vector3(randomSize, 0, 0);
		}

		//numbers the platforms
		numberPlatform();

		//places pickup if necessarry
		placePickUp ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//platformNo = GameController.instance.platforms.IndexOf(gameObject);
		//to delete the objects behind the player
		if ( player.transform.position.z - transform.position.z > GameController.instance.getDeletionGap() * GameController.instance.getPlatformGap() )
		{
			GameController.instance.platforms.Remove(gameObject);
			float newZ;
			newZ = GameController.instance.platforms [GameController.instance.platforms.Count - 1].transform.position.z;
			newZ = newZ + GameController.instance.getPlatformGap();
			Vector3 newPosition = new Vector3 ( transform.position.x, transform.position.y, newZ );
			transform.position = newPosition;
			GameController.instance.platforms.Add(gameObject);

			//places pickup if necessarry
			placePickUp();

			//numbers the platforms
			numberPlatform();
		}
	}

	//creates pickUp on the platform
	public void CreatePickUp()
	{
		float randomX = Random.Range ( -transform.lossyScale.x / 2, transform.lossyScale.x / 2 );

		Vector3 pickUpPosition = new Vector3 ( transform.position.x + randomX, PICKUP_Y, transform.position.z );
		Instantiate (pickUp, pickUpPosition, transform.rotation);
	}

	//places pickups accordingly
	public void placePickUp()
	{
		if (platformNo % PICKUP_GAP == 0) 
		{
			CreatePickUp ();
		}
	}

	//numbers the platforms accordingly
	public void numberPlatform()
	{
		platformNo = platformNoS;
		platformNoS++;
	}
}
