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

		User user = (User) arg0.getParameter(SFSEventParam.USER);
		trace("user join"+user);
		List<HashMap> accounts = DBManager.GetInstance()
				.doQuery("select * from account where acc_name='" + user.getName() + "'");
		SFSObject resData = new SFSObject();
		//如果查询到信息
		if (accounts.size() >= 1) {
			// 取出查到的账户
			HashMap acc = accounts.get(0);
    		resData.putBool("success", true);
			resData.putUtfString("username", user.getName());
		    resData.putUtfString("nick", acc.get("acc_nick").toString());
			resData.putInt("role", (int) acc.get("acc_role"));
			resData.putInt("coin", (int) acc.get("acc_coin"));
			resData.putInt("diamond", (int) acc.get("acc_diamond"));

			
			//检测当前用户是否已经创建过角色 ，如果没有创建角色，进入角色房间创建角色，否则 直接 进入Lobby
			Room roleRoom = this.getParentExtension().getParentZone().getRoomByName("roleroom");
			Room lobby = this.getParentExtension().getParentZone().getRoomByName("lobby");
			
			try {
				if (resData.getUtfString("nick").length() == 0) {
					trace("用户" + arg0 + "登录RoleRoom成功!");
					this.getApi().joinRoom(user, roleRoom);
				} else {
     				// 在自身存储角色和呢称
					SFSUserVariable surole = new SFSUserVariable("role", resData.getInt("role"));
					SFSUserVariable sunick = new SFSUserVariable("nick", resData.getUtfString("nick"));
					List<UserVariable> uvs = new ArrayList();
					uvs.add(surole);
					uvs.add(sunick);
					user.setVariables(uvs);
				    trace("用户" + user + "登录Lobby成功!");
					this.getApi().joinRoom(user, lobby);
				}
			} catch (Exception e) {
				trace("用户进入 房间失败");
			}
			
		    this.send(ExtType.UserLoginResult, resData, user);
		}

	}

}
