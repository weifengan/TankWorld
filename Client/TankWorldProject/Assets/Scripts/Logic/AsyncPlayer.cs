using Sfs2X.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

public class AsyncPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Global.AddEventListener(SFSEvent.USER_VARIABLES_UPDATE, OnUserVariableHandler);	
	}

    private void OnUserVariableHandler(NEvent evt)
    {
        User user = evt.BaseEvt.Params["user"] as User;
        try
        {
            if(!user.IsItMe && user.Name == this.name) {

                ///同步位移
                if (user.ContainsVariable("position"))
                {
                    SFSObject so = (SFSObject)user.GetVariable("position").Value;
                    this.transform.position = Vector3.Lerp(this.transform.position,new Vector3((float)so.GetDouble("x"), (float)so.GetDouble("y"), (float)so.GetDouble("z")),Time.deltaTime*10);
                }

                ///同步旋转
                if (user.ContainsVariable("rotation"))
                {
                    SFSObject so = (SFSObject)user.GetVariable("rotation").Value;

                    this.transform.eulerAngles = new Vector3(0, (float)so.GetDouble("y"), 0);
                }
             }
         }
        catch (Exception e)
        {
            print("出错"+e.Message);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
