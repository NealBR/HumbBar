using UnityEngine;
using System.Collections;

public class CrossbarScript : MonoBehaviour {
	
	public GameManager gameManager;
	public GoalScript goalScript;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
	{
		//this.gameManager.BallHitCrossbar ();
		//this.goalScript.GoalsWereHit ();
	}
}
