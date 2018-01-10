package com.tw.zone;

import java.util.HashMap;
import java.util.List;

import com.smartfoxserver.bitswarm.sessions.Session;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import utlis.DBManager;

public class SFSLoginHandler extends BaseServerEventHandler {

	public void handleServerEvent(ISFSEvent event) throws SFSException {
		//获取客户端用来登录的账户名
		String name=(String)event.getParameter(SFSEventParam.LOGIN_NAME);
		
		//获取客户端用来登录的密码
		String pwd=(String)event.getParameter(SFSEventParam.LOGIN_PASSWORD);
		
		//使用数据库根据用户名进行查询
		List<HashMap> accounts=DBManager.GetInstance().doQuery("select * from account where acc_name='"+name+"'");
		
		//判断是否查询到记录
		if(accounts.size()<1) {
			SFSErrorData data=new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
			data.addParameter(name);
			throw new SFSLoginException("账户{"+name+"} 不存在", data) ;
		}else {
			//取出查到的账户
			HashMap user=accounts.get(0);
			//获取session
			Session  session= (Session)event.getParameter(SFSEventParam.SESSION);
			//校验密码(检验密码时，必须使用SFS的checkSecurePassword
			if(!this.getApi().checkSecurePassword(session, user.get("acc_pwd").toString(),pwd)) {
				SFSErrorData data=new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
				data.addParameter(user.get("acc_pwd").toString());
				throw new SFSLoginException("密码不正确", data);
			}else {
				trace("用户"+name+"登录成功!");			    	
			}
		}
	}

}
