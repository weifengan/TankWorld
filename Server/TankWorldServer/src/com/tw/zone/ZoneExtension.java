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
		
		//添加用户登录扩展请求
	    this.addRequestHandler(ExtType.UserLogin, ZoneUserLoginHandler.class);
	    this.addRequestHandler(ExtType.UserReg, ZoneUserRegisterHandler.class);
	    
	    //重呢称检测
	    this.addRequestHandler(ExtType.UpdateRole, ZoneUpdateRoleHandler.class);
	    this.addRequestHandler(ExtType.FetchBoardList, ExtFetchBoardListHandler.class);
	    
		trace("坦克服务器启动成功。。。。。");
       
		
	}


}
