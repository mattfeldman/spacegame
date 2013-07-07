using UnityEngine;
using System.Collections;

public class BaseBullet : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		this.rigidbody.velocity = this.transform.forward*10;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
