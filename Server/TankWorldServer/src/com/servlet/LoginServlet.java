package com.servlet;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.entities.data.SFSObject;

import utlis.DBManager;

public class LoginServlet extends HttpServlet {
	@Override
	protected void service(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		// TODO Auto-generated method stub
		// 使用数据库根据用户名进行查询

		DBManager db = DBManager.GetInstance();
		db.Init(SmartFoxServer.getInstance().getZoneManager().getZoneByName("TankWorld").getDBManager());
		System.out.println(db);
		//设置反馈页面编码方式
		resp.setContentType("text/html;charset=utf-8");

		// 接收请求时传递的username和password
		String username = req.getParameter("username");
		username = username == null ? "" : username;

		String password = req.getParameter("password");
		password = password == null ? "" : password;

		// 创建反馈对象
		SFSObject resObj = new SFSObject();
		if (username.isEmpty() || password.isEmpty()) {
			resObj.putBool("success", false);
			resObj.putUtfString("username", username);
			resObj.putInt("id", -1);
			resObj.putUtfString("nick","");
			resObj.putInt("role", 0);
			resObj.putInt("coin",0);
			resObj.putInt("diamond",0);
			resObj.putUtfString("info", "账号或密码错误");
			resp.getWriter().write(resObj.toJson());
		} else {

			List<HashMap> accounts = DBManager.GetInstance()
					.doQuery("select * from account where acc_name='" + username + "'");

			System.out.println(accounts.size());
			// 如果小于1，则未查找到账户
			if (accounts.size() <=0) {
				resObj.putBool("success", false);
				resObj.putUtfString("username", username);
				resObj.putInt("id", -1);
				resObj.putUtfString("nick","");
				resObj.putInt("role", 0);
				resObj.putInt("coin",0);
				resObj.putInt("diamond",0);
				resObj.putUtfString("info", "此账户不存在");
				resp.getWriter().write(resObj.toJson());
				
			} else {
				HashMap acc = accounts.get(0);

				if (acc.get("acc_pwd").toString().equals(password)) {
					resObj.putBool("success", true);
					resObj.putUtfString("username", username);
					resObj.putInt("id", (int) acc.get("acc_id"));
					resObj.putUtfString("nick",acc.get("acc_nick").toString());
					resObj.putInt("role", (int)acc.get("acc_role"));
					resObj.putInt("coin",(int)acc.get("acc_coin"));
					resObj.putInt("diamond",(int)acc.get("acc_diamond"));
					resObj.putUtfString("info", "恭喜你，登录成功");
					resp.getWriter().write(resObj.toJson());
				} else {
					resObj.putBool("success", false);
					resObj.putUtfString("username", username);
					resObj.putInt("id", (int) acc.get("acc_id"));
					resObj.putUtfString("nick","");
					resObj.putInt("role", 0);
					resObj.putInt("coin",0);
					resObj.putInt("diamond",0);
					resObj.putUtfString("info", "账号或密码错误!");
					resp.getWriter().write(resObj.toJson());
				}
			}
		}
		
	}
}
