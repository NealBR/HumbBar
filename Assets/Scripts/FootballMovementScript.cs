using UnityEngine;
using System.Collections;

public class FootballMovementScript : MonoBehaviour {
	
	public GameManager gameManager;
	
	public GameObject tracer;
	public GameObject tracerSmall;

	[Range(0.0f, 1.0f)]
	public float crossbarCenterOffset;
	
	public Vector3 startPosition;
	public Vector3 endPosition;

	public Vector3 controlPoint1;
	public Vector3 controlPoint2;
	
	public bool tracers;
	public float curve = 5;
	public float height = 3;
	
	[Range(0.0f, 1.0f)]
	public float controlPoint1ZLength = 0.5f;
	[Range(0.0f, 1.0f)]
	public float controlPoint2ZLength = 0.8f;
	
//	[Range(0.0f, 1.0f)]
//	public float controlPoint1Height = 1.0f;
	[Range(0.0f, 1.0f)]
	public float controlPoint2Height = 0.5f;

	public bool finished;
	
	bool kicked;
	bool forced;

	Rigidbody body;
	
	Transform crossbar;

	float previousDelta;
	Vector3 previousPosition;

	float totalTime;
	float currentTime;

	// Use this for initialization
	void Start () {
		
		this.body = this.GetComponent<Rigidbody>();
		this.body.useGravity = false;

		this.forced = false;

		this.currentTime = 0;
		this.totalTime = 1;

		this.crossbar = GameObject.Find ("Crossbar").transform;
			
		float yPos = this.transform.localScale.y * 0.5f;

		if(this.startPosition.y < yPos)
		{
			this.startPosition.y = yPos;
		}
		
		float crossBarRadius = this.crossbar.localScale.z * 0.5f;
		float ballRadius = this.transform.localScale.x * 0.5f;

		float radius = crossBarRadius + ballRadius;
		radius *= 1.1f;

		float xPos = this.crossbar.position.x;

		float offset = this.crossbar.transform.localScale.y - ((this.crossbar.transform.localScale.y * 2) * this.crossbarCenterOffset);
		xPos -= offset;

		print(offset);

//		xPos += this.crossbar.transform.localScale.y * this.crossbarCenterOffset;

		this.endPosition = new Vector3 (xPos, this.crossbar.position.y + radius * 0.4f, this.crossbar.position.z - radius);

		float xLength = this.endPosition.x - this.startPosition.x;
		float zLength = this.endPosition.z - this.startPosition.z;

		this.controlPoint1 = new Vector3 (this.startPosition.x + xLength + this.curve, this.endPosition.y + this.height, this.startPosition.z + (zLength * this.controlPoint1ZLength));
		this.controlPoint2 = new Vector3 (this.startPosition.x + xLength + (this.curve * 0.6f), this.endPosition.y + (this.height * this.controlPoint2Height), this.startPosition.z + (zLength * this.controlPoint2ZLength));

		if(this.tracers)
		{
			{
				GameObject tracer = GameObject.Instantiate(this.tracer);
				tracer.transform.position = this.endPosition;
			}

			{	
				GameObject tracer = GameObject.Instantiate(this.tracer);
				tracer.transform.position = this.startPosition;
			}

			{	
				GameObject tracer = GameObject.Instantiate(this.tracer);
				tracer.transform.position = this.controlPoint1;
			}

			{	
				GameObject tracer = GameObject.Instantiate(this.tracer);
				tracer.transform.position = this.controlPoint2;
			}

			{	
				GameObject tracer = GameObject.Instantiate(this.tracer);
				tracer.transform.position = this.endPosition;
			}
		}

		Color c1 = Color.red;
		Color c2 = Color.red;
		int lengthOfLineRenderer = 4;
		
		if(this.tracers)
		{
			LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
			lineRenderer.SetColors(c1, c2);
			lineRenderer.SetWidth(0.2F, 0.2F);
			lineRenderer.SetVertexCount(lengthOfLineRenderer);
			lineRenderer.SetPosition(0, this.startPosition);
			lineRenderer.SetPosition(1, this.controlPoint1);
			lineRenderer.SetPosition(2, this.controlPoint2);
			lineRenderer.SetPosition(3, this.endPosition);
		}

		Invoke("ShouldKick", 2.0f);
	}

