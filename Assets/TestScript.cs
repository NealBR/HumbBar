using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public GameObject football;

	// Use this for initialization
	void Start () {
		
		Vector3 torque = new Vector3(-200, 0, 0);
		Vector3 force = new Vector3(0, 590, 2200);
		Vector3 position = new Vector3(-0.4f, 0.4f, 0);

		Rigidbody footballBody = this.football.GetComponent<Rigidbody> ();
		footballBody.AddForce (force);
		footballBody.AddRelativeTorque (torque);
		//footballBody.AddRelativeForce (force);
	


	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter (Collision col)
	{
		print ("HIT");

		Vector3 torque = new Vector3(200, 0, 0);
		Rigidbody footballBody = this.football.GetComponent<Rigidbody> ();
		footballBody.AddRelativeTorque (torque);
	}
}
