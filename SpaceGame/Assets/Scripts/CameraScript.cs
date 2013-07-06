using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

	public GameObject Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Target.transform.position + Vector3.up * 10;
	}
}
