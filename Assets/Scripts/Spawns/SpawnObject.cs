using UnityEngine;
using System.Collections;

public class SpawnObject : MonoBehaviour {
	
	[Range(0, 100)]
	public int spawnWeighting;
	
	[Range(0.0f, 5.0f)]
	public float time;

	[Range(0, 20)]
	public int curve;

	[Range(0, 10)]
	public int height;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
