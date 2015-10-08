using UnityEngine;
using System.Collections;

public class FootballMovementScript : MonoBehaviour {
	
	public GameManager gameManager;

	public PredictionScript predictor;
	
	public GameObject tracer;
	public GameObject tracerSmall;

	[Range(0.0f, 1.0f)]
	public float crossbarCenterOffset;
	public float currentTime;
	
	[Range(0.0f, 1.0f)]
	public float ratioForTimeScale = 0.5f;
	
	[Range(0.0f, 1.0f)]
	public float timeScaleToDrop = 1.0f;
	
	public float totalTime;
	
	Vector3 startPosition;
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

	public int type;

	public bool finished;
	
	bool kicked;
	bool forced;

	Rigidbody body;
	
	Transform crossbar;

	float previousDelta;
	Vector3 previousPosition;

	// Use this for initialization
	void Start () {

		this.startPosition = this.transform.position;
		this.body = this.GetComponent<Rigidbody>();

		this.crossbar = GameObject.Find ("Crossbar").transform;
		this.predictor.crossbarHeight = this.crossbar.position.y;
			
		float yPos = this.transform.localScale.y * 0.5f;

		if(this.startPosition.y < yPos)
		{
			this.startPosition.y = yPos;
		}

		if(this.tracers)
		{
			AddTracers();
		}
	}

	public bool WasKicked ()
	{
		return this.kicked;
	}

	public void SetVelocity (Vector2 velocity)
	{		
		this.endPosition = new Vector3 (this.startPosition.x + velocity.x, this.crossbar.position.y, this.startPosition.z + velocity.y);
		
		float xLength = this.endPosition.x - this.startPosition.x;
		float zLength = this.endPosition.z - this.startPosition.z;
		
		this.controlPoint1 = ControlPoint1ForStartAndEndPosition(this.startPosition, this.endPosition);
		this.controlPoint2 = ControlPoint2ForStartAndEndPosition(this.startPosition, this.endPosition);
	
		this.kicked = true;	
	}

	public Vector3 ControlPoint1ForStartAndEndPosition (Vector3 startPosition, Vector3 endPosition)
	{
		float xLength = endPosition.x - startPosition.x;
		float zLength = endPosition.z - this.startPosition.z;

		Vector3 controlPoint1 = new Vector3 (startPosition.x + xLength, endPosition.y + this.height, startPosition.z + (zLength * this.controlPoint1ZLength));
		return controlPoint1;
	}
	
	public Vector3 ControlPoint2ForStartAndEndPosition (Vector3 startPosition, Vector3 endPosition)
	{
		float xLength = endPosition.x - startPosition.x;
		float zLength = endPosition.z - this.startPosition.z;
		
		Vector3 controlPoint2 = new Vector3 (startPosition.x + xLength, endPosition.y + (this.height * this.controlPoint2Height), startPosition.z + (zLength * this.controlPoint2ZLength));
		return controlPoint2;
	}

	void AddTracers ()
	{				
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

		Color c1 = Color.red;
		Color c2 = Color.red;
		int lengthOfLineRenderer = 4;

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

	void Reset()
	{
		Time.timeScale = 1.0f;
		this.body.useGravity = false;
		this.kicked = false;
		this.currentTime = 0.0f;
		this.finished = false;
		this.forced = false;
		this.transform.position = this.startPosition;
		this.body.velocity = Vector3.zero;
		this.body.angularVelocity = Vector3.zero;
		this.transform.rotation = Quaternion.identity;
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

				Invoke("Reset", 2.0f);
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

		if(this.currentTime > this.totalTime * this.ratioForTimeScale)
		{
			if(this.timeScaleToDrop != Time.timeScale)
			{
				Time.timeScale = this.timeScaleToDrop;
			}
		}

		MoveBallViaBezier(deltaTime);
	}

	void AddForceAfterBezier () 
	{
		this.body.velocity = Vector3.zero;
		this.body.angularVelocity = Vector3.zero;
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
		
		this.body.velocity = Vector3.zero;
		this.body.angularVelocity = Vector3.zero;
		this.body.useGravity = true;
		this.body.AddForce(x, y, z, ForceMode.Impulse);
		
		this.forced = true;
	}

	void MoveBallViaBezier (float deltaTime)
	{
		float percent = 0.0f;

		if(this.type == 0)
		{
			percent = Linear(this.currentTime, 0, 1, this.totalTime);
		}
		else if(this.type == 1)
		{
			percent = SineOut(this.currentTime, 0, 1, this.totalTime);
		}
		else if(this.type == 2)
		{
			percent = CubeOut(this.currentTime, 0, 1, this.totalTime);
		}
		else if(this.type == 3)
		{
			percent = QuadOut(this.currentTime, 0, 1, this.totalTime);
		}
		else if(this.type == 4)
		{
			percent = ExpoOut(this.currentTime, 0, 1, this.totalTime);
		}
		else if(this.type == 4)
		{
			percent = CircOut(this.currentTime, 0, 1, this.totalTime);
		}

		this.previousDelta = deltaTime;
		this.previousPosition = this.transform.position;
		
		Vector3 position = PositionOfBezierAtPercent(this.controlPoint1, this.controlPoint2, this.endPosition, percent);
		this.transform.position = position;
	}

	public Vector3 PositionOfBezierAtPercent (Vector3 controlPoint1, Vector3 controlPoint2, Vector3 endPosition,  float percent)
	{
		float x1 = this.startPosition.x;
		float x2 = controlPoint1.x;
		float x3 = controlPoint2.x;
		float x4 = endPosition.x;
		
		float y1 = this.startPosition.y;
		float y2 = controlPoint1.y;
		float y3 = controlPoint2.y;
		float y4 = endPosition.y;
		
		float z1 = this.startPosition.z;
		float z2 = controlPoint1.z;
		float z3 = controlPoint2.z;
		float z4 = endPosition.z;
		
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

		return new Vector3(x, y, z);
	}

	float getPt(float n1, float n2, float perc)
	{
		float diff = n2 - n1;
		
		return n1 + ( diff * perc );
	} 
	
	float Linear(float t, float b, float c, float d)
	{
		return c*t/d + b;
	}
	
	float ExpoOut(float t, float b, float c, float d)
	{
		return c * ( -Mathf.Pow( 2, -10 * t/d ) + 1 ) + b;
	}
	
	float SineOut(float t, float b, float c, float d)
	{
		return c * Mathf.Sin(t/d * (Mathf.PI/2)) + b;
	}

	float QuadOut(float t, float b, float c, float d)
	{
		t /= d;
		return -c * t*(t-2) + b;
	}
	
	float CubeOut(float t, float b, float c, float d)
	{
		t /= d;
		t--;
		return c*(t*t*t + 1) + b;
	}
	
	float CircOut(float t, float b, float c, float d)
	{
		t /= d;
		t--;
		return c * Mathf.Sqrt(1 - t*t) + b;
	}
}
