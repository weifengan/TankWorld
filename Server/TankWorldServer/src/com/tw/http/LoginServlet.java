package com.tw.http;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.smartfoxserver.v2.SmartFoxServer;
import com.smartfoxserver.v2.entities.Zone;
import com.smartfoxserver.v2.entities.data.SFSObject;

import utlis.DBManager;

public class LoginServlet extends HttpServlet {

	/// HttpServletRequest 请求
	// HttpServletResponse 回复
	@Override
	protected void service(HttpServletRequest arg0, HttpServletResponse arg1) throws ServletException, IOException {
		// TODO Auto-generated method stub
		//控制反馈给客户时，文档的编码方式
		arg1.setContentType("text/html;charset=utf-8");
		
		///1. 获取请求信息
		//获取客户端请求时，传递过来的username
		String username=arg0.getParameter("username");
		
		username=username==null ? "":username;
		
		//获取客户端请求时，传递过来的password
		String password=arg0.getParameter("password");
		password=password==null ? "":password;
		
		//创建一个SFSObject
		SFSObject outData=new SFSObject();
		if(username.equals("") || password.equals("")) {
			outData.putBool("success", false);
			outData.putUtfString("username", "");
			outData.putUtfString("info", "请正确输入账号和密码！");
			arg1.getWriter().write(outData.toJson());
			return;
		}
		
		
		//2.根据信息进行DB查询
		DBManager db=DBManager.GetInstance();
		
		//获取区， 然后获取区的dbManager对象
		Zone zone=SmartFoxServer.getInstance().getZoneManager().getZoneByName("TankWorld");
		
		//初始化数据库连接对象
		db.Init(zone.getDBManager());
		
		
		List<HashMap> users=db.doQuery("select * from account where acc_name='"+username+"'");
		
		if(users.size()<=0) {
		  //没有找到这个人
			outData.putBool("success", false);
			outData.putUtfString("username", username);
			outData.putUtfString("info", "此账号不存在 !");
			
		}else {
			//获取查找到的记录
			HashMap user=users.get(0);
			
			if(user.get("acc_pwd").equals(password)) {
				//登录成功
				outData.putBool("success", true);
				outData.putUtfString("username", username);
				outData.putUtfString("info", "登录成功 !");
				
			}else {
				//登录失败
				outData.putBool("success", false);
				outData.putUtfString("username", username);
				outData.putUtfString("info", "密码错误!");
			}
		}
		
		//3.向客户端回复内结果
		//向请求用户回复信息
		arg1.getWriter().write(outData.toJson());
		
	}

}
