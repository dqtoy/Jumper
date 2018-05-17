using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private const int PLATFORM_SCORE = 1;
	private const int PICKUP_SCORE = 2;
	private const int Y_VELOCITY = 8;
	private const int DROP_HEIGHT = 5;
	private const int Z_VELOCITY_INITIAL = 8;
	private const int MAX_Z_VELOCITY = 16;
	private const int X_SPEED = 10;
	private const int SPEED_UP_GAP = 15;

	private Rigidbody rb;
	private float zVelocity;
	private bool newGame;
	private PlatformController platform;

	//starts object
	void Start()
	{
		//disable multi touch
		Input.multiTouchEnabled = false;

		rb = GetComponent<Rigidbody> ();

		//set the init values for movement
		Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);
		rb.velocity = movement;
		zVelocity = 0;
		newGame = true;
	}

	//for checking physics based updates
	void Update()
	{
		//check if player falls
		if ( transform.position.y < -DROP_HEIGHT )
		{
			GameController.instance.BallFell ();
			rb.velocity = Vector3.zero;
			rb.useGravity = false;
			zVelocity = 0;
		}

		//controls x movement
		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER


		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE

		else if (Input.touchCount == 1) 
		{
			Touch touch = Input.GetTouch (0);

			if ( touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved )
			{
				Vector3 touchPosition = Camera.main.ScreenToWorldPoint ( new Vector3( touch.position.x, touch.position.y, 10 ) );
				Vector3 target = transform.position;
				target.x = touchPosition.x;
				transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * X_SPEED);
			}
		}

		#endif 
	}

	//checks for pickups
	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("PickUp"))
		{
			//destroy object
			Destroy(other.gameObject);

			//score
			IncrementScore( PICKUP_SCORE );
		}
	}
		
	//checks for platforms to jump for score and velocity
	void OnCollisionEnter (Collision other)
	{		
		//if it hits a platform
		if (other.gameObject.CompareTag ("Platform"))
		{
			//checks if it is a new game applies the velocity
			if ( newGame )
			{
				zVelocity = Z_VELOCITY_INITIAL;
				Vector3 movement = new Vector3 (rb.velocity.x, rb.velocity.y, zVelocity);
				rb.velocity = movement;
				newGame = false;
			}

			//controls jumping
			Vector3 jump = new Vector3 (rb.velocity.x, Y_VELOCITY, zVelocity);
			rb.velocity = jump;

			//score
			IncrementScore( PLATFORM_SCORE );

			//speedUp
			platform = other.gameObject.GetComponent<PlatformController>();
			if ( platform.platformNo % SPEED_UP_GAP == 0 )
			{
				if (platform.platformNo >= GameController.instance.getNumberOfPlatforms() ) 
				{
					speedUp ( GameController.instance.getIncrementInVelocity() );
				}
			}

			//platform gap
			if (platform.platformNo % SPEED_UP_GAP == GameController.instance.getDeletionGap() + 1) 
			{
				updatePlatformGap();
			}
		}
	}

	//updates score
	void IncrementScore( int point )
	{
		GameController.instance.setScore(GameController.instance.getScore() + point);
		GameController.instance.SetCountText ();
	}

	//controls speedUp
	public void speedUp( int amount )
	{
		if ( zVelocity < MAX_Z_VELOCITY )
		{
			zVelocity += amount;
			Vector3 movement = new Vector3 (rb.velocity.x, rb.velocity.y, zVelocity);
			rb.velocity = movement;
		}
	}

	//controls platformGap
	public void updatePlatformGap()
	{
		if (zVelocity < MAX_Z_VELOCITY) 
		{
			GameController.instance.updatePlatformGap ();
		}
	}

	//Get and Set Methods

	//returns initial z velocity
	public float getInitialZVelocity()
	{
		return Z_VELOCITY_INITIAL;
	}

	//returns z velocity
	public float getZVelocity()
	{
		return zVelocity;
	}

	//sets the ZVelocity
	public void setZVelocity( float newSpeed ) 
	{
		zVelocity = (int)newSpeed;
	}
}
