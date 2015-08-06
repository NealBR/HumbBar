using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Camera closeUpCamera;
	public Camera mainCamera;

	public Transform goals;
	
	public TouchRecogniser touchRecogniser;

	public GameObject footballObject;
	
	public Text scoreText;


	[HideInInspector]
	public GameObject football;

	bool hitCrossbar;
	float crossbarWaitTime;
	int score;

	// Use this for initialization
	void Start () 
	{
		CreateBall ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.hitCrossbar)
		{
			this.crossbarWaitTime += Time.deltaTime;

			if(this.crossbarWaitTime > 2.0f)
			{
				this.crossbarWaitTime = 0.0f;
				this.hitCrossbar = false;
				CreateBall ();
			}
		}

		if (this.football.transform.position.z > 30)
		{
			this.closeUpCamera.enabled = true;
		}
	}

	void CreateBall()
	{
		if (this.football)
		{
			Destroy (this.football);
		}

		this.closeUpCamera.enabled = false;

		int maxRange = 30;

		this.football = (GameObject)Instantiate (footballObject, new Vector3 (0, 0.5f, 0), Quaternion.identity);
		
		Transform arrow = (Transform)this.football.transform.FindChild ("Arrow");
		Renderer renderer = arrow.GetComponent<Renderer> ();
		renderer.enabled = false;

		this.touchRecogniser.SetFootball (this.football);
		
		Vector3 footballPosition = this.football.transform.position;
		footballPosition = new Vector3 (Random.Range (-maxRange, maxRange), footballPosition.y, footballPosition.z);
		this.football.transform.position = footballPosition;
		
		Vector3 cameraPosition = this.mainCamera.transform.position;
		cameraPosition = new Vector3 (footballPosition.x * 1.2f, cameraPosition.y, cameraPosition.z);
		this.mainCamera.transform.position = cameraPosition;
		this.mainCamera.transform.LookAt (this.goals.position);;
		
		Vector3 lookAt = this.goals.transform.position;
		lookAt.y = 0.0f;
		this.football.transform.LookAt (lookAt);
	}

	public void BallWentDead ()
	{
		if (this.hitCrossbar)
		{
			return;
		}

		print ("BallDead");

		this.score = 0;
		DidSetScore ();

		CreateBall ();
	}
	
	public void BallHitCrossbar ()
	{
		print ("Crossbar");
		this.hitCrossbar = true;
		this.score++;
		DidSetScore ();
	}
	
	public void BallHitPost ()
	{
		print ("Post");
		CreateBall ();
	}

	void DidSetScore ()
	{
		this.scoreText.text = ("Score: " + this.score);
	}
}
