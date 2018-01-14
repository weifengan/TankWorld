using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour {

    public float MaxSteerAngle = 30;
    private WheelCollider wheelFR;
    private WheelCollider wheelFL;
    private WheelCollider wheelRR;
    private WheelCollider wheelRL;

    private Vector3 lastPosition;
    private Vector3 lastEulerAngle;
    private Quaternion lastRotation;
    private Rigidbody rig;

 
    // Use this for initialization
    void Start () {

        lastPosition = this.transform.position;
        lastRotation = this.transform.rotation;
        lastEulerAngle = this.transform.eulerAngles;
        rig = this.gameObject.GetComponent<Rigidbody>();
        rig.centerOfMass = this.transform.Find("com").localPosition;
        wheelFL = this.transform.Find("FL").GetComponent<WheelCollider>();
        wheelFR = this.transform.Find("FR").GetComponent<WheelCollider>();
        wheelRR = this.transform.Find("RR").GetComponent<WheelCollider>();
        wheelRL = this.transform.Find("RL").GetComponent<WheelCollider>();

        SFSArray so = new SFSArray();
        so.AddFloatArray(new float[] { lastPosition.x, lastPosition.y, lastPosition.z });
        SFSUserVariable suvp = new SFSUserVariable("position", so);
        Global.SendSFSRequest(new SetUserVariablesRequest(new SFSUserVariable[] { suvp }));
    }
	
	// Update is called once per frame
	void Update () {
        float vx = Input.GetAxis("Horizontal");
        float vy = Input.GetAxis("Vertical");
        this.transform.Rotate(Vector3.up * vx * MaxSteerAngle*Time.deltaTime);

        if (Mathf.Abs(vy) != 0)
        {
            wheelFL.motorTorque = vy * 500;
            wheelFR.motorTorque = vy * 500;

            wheelFL.brakeTorque =0;
            wheelFR.brakeTorque =0;
            wheelRL.brakeTorque =0;
            wheelRR.brakeTorque =0;
        }
        else
        {
            wheelFL.motorTorque = 0;
            wheelFR.motorTorque = 0;
            wheelFL.brakeTorque = 1000;
            wheelFR.brakeTorque = 1000;
            wheelRL.brakeTorque = 1000;
            wheelRR.brakeTorque = 1000;
        }

        if (Vector3.Distance(lastPosition,this.transform.position)>=0.2f)
        {
            SFSObject so = new SFSObject();
            so.PutDouble("x", this.transform.position.x);
            so.PutDouble("y", this.transform.position.y);
            so.PutDouble("z",this.transform.position.z);
            SFSUserVariable suvp = new SFSUserVariable("position", so);
            Global.SendSFSRequest(new SetUserVariablesRequest(new SFSUserVariable[]{ suvp}));
            lastPosition = this.transform.position;
            print("更改位置");
        }

        if (Vector3.Distance(lastEulerAngle,this.transform.eulerAngles)>=0.2f && !lastRotation.Equals(this.transform.rotation))
        {
            SFSObject so = new SFSObject();
            so.PutFloat("y", this.transform.localEulerAngles.y);
            SFSUserVariable suvr = new SFSUserVariable("rotation", so);
            Global.SendSFSRequest(new SetUserVariablesRequest(new SFSUserVariable[] { suvr }));
            print("更改旋转");
            lastRotation = this.transform.rotation;
        }
    }
}
