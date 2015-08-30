using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Slider powerSlider;
	public InputField minXInput;
	public InputField maxXInput;
	public InputField minYInput;
	public InputField maxYInput;

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
	FootballScript footballScript;
	float sliderPower;
	
	int minX;
	int maxX;
	int minY;
	int maxY;

	// Use this for initialization
	void Start () 
	{
		this.sliderPower = 1.0f;
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

		this.minX = int.Parse(this.minXInput.text);
		this.maxX = int.Parse(this.maxXInput.text);
		this.minY = int.Parse(this.minYInput.text);
		this.maxY = int.Parse(this.maxYInput.text);

		int maxXRange = 30;

		int minZRange = 15;
		int maxZRange = 35;

		this.football = (GameObject)Instantiate (footballObject, new Vector3 (0, 0.5f, 0), Quaternion.identity);
		
		Transform arrow = (Transform)this.football.transform.FindChild ("Arrow");
		Renderer renderer = arrow.GetComponent<Renderer> ();
		renderer.enabled = false;

		this.touchRecogniser.SetFootball (this.football);
		
		Vector3 footballPosition = this.football.transform.position;
		footballPosition = new Vector3 (Random.Range (-maxXRange, maxXRange), footballPosition.y, Random.Range (minZRange, maxZRange));
		this.football.transform.position = footballPosition;
		
		Vector3 cameraPosition = this.mainCamera.transform.position;
		cameraPosition = new Vector3 (footballPosition.x * 1.2f, cameraPosition.y, footballPosition.z - 20);
		this.mainCamera.transform.position = cameraPosition;

		this.mainCamera.transform.LookAt (this.goals.position);
		Quaternion cameraRotation = this.mainCamera.transform.localRotation;
		this.mainCamera.transform.Rotate (5, 0, 0);
		//cameraRotation.x = 20;
		//this.mainCamera.transform.rotation = cameraRotation;
		
		Vector3 lookAt = this.goals.transform.position;
		lookAt.y = 0.0f;
		this.football.transform.LookAt (lookAt);

		this.footballScript = this.football.GetComponent<FootballScript>();
		this.footballScript.setSliderPower (this.sliderPower);
		this.footballScript.setMinX(this.minX);
		this.footballScript.setMaxX(this.maxX);
		this.footballScript.setMinY(this.minY);
		this.footballScript.setMaxY(this.maxY);
	}

	public void BallWentDead ()
	{
		if (this.hitCrossbar)
		{
			return;
		}

		//print ("BallDead");

		this.score = 0;
		DidSetScore ();

		CreateBall ();
	}

	public void SliderChanged (float sliderValue_)
	{
		this.sliderPower = sliderValue_;
		this.footballScript.setSliderPower (sliderValue_);
	}

	public void MinXChanged (string fieldValue_)
	{
		this.minX = int.Parse(fieldValue_);
		this.footballScript.setMinX(this.minX);
	}
	
	public void MaxXChanged (string fieldValue_)
	{
		this.maxX = int.Parse(fieldValue_);
		this.footballScript.setMaxX(this.maxX);
	}
	
	public void MinYChanged (string fieldValue_)
	{
		this.minY = int.Parse(fieldValue_);
		this.footballScript.setMinY(this.minY);
	}
	
	public void MaxYChanged (string fieldValue_)
	{
		this.maxY = int.Parse(fieldValue_);
		this.footballScript.setMaxY(this.maxY);
	}
	
	public void BallHitCrossbar ()
	{
		//print ("Crossbar");
		this.hitCrossbar = true;
		this.score++;
		DidSetScore ();
	}
	
	public void BallHitPost ()
	{
		//print ("Post");
		CreateBall ();
	}

	void DidSetScore ()
	{
		this.scoreText.text = ("Score: " + this.score);
	}
}
