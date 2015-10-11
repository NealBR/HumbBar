using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FootballGameManager : MonoBehaviour {
	
	public DragRecogniser dragRecogniser;
	public SpawnManager spawnManager;

	public Camera mainCamera;

	public GameObject footballPrefab;

	public Transform goalposts;
	
	public Text scoreText;

	GameObject football;

	bool spawningBall;
	int score;

	// Use this for initialization
	void Start () 
	{
		Reset();
	}

	void Reset ()
	{
		this.dragRecogniser.enabled = false;
		this.spawningBall = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!this.football && !this.spawningBall)
		{
			this.spawningBall = true;

			Invoke("SpawnFootball", 1.0f);
		}
	
	}

	void SpawnFootball ()
	{
		if(this.football)
		{
			Destroy(this.football);
		}

		this.spawnManager.Reset();

		this.football = GameObject.Instantiate(this.footballPrefab);
		Vector2 position = this.spawnManager.GetRandomPosition();
		this.football.transform.position = new Vector3(position.x, this.football.transform.localScale.y * 0.5f, position.y);

		FootballMovementScript footballScript = this.football.GetComponent<FootballMovementScript>();
		footballScript.curve = this.spawnManager.GetRandomCurve();
		footballScript.height = this.spawnManager.GetRandomHeight();
		footballScript.totalTime = this.spawnManager.GetRandomTime();

		this.dragRecogniser.football = this.football.GetComponent<PredictionScript>();
		this.dragRecogniser.movingFootball = this.football.GetComponent<FootballMovementScript>();
		this.dragRecogniser.enabled = true;

		PositionCamera();

		this.spawningBall = false;
	}

	void PositionCamera ()
	{		
		float minX = this.goalposts.position.x;

		float width = this.football.transform.position.x;

		float x = minX + (width * 0.5f);
		float z = this.football.transform.position.z;

		this.mainCamera.transform.position = new Vector3(x, this.mainCamera.transform.position.y, z);

		bool onScreen = false;

		while(!onScreen)
		{
			Vector3 position = this.mainCamera.WorldToViewportPoint(this.football.transform.position);

			float xBuffer = 0.05f;
			float zBuffer = 0.05f;

			bool validX = position.x >= xBuffer && position.x <= 1 - xBuffer;
			bool validY = position.y >= 0 && position.y <= 1;
			bool validZ = position.z >= zBuffer;

			if(validX && validY && validZ)
			{
				onScreen = true;
			}
			else
			{
				Vector3 camPosition = this.mainCamera.transform.position;
				this.mainCamera.transform.position = new Vector3(camPosition.x, camPosition.y, camPosition.z - 1);
			}
		}
	}

	public void MorePowerNeeded ()
	{
		
	}

	public void CrossbarHit ()
	{
		this.score++;
		DidSetScore();
		Invoke("SpawnFootball", 1.0f);
	}

	public void NoCrossbarHit ()
	{
		this.score = 0;
		DidSetScore();
		SpawnFootball();
	}
	
	public static void BroadcastToGameManager(string methodName)
	{
		GameObject gObject = GameObject.Find("GameManager");
		gObject.BroadcastMessage(methodName);
	}
	
	void DidSetScore ()
	{
		this.scoreText.text = ("Score: " + this.score);
	}
}
