package com.tw.zone;

import java.util.ArrayList;
import java.util.List;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ExtFetchBoardListHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		
		trace("获取用户列表 ");
		// TODO Auto-generated method stub
		int type = arg1.getInt("type");
		
		
		SFSObject outData=new SFSObject();
		outData.putInt("type", type);
		
		switch (type) {
		case 1: //竞技
			SFSArray arr=new SFSArray();
			
			for (int i = 1; i < 10; i++) {
				  SFSObject obj=new SFSObject();
				  obj.putInt("id", i);
				  obj.putUtfString("title", "用户"+i);
				  obj.putInt("score", 100);
				  arr.addSFSObject(obj);
			}
			
		
			outData.putInt("count",10);
			outData.putSFSArray("list", arr);
			
			this.send(ExtType.FetchBoardList,outData, arg0);
			
			break;
		case 2://胜局
			break;
		}

	}
	
	
}
