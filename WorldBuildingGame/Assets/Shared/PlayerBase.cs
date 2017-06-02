using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour {

	Vector3 movement;                   // The vector to store the direction of the player's movement.
	Vector3 rotation; 

	public float speed;
	float rotSpeed = 100;

	int health;


	// Use this for initialization
	void Start () {
		health = 100;

	}

	// Update is called once per frame
	void Update () {
		// Move the player around the scene.
		if (health >= 1) {
			Move ();
			Rotation ();
		} else {
			Debug.Log ("deed");
		}
	}

	void Move ()
	{
		if (Input.GetAxisRaw ("Vertical") >= .01f) {
			movement = transform.TransformDirection (Vector3.forward) * speed * Time.deltaTime;
		} else if (Input.GetAxisRaw ("Vertical") <= -.01f) {
			movement = transform.TransformDirection (-Vector3.forward) * speed * Time.deltaTime;
		} else if (Input.GetAxisRaw ("Vertical") == 0f) {
			movement = movement - movement;
		}

		transform.position = (transform.position + movement);
	}

	void Rotation ()
	{
		if (Input.GetAxisRaw ("Horizontal") >= .01f) {
			transform.Rotate (Vector3.up * rotSpeed * Time.deltaTime);
		} else if (Input.GetAxisRaw ("Horizontal") <= -.01f) {
			transform.Rotate (-Vector3.up * rotSpeed * Time.deltaTime);
		} else if (Input.GetAxisRaw ("Horizontal") == 0f) {
			//do nothing
		}
	}
}
