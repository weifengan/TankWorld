using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    public float RotateSpeed = 50;
    private Vector3 lastPosition;
    private Vector3 lastEulerAngle;
    private Quaternion lastRotation;
    private Rigidbody rig;


    public float vx=0;
    public float vy = 0;
 
    // Use this for initialization
    void Start () {
        rig = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

         vx = Input.GetAxis("Horizontal");
         vy = Input.GetAxis("Vertical");

    }
}
