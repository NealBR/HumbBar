using UnityEngine;
using System.Collections;

public class DragRecogniser : MonoBehaviour {

	public PredictionScript football;
	public FootballMovementScript movingFootball;
	
	bool isDragging;
	Vector3 startPosition;
	Vector3 difference;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update() 
	{		
		if(this.movingFootball.WasKicked())
		{
			print("WAS KICKED");
			return;
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
			this.startPosition = mousePosition;
		}
		
		if(this.isDragging) 
		{						
			Vector3 mousePosition = Input.mousePosition;
			IsDraggingToPosition(mousePosition);
		}
		
		if (Input.GetMouseButtonUp (0))
		{
			this.isDragging = false;
			Vector3 mousePosition = Input.mousePosition;
			EndDrag(mousePosition);
		}
	}

	void Reset ()
	{
		this.isDragging = false;
		this.startPosition = Vector3.zero;
		this.difference = Vector3.zero;
	}
	
	void GotDeltaPosition (Vector2 delta_)
	{
		//print("hello");
	}
	
	void IsDraggingToPosition (Vector3 mousePosition)
	{		
		this.difference = new Vector3(this.startPosition.x - mousePosition.x,this.startPosition.y - mousePosition.y, this.startPosition.z - mousePosition.z);
		float distance = Vector3.Distance(this.startPosition, mousePosition);

		Vector2 deltaPosition = new Vector2(this.difference.x, this.difference.y);
		this.football.ShowVelocity(deltaPosition);
	}

	void EndDrag (Vector3 mousePosition)
	{
		this.difference = new Vector3(this.startPosition.x - mousePosition.x,this.startPosition.y - mousePosition.y, this.startPosition.z - mousePosition.z);
		float distance = Vector3.Distance(this.startPosition, mousePosition);
		
		Vector2 deltaPosition = new Vector2(this.difference.x, this.difference.y);
		this.football.SetVelocity(deltaPosition);
		
		Reset();
	}
}
