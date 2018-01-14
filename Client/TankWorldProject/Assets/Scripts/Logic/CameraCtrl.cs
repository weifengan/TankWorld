using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {

    public Transform target = null;

    private Transform updown = null;
    private Transform zoom = null;
    private Camera cm;

    public Vector2 ZoomRange = new Vector2(-3, 0);
    
    // Use this for initialization
    void Start() {
        updown = this.transform.Find("updown");
        zoom = this.transform.Find("updown/zoom");
        if(target!=null)
        this.transform.position = target.position;
        zoomV = zoom.localPosition.z;
    }

    // Update is called once per frame
    void Update() {

    }

    private float zoomV = 0;
    void FixedUpdate()
    {
        if (target == null) return;

        this.transform.position = Vector3.Lerp(this.transform.position, target.position, Time.deltaTime * 10);


        Quaternion vr = this.transform.rotation;
        this.transform.rotation = Quaternion.Lerp(vr, target.rotation, Time.deltaTime*10);

        //缩放控制
        zoomV += Input.GetAxis("Mouse ScrollWheel")*5;
        zoomV = Mathf.Clamp(zoomV,-3,0);
        Vector3 zv3 = zoom.localPosition;
        zv3.z = zoomV;
        zoom.localPosition = zv3;
 
    }

 
}
