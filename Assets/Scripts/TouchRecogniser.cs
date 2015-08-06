using UnityEngine;
using System.Collections;

public class TouchRecogniser : MonoBehaviour {

	public GameManager gameManager;
	
	GameObject football;
	FootballScript footballScript;

	Transform arrow;

	bool isDragging;
	Vector3 startPosition;
	Vector3 difference;

	// Use this for initialization
	void Start () 
	{
		startPosition = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (this.isDragging) 
		{			
			Renderer renderer = this.arrow.GetComponent<Renderer> ();
			renderer.enabled = true;

			Vector3 mousePosition = Input.mousePosition;
			GotMousePosition(mousePosition);
		}

		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
		{
			// Get movement of the finger since last frame
			Vector2 delta = Input.GetTouch(0).deltaPosition;
			GotDeltaPosition(delta);
		}

		if (Input.GetMouseButtonDown (0))
		{
			this.isDragging = true;
			Vector3 mousePosition = Input.mousePosition;
			GotMousePosition(mousePosition);
		}
		
		if (Input.GetMouseButtonUp (0))
		{
			this.isDragging = false;
			Vector3 mousePosition = Input.mousePosition;
			GotMousePosition(mousePosition);

			float power = Vector3.Distance(this.startPosition, mousePosition);
			KickFootballWithDirectionAndPower(this.difference, power);
		}
	}

	void GotMousePosition (Vector3 mousePosition)
	{
		if (this.startPosition.Equals (new Vector3 (0, 0, 0)))
		{
			this.startPosition = mousePosition;
			return;
		}

		this.difference = new Vector3(this.startPosition.x - mousePosition.x,this.startPosition.y - mousePosition.y, this.startPosition.z - mousePosition.z);

		float distance = Vector3.Distance(this.startPosition, mousePosition);
		float maxDistance = 100;

		if (distance > maxDistance) {
			//distance = maxDistance;
		}

		float scale = (distance / maxDistance) * 3.0f;

		this.arrow.localScale = new Vector3 (0.25f, 1.0f, scale);
		this.arrow.transform.localPosition = new Vector3 (1.0f, 1.0f, scale * 3.0f);
		this.arrow.localRotation = Quaternion.LookRotation (new Vector3(-this.difference.x, -this.difference.z, -this.difference.y));
	}
	
	void GotDeltaPosition (Vector2 delta_)
	{
		//print("hello");
	}

	void KickFootballWithDirectionAndPower (Vector3 direction_, float power_)
	{
		this.footballScript.setKickForceWithDirectionAndPower (direction_, power_);

		startPosition = new Vector3 (0, 0, 0);
	}

	public void SetFootball (GameObject football_)
	{
		//print ("football set");
		this.football = football_;
		this.footballScript = football_.GetComponent<FootballScript>();

		this.arrow = (Transform)this.football.transform.FindChild ("Arrow");
	}
}
