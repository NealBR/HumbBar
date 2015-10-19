using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

	int totalWeighting;
	Transform spawnTransform;

	// Use this for initialization
	void Start () 
	{
		int count = this.transform.childCount;

		for(int i = 0; i < count; i++)
		{
			Transform cTrans = this.transform.GetChild(i);
			SpawnObject spawnObject = cTrans.GetComponent<SpawnObject>();
			
			this.totalWeighting += spawnObject.spawnWeighting;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Reset ()
	{
		int count = this.transform.childCount;
		int weight = Random.Range(0, this.totalWeighting);
		Transform spawnTransform = null;
		int totalWeighting = 0;

		print ("Weight: " + weight);
		
		for(int i = 0; i < count; i++)
		{
			Transform cTrans = this.transform.GetChild(i);
			SpawnObject spawnObject = cTrans.GetComponent<SpawnObject>();
			
			totalWeighting += spawnObject.spawnWeighting;
			
			if(totalWeighting > weight)
			{
				spawnTransform = cTrans;
				break;
			}
		}

		this.spawnTransform = spawnTransform;
	}

	public Vector2 GetRandomPosition ()
	{
		Transform spawnTransform = this.spawnTransform;

		float width = spawnTransform.localScale.x;
		float height = spawnTransform.localScale.z;

		float minX = spawnTransform.position.x - (width * 0.5f);
		float minZ = spawnTransform.position.z - (height * 0.5f);
		
		float x = minX + Random.Range(0, width);
		float z = minZ + Random.Range(0, height);

		return new Vector2(x, z);
	}
	
	public float GetRandomCurve ()
	{
		SpawnObject spawnObject = this.spawnTransform.GetComponent<SpawnObject>();

		return (float)spawnObject.curve;
	}
	
	public float GetRandomHeight ()
	{
		SpawnObject spawnObject = this.spawnTransform.GetComponent<SpawnObject>();
		
		return (float)spawnObject.height;
	}
	
	public float GetRandomTime ()
	{
		SpawnObject spawnObject = this.spawnTransform.GetComponent<SpawnObject>();
		
		return spawnObject.time;
	}

	public Vector3 GetCameraPositionOffset ()
	{
		SpawnObject spawnObject = this.spawnTransform.GetComponent<SpawnObject>();
		
		return spawnObject.cameraPositionOffset;
	}
	
	public Vector3 GetCameraRotation ()
	{
		SpawnObject spawnObject = this.spawnTransform.GetComponent<SpawnObject>();
		
		return spawnObject.cameraRotationOffset;
	}
}
