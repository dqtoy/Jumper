using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.Random;

public class GameController : MonoBehaviour {

	public static GameController instance;

	private const float SPEED_MULTIPLIER = 1.64f;
	private const float PLATFORM_Y = -0.5f;
	private const int NUMBER_OF_PLATFORMS = 15;
	private const int INCREMENT_IN_VELOCITY = 2;
	private const int DELETION_GAP = 2;
	private const int MAX_PLATFORM_X = 13;
	private const int MIN_PLATFORM_X = -MAX_PLATFORM_X;

	public Text countText;
	public Text loseText;
	public Button restartButton;
	public Button mainMenuButton;
	public bool gameOver = false;

	public List<GameObject> platforms;
	public GameObject platform;

	private PlayerController player;
	private int score;
	public float platformGap;

	//to avoid multiple game controllers
	void Awake()
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}
	}

	//Sets up to game at the start inits all values
	void Start () 
	{
		player = FindObjectOfType<PlayerController>();

		PlatformController.platformNoS = 1;

		platformGap = SPEED_MULTIPLIER * player.getInitialZVelocity ();
		platforms = new List<GameObject> ();
		platforms.Clear ();
		SetUpGame ();
			
		score = 0;
		SetCountText ();

		loseText.gameObject.SetActive (false);
		restartButton.gameObject.SetActive (false);
		mainMenuButton.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if game over restarts the scene with button
		if (gameOver == true) 
		{
			loseText.gameObject.SetActive (true);
			restartButton.gameObject.SetActive (true);
			mainMenuButton.gameObject.SetActive (true);
		}
	}

	//game over method
	public void BallFell()
	{
		loseText.text = "You Lose!";
		gameOver = true;
	}

	//Sets the count text
	public void SetCountText()
	{
		countText.text = "Score: " + score;
	}

	//sets the first set of platforms
	public void SetUpGame()
	{
		//first batch of platforms
		platforms.Add( Instantiate (platform, new Vector3( 0, PLATFORM_Y, 0 ), transform.rotation) );
		for (int i = 1; i < NUMBER_OF_PLATFORMS; i++)
		{
			float randomX = Random.Range ( MIN_PLATFORM_X, MAX_PLATFORM_X );
			platforms.Add( Instantiate (platform, new Vector3( randomX, PLATFORM_Y, platformGap * i ), transform.rotation) );
		}

	}

	//updates the plaformGap to keep up with the speed up
	public void updatePlatformGap()
	{
		platformGap += SPEED_MULTIPLIER * INCREMENT_IN_VELOCITY;
	}

	//restarts game
	public void restart()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

	//goes to the mainMenu
	public void mainMenu()
	{
		SceneManager.LoadScene (0);
	}

	//get set methods

	//returns increment value of velocity
	public int getIncrementInVelocity()
	{
		return INCREMENT_IN_VELOCITY;
	}

	//returns score
	public int getScore()
	{
		return score;
	}

	//sets score
	public void setScore( int newScore )
	{
		score = newScore;
	}

	//returns platformGap
	public float getPlatformGap()
	{
		return platformGap;
	}

	//returns number of platforms
	public int getNumberOfPlatforms()
	{
		return NUMBER_OF_PLATFORMS;
	}

	//returns deletion gap
	public int getDeletionGap()
	{
		return DELETION_GAP;
	}
}
