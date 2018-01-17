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

public class RegisterServlet extends HttpServlet {

	@Override
	protected void service(HttpServletRequest req, HttpServletResponse resp) throws ServletException, IOException {
		// TODO Auto-generated method stub
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
		
		SFSObject resData=new SFSObject();
		if (username.isEmpty() || password.isEmpty()) {
			resData.putBool("success", false);
			resData.putUtfString("username", username);
			resData.putUtfString("password",password);
			resData.putUtfString("info", "请正确填写账号和密码!");
			resp.getWriter().write(resData.toJson());
			return;
		}
		
	
		//使用数据库根据用户名进行查询
		List<HashMap> accounts=DBManager.GetInstance().doQuery("select * from account where acc_name='"+username+"'");
				
				//判断是否查询到记录
		if(accounts.size()>=1) {
			resData.putBool("success", false);
			resData.putUtfString("username", username);
			resData.putUtfString("password",password);
			resData.putUtfString("info", "此账号已经存在!");
		}else {
			
			if(DBManager.GetInstance().ExecuteSQL("insert into account(acc_name,acc_pwd) values(?,?)",username,password)) {
				resData.putBool("success", true);
				resData.putUtfString("username", username);
				resData.putUtfString("password",password);
				resData.putUtfString("info", "用户注册成功!");
				
			}else {
				resData.putBool("success", false);
				resData.putUtfString("username", username);
				resData.putUtfString("password",password);
				resData.putUtfString("info", "用户注册失败!");
			}
		}
		
		resp.getWriter().write(resData.toJson());
	}
	
	
   
}
