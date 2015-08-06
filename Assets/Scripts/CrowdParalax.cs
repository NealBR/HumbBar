using UnityEngine;
using System.Collections;

public class CrowdParalax : MonoBehaviour {
	
	float start;
	float offset;
	float maxOffset;

	// Use this for initialization
	void Start () 
	{
		if (Random.Range (0, 100) < 50) {
			this.maxOffset = 0.5f;
		} else {
			this.maxOffset = -0.5f;
		}

		this.start = this.transform.localPosition.y;

		this.offset = Random.Range (0.0f, this.maxOffset);
		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.start + this.offset, this.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {

		this.offset += this.maxOffset * (Time.deltaTime * 3);

		if (this.maxOffset > 0 && this.offset > this.maxOffset) {
			this.offset = this.maxOffset;
			this.maxOffset *= -1;
		}
		else if (this.maxOffset < 0 && this.offset < this.maxOffset) {
			this.offset = this.maxOffset;
			this.maxOffset *= -1;
		}

		this.transform.localPosition = new Vector3 (this.transform.localPosition.x, this.start + this.offset, this.transform.localPosition.z);
	
	}
}
