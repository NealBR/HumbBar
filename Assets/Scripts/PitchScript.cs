using UnityEngine;
using System.Collections;

public class PitchScript : MonoBehaviour {
	
	public GameManager gameManager;

	int numberOfBounces;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter (Collision col)
	{
		print ("Pitch Hit");

		this.numberOfBounces++;

		if (this.numberOfBounces > 1)
		{
			this.gameManager.BallWentDead ();
		}
	}

	public void Reset ()
	{
		this.numberOfBounces = 0;
	}
}
