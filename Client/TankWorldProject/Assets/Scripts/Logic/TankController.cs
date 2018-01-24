using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TankController : MonoBehaviour {

    public enum TurrentDir
    {
        None=0,Left=-1,Right=1
    }
    private TurrentDir turrentDir = TurrentDir.None;
    public float RotateSpeed = 30;
    public float MoveSpeed = 30;
    public float GravityForce = -2;

    private CharacterController m_CharacterController = null;
    private Transform m_tank = null;
    private Transform m_turrent = null;
    private Transform mF;
    private Transform mB;
    private Transform mL;
    private Transform mR;

    /// <summary>
    /// 前后倾斜角度自适应
    /// </summary>
    private float AngleFB;
    /// <summary>
    /// 左右倾斜角度自适应
    /// </summary>
    private float AngleLR;
 


    // Use this for initialization
    void Start () {
        m_tank = this.transform.Find("model");
        m_turrent = this.transform.Find("model/turrent");
        m_CharacterController = this.GetComponent<CharacterController>();
        mF = m_tank.Find("autojust/F");
        mB = m_tank.Find("autojust/B");
        mR = m_tank.Find("autojust/R");
        mL = m_tank.Find("autojust/L");
    }

	void Update () {
        //自动调整Tank的倾斜角度
        AutoAdjustTankAngle();
        MoveMent();
    }

    public bool isGrounded = false;

    private float vy = 0;
    private void MoveMent()
    {
        float vx = Input.GetAxis("Horizontal");
        float vz = Input.GetAxis("Vertical");

        isGrounded = m_CharacterController.isGrounded;

        if (!m_CharacterController.isGrounded)
        {
            vy -= GravityForce;
        }
        m_CharacterController.Move(m_tank.forward * vz * MoveSpeed * Time.deltaTime+new Vector3(0,-vy,0)*Time.deltaTime);        
        m_tank.Rotate(Vector3.up * RotateSpeed * vx);

        if (Input.GetKey(KeyCode.X))
        {
            turrentDir = TurrentDir.Left;
        }else if (Input.GetKey(KeyCode.C))
        {
            turrentDir = TurrentDir.Right;
        }else
        {
            turrentDir = TurrentDir.None;
        }



        m_turrent.Rotate(Vector3.up * (int)turrentDir * 3);


    }

    /// <summary>
    /// 坦克自适应角度
    /// </summary>
    private void AutoAdjustTankAngle()
    {
        Ray ray = new Ray(mF.position, -Vector3.up);
        RaycastHit hitInfo;
        Vector3 fp = new Vector3();
        if(Physics.Raycast(ray,out hitInfo))
        {
            Debug.DrawLine(mF.position, hitInfo.point,Color.red);
            fp = hitInfo.point;
        }

        Vector3 bp = new Vector3();
        ray = new Ray(mB.position, -Vector3.up);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(mB.position, hitInfo.point, Color.red);
            bp = hitInfo.point;
        }
        if (Mathf.Abs(bp.z - fp.z) < 0.1f)
        {
            AngleFB = 0;
        }
        else if (Mathf.Abs(bp.z - fp.z) > 3)
        {
            AngleFB = 0;
        }
        else
        {
            AngleFB = -Mathf.Asin((bp.y - fp.y) / Vector3.Distance(fp, bp)) * Mathf.Rad2Deg;
        }


        Vector3 lp = new Vector3();
        ray = new Ray(mL.position, -Vector3.up);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(mL.position, hitInfo.point, Color.yellow);
            lp = hitInfo.point;
        }

        Vector3 rp = new Vector3();
        ray = new Ray(mR.position, -Vector3.up);
        if (Physics.Raycast(ray, out hitInfo))
        {
            Debug.DrawLine(mR.position, hitInfo.point, Color.yellow);
            rp = hitInfo.point;
        }

        if (Mathf.Abs(lp.y - rp.y) < 0.1f)
        {
            AngleLR = 0;
        }
        else
        {
            AngleLR = -Mathf.Asin((lp.y - rp.y)/ Vector3.Distance(lp, rp)) * Mathf.Rad2Deg;
        }
        m_tank.localRotation =Quaternion.Lerp(m_tank.localRotation,Quaternion.Euler(new Vector3(AngleFB, m_tank.localRotation.eulerAngles.y, AngleLR)),Time.deltaTime*5);
    }
 
}
