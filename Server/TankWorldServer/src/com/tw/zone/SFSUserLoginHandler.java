package com.tw.zone;

import java.util.HashMap;
import java.util.List;

import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

import utlis.DBManager;

public class SFSUserLoginHandler extends BaseServerEventHandler {

	@Override
	public void handleServerEvent(ISFSEvent arg0) throws SFSException {

		// 获取登录账号及密码
		String username = (String) arg0.getParameter(SFSEventParam.LOGIN_NAME);
		String password = (String) arg0.getParameter(SFSEventParam.LOGIN_PASSWORD);
		ISession session = (ISession) arg0.getParameter(SFSEventParam.SESSION);

		try {
			// 进行数据库查询
			List<HashMap> accounts = DBManager.GetInstance()
					.doQuery("select * from account where acc_name='" + username + "'");

			// 判断是否查询到记录
			if (accounts.size() <= 0) {
				// 未找到账号
				SFSErrorData errData = new SFSErrorData(SFSErrorCode.LOGIN_BAD_USERNAME);
				errData.addParameter(username);
				throw new SFSLoginException("无法找到些用户:" + username, errData);

			} else {
				// 取出查到的账户
				HashMap user = accounts.get(0);
				// 获取数据库中密码
				String dbPwd = user.get("acc_pwd").toString();

				// Verify the secure password
				if (!getApi().checkSecurePassword(session, dbPwd, password)) {
					SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
					data.addParameter(username);
					// Sends response if user gave incorrect password
					throw new SFSLoginException("用户名或密码错误! ", data);
				}
			}
		} catch (Exception e) {
			//数据库执行登录验证失败
			SFSErrorData errData = new SFSErrorData(SFSErrorCode.GENERIC_ERROR);
			errData.addParameter("" + e.getMessage());
			throw new SFSLoginException(e.getMessage(), errData);
		}

	}

}
