using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.AddForce(Vector3.left * Input.GetAxis("Horizontal") * -10);
    }
}
