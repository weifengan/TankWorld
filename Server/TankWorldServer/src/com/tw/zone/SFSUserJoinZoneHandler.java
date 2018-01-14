package com.tw.zone;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import utlis.DBManager;

public class SFSUserJoinZoneHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {
		// TODO Auto-generated method stub
		User sfsuser = (User) arg0.getParameter(SFSEventParam.USER);

		List<HashMap> accounts = DBManager.GetInstance()
				.doQuery("select * from account where acc_name='" + sfsuser.getName() + "'");
		// 取出查到的账户
		HashMap user = accounts.get(0);
		SFSObject resData = new SFSObject();
		resData.putBool("success", true);
		resData.putUtfString("username", sfsuser.getName());
		resData.putUtfString("nick", user.get("acc_nick").toString());
		resData.putInt("role", (int) user.get("acc_role"));
		resData.putInt("coin", (int) user.get("acc_coin"));
		resData.putInt("diamond", (int) user.get("acc_diamond"));
		resData.putUtfString("info", "登陆成功!");
		Room roleRoom = this.getParentExtension().getParentZone().getRoomByName("roleroom");
		Room lobby = this.getParentExtension().getParentZone().getRoomByName("lobby");
		try {
			if (resData.getUtfString("nick").length() == 0) {
				trace("用户" + arg0 + "登录RoleRoom成功!");

				// roleRoom.addUser(arg0);
				this.getApi().joinRoom(sfsuser, roleRoom);
			} else {

				// 在自身存储角色和呢称
				SFSUserVariable surole = new SFSUserVariable("role", resData.getInt("role"));
				SFSUserVariable sunick = new SFSUserVariable("nick", resData.getUtfString("nick"));
				List<UserVariable> uvs = new ArrayList();
				uvs.add(surole);
				uvs.add(sunick);
				sfsuser.setVariables(uvs);

				trace("用户" + arg0 + "登录Lobby成功!");
				// lobby.addUser(arg0);
				this.getApi().joinRoom(sfsuser, lobby);
			}

		} catch (Exception e) {
			trace("用户进入 房间失败");
		}
		this.send(ExtType.UserLogin, resData, sfsuser);
	}

}
