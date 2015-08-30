using UnityEngine;
using System.Collections;

public class FootballScript : MonoBehaviour {

	Vector3 initialForce;
	
	float time;
	float modifier;
	Vector3 speed;
	float gravity;
	float sliderPower;
	
	int minX;
	int maxX;
	int minY;
	int maxY;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!this.initialForce.Equals (new Vector3 (0, 0, 0)))
		{
			this.transform.Rotate (new Vector3 (5, 0, 0));
		}
	
	}

	public void setKickForceWithDirectionAndPower (Vector3 direction_, float power_)
	{
		Transform arrow = (Transform)this.transform.FindChild ("Arrow");
		Renderer renderer = arrow.GetComponent<Renderer> ();
		renderer.enabled = false;

		float minX = this.minX;
		float maxX = this.maxX;
		float xDiff = maxX - minX;

		float minY = this.minY;
		float maxY = this.maxY;
		float yDiff = maxY - minY;

		float xModifier = minX + (xDiff * this.sliderPower);
		float yModifier = maxY - (yDiff * this.sliderPower);
		float zModifier = 15;

		print (this.sliderPower);
		print (xModifier);
		print (yModifier);

		Vector3 force = new Vector3(direction_.x * xModifier, power_ * yModifier, direction_.y * zModifier);
		Rigidbody rigid = GetComponent<Rigidbody>();
		rigid.AddRelativeForce (force);
		this.initialForce = force;
	}

	public void setSliderPower (float power_)
	{
		this.sliderPower = power_;
	}
	
	public void setMinX (int value_)
	{
		this.minX = value_;
	}
	
	public void setMaxX (int value_)
	{
		this.maxX = value_;
	}
	
	public void setMinY (int value_)
	{
		this.minY = value_;
	}
	
	public void setMaxY (int value_)
	{
		this.maxY = value_;
	}
}
