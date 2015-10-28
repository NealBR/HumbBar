using UnityEngine;
using System.Collections;

public class FootballSpriteScript : MonoBehaviour {

	public GameObject camera;

	public Sprite[] sprites;

	public bool wasHit;

	// Use this for initialization
	void Start () {
		this.camera = GameObject.Find ("Main Camera");

		int random = Random.Range(0, sprites.Length);

		SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
		sprite.sprite = sprites[random];
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.LookAt(this.camera.transform);
	
		if(this.wasHit)
		{
			this.transform.Rotate(0, 0, Random.Range(0, 360));
		}
	}
}
