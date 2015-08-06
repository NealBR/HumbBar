using UnityEngine;
using System.Collections;

public class FootballScript : MonoBehaviour {

	Vector3 initialForce;
	
	float time;
	float modifier;
	Vector3 speed;
	float gravity;

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

		float modifier = 3;

		Vector3 force = new Vector3(direction_.x * modifier * 5, power_ * modifier, direction_.y * 5 * modifier);
		Rigidbody rigid = GetComponent<Rigidbody>();
		rigid.AddRelativeForce (force);
		this.initialForce = force;
	}
}
