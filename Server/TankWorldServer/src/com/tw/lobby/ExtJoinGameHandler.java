package com.tw.lobby;

import java.util.List;

import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSJoinRoomException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.tw.zone.ExtType;

public class ExtJoinGameHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User arg0, ISFSObject arg1) {
		// TODO Auto-generated method stub
        int mode=arg1.getInt("mode");

        List<Room> rooms=this.getParentExtension().getParentZone().getRoomList();
        
        Room room=this.getParentExtension().getParentZone().getRoomByName("game1");
        
        
        SFSObject outData=new SFSObject();
        outData.putInt("mode", mode);
        
        try {
			this.getApi().joinRoom(arg0,room);
			trace("用户加入游戏 ");
			outData.putBool("success", true);
		} catch (SFSJoinRoomException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
			outData.putBool("success", false);
		}
        
        this.send(ExtType.JoinGame, outData, arg0);
        
        
        
	}

}
