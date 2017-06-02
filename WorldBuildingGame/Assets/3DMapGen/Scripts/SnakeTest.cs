using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SnakeTest : MonoBehaviour {

	const float dist = 2;
	bool coll = false;
	List<Transform> transList = new List<Transform>();

	void Update () {
		ChainLinkList ();
	}
			
	void ChainLinkList(){
		Vector3 prevLocation = transform.position;		//set the prevLocation to the currant Position 

		foreach (Transform chainList in transList) {
			Vector3 direction = (prevLocation - chainList.position).normalized; //set direction to normalised version of position - the prevLocation
			chainList.position = prevLocation - direction * dist;
			prevLocation = chainList.position;
		}
	}

	void OnCollisionEnter(){
		coll = true;

		if (coll) {
			GameObject tail = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Destroy (tail.GetComponent<SphereCollider> ());
			transList.Add (tail.transform);
		}
	}

	void OnColisionExit(){
		coll = false;
	}
		
}