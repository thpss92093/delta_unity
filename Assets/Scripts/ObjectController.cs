using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {
    Rigidbody rb;
    public float pushForce;
    public bool isMoving;
    Vector3 prevLocation;
    // Use this for initialization
    void Start() {
        rb = GetComponent<Rigidbody>();
        prevLocation = transform.position;
    }

    // Update is called once per frame
    void Update() {

    }

    public void FixedUpdate()
    {
        if (prevLocation != transform.position)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        prevLocation = transform.position;
    }

    public void Push()
    {
        // can be improve by better way
        //Vector3 force = Random.insideUnitSphere * pushForce;

        Vector3 force = Random.insideUnitSphere * pushForce;
        force.y = transform.position.y;
        // Debug.Log(force);
        rb.AddForce(force, ForceMode.Impulse);
        Debug.DrawLine(transform.position, transform.right, Color.red, 2f);

        //        Vector3 up = transform.up * Random.value - transform.up / 2;
        //        Vector3 forward = transform.forward * Random.value - transform.forward / 2;
        //        Vector3 right = transform.right * Random.value - transform.right / 2;
        //        Vector3 rndF = (up + forward + right) * pushForce;
        //Debug.DrawLine(transform.position, transform.position + rndF, Color.green, 2f);
        //rb.AddForce(rndF, ForceMode.Impulse);

        isMoving = true;
    }

    public void Place()
    {
        //obj: face_2 rotation_5 position_1    cam: rotation_15 height_3 angle_3
        //position 1
        transform.position = new Vector3(Random.Range(-100.0f, 100.0f) * 0.001f, 0.2f, Random.Range(-100.0f, 100.0f) * 0.001f);
        // Debug.Log("transform.position = " + transform.position.ToString("f4"));
        //Debug.Log("eulerAngles = " + transform.eulerAngles);
        isMoving = true;
    }

    public void Place_away()
    {
        transform.position = new Vector3(1.0f, 0.1f, 2.0f);
    }

    public void Reset()
	{
		transform.position = new Vector3 (Random.Range(-100.0f, 100.0f) * 0.001f, 0.2f, Random.Range(-100.0f, 100.0f) * 0.001f);
        isMoving = true;
        Debug.Log("transform.position = " + transform.position);
    }
}
