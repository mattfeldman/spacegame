using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.rigidbody.velocity = new Vector3(0, 0, 10);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
