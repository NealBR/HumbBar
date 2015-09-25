using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {
	
	bool vibrate;
	bool dampen;
	bool forward;
	float currentRot;
	float rot;
	float maxRot;

	// Use this for initialization
	void Start () 
	{
		this.vibrate = false;
		this.dampen = false;
		this.rot = 10;
		this.maxRot = this.rot * 2;
		this.forward = true;
	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		if (this.vibrate)
		{
			float delta = Time.deltaTime * 16;
		
			if (this.forward) {
				this.currentRot += this.rot * delta;
			
				if (this.currentRot >= this.rot) {
					this.currentRot = this.rot;
					this.forward = false;
				
					if (!this.dampen && this.rot != this.maxRot) {
						this.rot = this.maxRot;
					}
				}
			
				float degrees = Mathf.Deg2Rad * this.currentRot;
			
				print (this.currentRot);
				print (this.rot);
			
				this.transform.RotateAround (new Vector3 (this.transform.position.x, this.transform.position.y -1.8f, this.transform.position.z), Vector3.right, degrees);
			} else {
				this.currentRot -= this.rot * delta;
			
				if (this.currentRot <= -this.rot) {				
					this.dampen = true;
					if (this.rot > 0) {
						this.rot *= 0.5f;
						if (this.rot < 1) 
						{
							this.rot = 0;
							this.vibrate = false;
						}
					}
				
					this.currentRot = -this.rot;
					this.forward = true;
				}
			
				float degrees = Mathf.Deg2Rad * this.currentRot;
			
				print (this.currentRot);
				print (this.rot);
			
				this.transform.RotateAround (new Vector3 (this.transform.position.x, this.transform.position.y -1.8f, this.transform.position.z), Vector3.right, degrees);
			}
		}
	}

	public void GoalsWereHit()
	{
		this.vibrate = true;
		this.dampen = false;
		this.rot = 10;
		this.maxRot = this.rot * 2;
		this.forward = true;
	}

	void Update () 
	{

	}
}
