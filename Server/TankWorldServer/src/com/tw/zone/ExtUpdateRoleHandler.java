package com.tw.zone;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

import utlis.DBManager;

public class ExtUpdateRoleHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
        
		
		String nick=arg1.getUtfString("nick");
		int role=arg1.getInt("role");
		
		List<HashMap> users=DBManager.GetInstance().doQuery("select * from account where acc_nick='"+nick+"'");
		ISFSObject outData=new SFSObject();
		try {
			if(users.size()<=0) {
				if(DBManager.GetInstance().ExecuteSQL("update account set acc_nick=?,acc_role=? where acc_name=?", nick,role,arg0.getName())) {
					outData.putBool("success", true);
					outData.putUtfString("info", "创建角色昵称成功!");
					Room lobby=this.getParentExtension().getParentZone().getRoomByName("lobby");
					
					this.getApi().joinRoom(arg0, lobby);
					trace("成功返回lobby");
					
					//在用户身上存储角色id
					SFSUserVariable surole=new SFSUserVariable("role",role);
					SFSUserVariable sunick=new SFSUserVariable("nick",nick);
					List<UserVariable> uvs= new ArrayList();
					uvs.add(surole);
					uvs.add(sunick);
					arg0.setVariables(uvs);
					
				}else {
					outData.putBool("success", false);
					outData.putUtfString("info", "创建角色昵称失败!");
					
				}
			}else {
				outData.putBool("success", false);
				outData.putUtfString("info", "昵称 "+nick+"已经被使用!");
				//this.send("setrole", outData, arg0);
			}
		}catch(Exception e) {
			outData.putBool("success", false);
			outData.putUtfString("info", "创建角色昵称失败!");
		}
		
		this.send(ExtType.UpdateRole,outData, arg0);
	}

}
