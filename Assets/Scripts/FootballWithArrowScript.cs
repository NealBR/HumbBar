using UnityEngine;
using System.Collections;

public class FootballWithArrowScript : FootballScript {

	public void setKickForceWithDirectionAndPower (Vector3 direction_, float power_)
	{
		Transform arrow = (Transform)this.transform.FindChild ("Arrow");
		Renderer renderer = arrow.GetComponent<Renderer> ();
		renderer.enabled = false;

		float minX = this.minX;
		float maxX = this.maxX;
		float xDiff = maxX - minX;

		float minY = this.minY;
		float maxY = this.maxY;
		float yDiff = maxY - minY;

		float xModifier = minX + (xDiff * this.sliderPower);
		float yModifier = maxY - (yDiff * this.sliderPower);
		float zModifier = 15;

		print (this.sliderPower);
		print (xModifier);
		print (yModifier);

		Vector3 force = new Vector3(direction_.x * xModifier, power_ * yModifier, direction_.y * zModifier);
		Rigidbody rigid = GetComponent<Rigidbody>();
		rigid.AddRelativeForce (force);
		this.initialForce = force;
	}
}