	void ShouldKick()
	{
		this.kicked = true;
	}
	
	// Update is called once per frame
	void Update () {

		if(!this.kicked)
		{
			return;
		}

		if(this.finished)
		{
			if(!this.forced)
			{
				print(this.previousPosition);
				print(this.transform.position);

				AddForceAfterBezier();
			}
			return;
		}
		
		if(this.tracers)
		{
			GameObject tracer = GameObject.Instantiate(this.tracerSmall);
			tracer.transform.position = this.transform.position;
		}

		float deltaTime = Time.deltaTime;
		this.currentTime += deltaTime;

		if(this.currentTime > this.totalTime)
		{
			this.currentTime = this.totalTime;
			this.finished = true;
		}

		MoveBallViaBezier(deltaTime);
	}

	void AddForceAfterBezier () 
	{
		float x1 = this.controlPoint2.x;
		float y1 = this.controlPoint2.y;
		float z1 = this.controlPoint2.z;
		
		float x2 = this.endPosition.x;
		float y2 = this.endPosition.y;
		float z2 = this.endPosition.z;
		
		float xLength = x2 - x1;
		float yLength = y2 - y1;
		float zLength = z2 - z1;
		
		//float modifier = 1.0f / this.previousDelta;
		
		float total = xLength + yLength + zLength;
		
		float px = xLength / total;
		float py = yLength / total;
		float pz = zLength / total;
		
		float xModifier = 5;
		float yModifier = 2;
		float zModifier = 14;
		
		float x = px * xModifier;
		float y = py * yModifier;
		float z = pz * zModifier;
		
		print("X: " + x + "Y: " +  + y + "Z: " +  + z);
		
		//				float modifier = 1;
		//
		//				float x = xLength * modifier;
		//				float y = yLength * modifier;
		//				float z = zLength * modifier;
		//
		//				print("X: " + x + "Y: " +  + y + "Z: " +  + z + "Mod: " + modifier);
		
		this.body.useGravity = true;
		this.body.AddForce(x, y, z, ForceMode.Impulse);
		
		this.forced = true;
	}

	void MoveBallViaBezier (float deltaTime)
	{
		float percent = this.currentTime / this.totalTime;

		float x1 = this.startPosition.x;
		float x2 = this.controlPoint1.x;
		float x3 = this.controlPoint2.x;
		float x4 = this.endPosition.x;
		
		float y1 = this.startPosition.y;
		float y2 = this.controlPoint1.y;
		float y3 = this.controlPoint2.y;
		float y4 = this.endPosition.y;
		
		float z1 = this.startPosition.z;
		float z2 = this.controlPoint1.z;
		float z3 = this.controlPoint2.z;
		float z4 = this.endPosition.z;
		
		float xaa = getPt( x1 , x2 , percent );
		float yaa = getPt( y1 , y2 , percent );
		float zaa = getPt( z1 , z2 , percent );
		float xab = getPt( x2 , x3 , percent );
		float yab = getPt( y2 , y3 , percent );
		float zab = getPt( z2 , z3 , percent );
		
		float xba = getPt( x2 , x3 , percent );
		float yba = getPt( y2 , y3 , percent );
		float zba = getPt( z2 , z3 , percent );
		float xbb = getPt( x3 , x4 , percent );
		float ybb = getPt( y3 , y4 , percent );
		float zbb = getPt( z3 , z4 , percent );
		
		float xa = getPt( xaa , xab , percent );
		float ya = getPt( yaa , yab , percent );
		float za = getPt( zaa , zab , percent );
		
		float xb = getPt( xba , xbb , percent );
		float yb = getPt( yba , ybb , percent );
		float zb = getPt( zba , zbb , percent );
		
		float x = getPt( xa , xb , percent );
		float y = getPt( ya , yb , percent );
		float z = getPt( za , zb , percent );
		
		this.previousDelta = deltaTime;
		this.previousPosition = this.transform.position;
		
		this.transform.position = new Vector3(x, y, z);
	}

	float getPt(float n1, float n2, float perc)
	{
		float diff = n2 - n1;
		
		return n1 + ( diff * perc );
	} 
}
