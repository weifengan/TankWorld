package com.tw.zone;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.extensions.SFSExtension;

import utlis.DBManager;
import utlis.Tools;

public class ZoneExtension extends SFSExtension {

	public static DBManager db;
	public void init() {
	
		
		Tools.Init(this);
		//初始化数据库连接
		//初始化数据库
		db=DBManager.GetInstance();
		db.Init(this.getParentZone().getDBManager());
		
		this.addEventHandler(SFSEventType.USER_JOIN_ZONE, SFSUserJoinZoneHandler.class);
		
		//添加用户登录扩展请求
	    this.addRequestHandler("dologin", ZoneUserLoginHandler.class);
	    this.addRequestHandler("doreg", ZoneUserRegisterHandler.class);
	    
	    //重呢称检测
	    this.addRequestHandler("setRole", ZoneSetRoleNickHandler.class);
	    
		trace("坦克服务器启动成功。。。。。");
       
		
	}


}
