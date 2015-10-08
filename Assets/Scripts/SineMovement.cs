using UnityEngine;
using System.Collections;

public class SineMovement : MonoBehaviour {
	
	public GameObject tracerSmall;

	public Vector3 startPosition;

	public float maxY;
	public float maxZ;

	public float maxTime;

	public int type;

	bool repeated;
	
	float currentTime;

	float currentTracerTime;
	float tracerSpawnTime;

	ArrayList tracers;

	// Use this for initialization
	void Start () {

		this.tracers = new ArrayList();
		this.tracerSpawnTime = 0.05f;
	}
	
	// Update is called once per frame
	void Update () {
	
		this.currentTime += Time.deltaTime;
		this.currentTracerTime += Time.deltaTime;

		if(this.currentTime > this.maxTime)
		{
			if(this.repeated)
			{
				return;
			}
			foreach(GameObject gTracer in this.tracers)
			{
				Destroy(gTracer);
			}

			this.currentTime -= this.maxTime;
			this.repeated = true;
		}

		float timePercentage = this.currentTime / this.maxTime;
		
		float none = NoTween(this.currentTime, 0, 1, this.maxTime);
		float sine = SineOut(this.currentTime, 0, 1, this.maxTime);
		float quad = QuadOut(this.currentTime, 0, 1, this.maxTime);
		float expo = ExpoOut(this.currentTime, 0, 1, this.maxTime);

		float perc = none;

		if(this.type == 1)
		{
			perc = sine;
		}
		
		if(this.type == 2)
		{
			perc = quad;
		}
		
		if(this.type == 3)
		{
			perc = expo;
		}
		
		float y = this.maxY * perc; //Mathf.Sin(angle);
		float z = this.maxZ * timePercentage; //Mathf.Cos(angle);

		Vector3 position = new Vector3(this.startPosition.x, this.startPosition.y + y, this.startPosition.z + z);
		this.transform.position = position;

		if(this.currentTracerTime > this.tracerSpawnTime)
		{
			this.currentTracerTime -= this.tracerSpawnTime;

			GameObject tracer = GameObject.Instantiate(this.tracerSmall);
			tracer.transform.position = this.transform.position;

			this.tracers.Add(tracer);
		}
	}

	float NoTween(float t, float b, float c, float d)
	{
		return c*t/d + b;
	}
	
	float QuadOut(float t, float b, float c, float d)
	{
		t /= d;
		return -c * t*(t-2) + b;
	}
	
	float ExpoOut(float t, float b, float c, float d)
	{
		return c * ( -Mathf.Pow( 2, -10 * t/d ) + 1 ) + b;
	}
	
	float SineOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t/d * (Mathf.PI/2)) + b;
	}
}
