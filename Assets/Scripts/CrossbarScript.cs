using UnityEngine;
using System.Collections;

public class CrossbarScript : MonoBehaviour {
	
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
		
		print ("Crossbar");
		print ("Game:" + this.gameManager);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter ()
	{
		print ("Game:" + this.gameManager);
		this.gameManager.BallHitCrossbar ();
		print ("Game 2:" + this.gameManager);
	}
}
