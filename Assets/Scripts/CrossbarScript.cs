using UnityEngine;
using System.Collections;

public class CrossbarScript : MonoBehaviour {
	
	public GameManager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter ()
	{
		this.gameManager.BallHitCrossbar ();
	}
}
