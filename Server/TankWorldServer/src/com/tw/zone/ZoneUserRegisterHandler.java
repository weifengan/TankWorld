package com.tw.zone;

import java.util.HashMap;
import java.util.List;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import utlis.DBManager;

public class ZoneUserRegisterHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
		
				//获取用户名和密码
				String name=arg1.getUtfString("username");
				String pwd=arg1.getUtfString("password");
				
				SFSObject resData=new SFSObject();
				//使用数据库根据用户名进行查询
				List<HashMap> accounts=DBManager.GetInstance().doQuery("select * from account where acc_name='"+name+"'");
						
						//判断是否查询到记录
				if(accounts.size()>=1) {
					resData.putBool("success", false);
					resData.putUtfString("username", name);
					resData.putUtfString("password",pwd);
					resData.putUtfString("info", "此账号已经存在!");
				}else {
					
					if(DBManager.GetInstance().ExecuteSQL("insert into account(acc_name,acc_pwd) values(?,?)",name,pwd)) {
						resData.putBool("success", true);
						resData.putUtfString("username", name);
						resData.putUtfString("password",pwd);
						resData.putUtfString("info", "用户注册成功!");
						
					}else {
						resData.putBool("success", false);
						resData.putUtfString("username", name);
						resData.putUtfString("password",pwd);
						resData.putUtfString("info", "用户注册失败!");
					}
					
				}
				
				this.send(ExtType.UserReg,resData, arg0);
	}

	

}
