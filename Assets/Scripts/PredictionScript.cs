using UnityEngine;
using System.Collections;

public class PredictionScript : MonoBehaviour {
	
	public FootballMovementScript football;
	public GameObject tracerSmall;
	public GameObject tracerEnds;

	[Range(0, 100)]
	public int predictionRangePercentage;

	public int numberOfIndicators;

	public float crossbarHeight;
	Vector3 ballStartPosition;

	ArrayList predictors;

	// Use this for initialization
	void Start () {
		
		this.ballStartPosition = this.transform.position;
		this.predictors = new ArrayList();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RemovePredictors ()
	{		
		foreach(GameObject predictor in this.predictors)
		{
			Destroy(predictor);
		}
	}
	
	public void ShowVelocity (Vector2 velocity)
	{
		RemovePredictors();

		velocity = new Vector2(velocity.x * 0.1f, velocity.y * 0.2f);		
		AddPredictorsForVelocity(velocity);
	}
	
	public void SetVelocity (Vector2 velocity)
	{		
		RemovePredictors();

		velocity = new Vector2(velocity.x * 0.1f, velocity.y * 0.2f);
		this.football.SetVelocity(velocity);
	}

	void AddPredictorsForVelocity (Vector2 velocity)
	{
		Vector3 endPosition = new Vector3 (this.ballStartPosition.x + velocity.x, this.crossbarHeight, this.ballStartPosition.z + velocity.y);
		
		Vector3 controlPoint1 = this.football.ControlPoint1ForStartAndEndPosition(this.ballStartPosition, endPosition);
		Vector3 controlPoint2 = this.football.ControlPoint2ForStartAndEndPosition(this.ballStartPosition, endPosition);

		int range = this.predictionRangePercentage;
		int numberOfPredictors = this.numberOfIndicators;

		float mod = (range / numberOfPredictors);
		float index = 0;

		for(int i = 0; i < numberOfPredictors; i++)
		{
			index += mod;
			float percent = index / 100.0f;
			Vector3 position = this.football.PositionOfBezierAtPercent(controlPoint1, controlPoint2, endPosition, percent);
			
			GameObject predictor = GameObject.Instantiate(this.tracerSmall);
			predictor.transform.position = position;

			this.predictors.Add(predictor);
		}

		{
			Vector3 position = this.football.PositionOfBezierAtPercent(controlPoint1, controlPoint2, endPosition, 1.0f);
			
			GameObject predictor = GameObject.Instantiate(this.tracerEnds);
			predictor.transform.position = position;
			
			this.predictors.Add(predictor);
		}
	}
}
