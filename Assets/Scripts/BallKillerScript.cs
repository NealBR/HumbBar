using UnityEngine;
using System.Collections;

public class BallKillerScript : MonoBehaviour {

	public GameManager gameManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter ()
	{
		this.gameManager.BallWentDead ();
	}
}
