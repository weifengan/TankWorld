package com.tw.lobby;

import com.smartfoxserver.v2.extensions.ExtensionLogLevel;
import com.smartfoxserver.v2.extensions.SFSExtension;
import com.tw.zone.ExtType;

import utlis.DBManager;
import utlis.Tools;

public class LobbyExtension extends SFSExtension {
	
	public static DBManager db;
	@Override
	public void init() {
		// TODO Auto-generated method stub
		Tools.Init(this);
		//初始化数据库连接
		//初始化数据库
		db=DBManager.GetInstance();
		db.Init(this.getParentZone().getDBManager());
		this.addRequestHandler(ExtType.JoinGame, ExtJoinGameHandler.class);
		trace("Lobby 扩展初始化成功！");
		 
	}

}
