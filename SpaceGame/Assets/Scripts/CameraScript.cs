using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
	public GameObject Target;

	// Use this for initialization
	void Start ()
	{
		this.Target = null;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Target != null)
		{
			transform.position = Target.transform.position + Vector3.up*10;
		}
	}
}
