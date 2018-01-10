package utlis;
import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import com.smartfoxserver.v2.db.IDBManager;

import com.smartfoxserver.v2.extensions.ExtensionLogLevel;


/*
 * 数据库连接管理操作类
 * 说明:用于连接数据库与数据库操作类
 * 
 */


public class DBManager {
	
	private static DBManager _instance=null;
	
	public static DBManager GetInstance() {
	
		if(_instance==null) {
			_instance=new DBManager();
		}
		return _instance;
	}
	
	public void Init(IDBManager mgr) {
		dbManager = mgr;
		try {
			Class.forName("com.mysql.jdbc.Driver");
			conn=DriverManager.getConnection(dbManager.getConfig().connectionString,dbManager.getConfig().userName,dbManager.getConfig().password);
			} catch (Exception e) {
			Tools.print(ExtensionLogLevel.WARN, "链接数据库失败"+dbManager.getConfig().connectionString+": " + e.toString());
		}
	}
	
	
	// 数据库管理器
	private IDBManager dbManager;
	// 数据库连接
	private Connection conn = null;

	/**
	 * 执行SQL查询语句 
	 * @param sql 查询SQL语句
	 * @return 
	 */
	public List<HashMap> doQuery(String sql) {
		try {
			Statement stmt = conn.createStatement();
			//执行SQL语句
			ResultSet rs = stmt.executeQuery(sql);
			//将查询结果转换为List
			List<HashMap> data=convertList(rs);
			rs.close();
			stmt.close();
			return data;
		} catch (Exception e) {
			Tools.print("do Query【"+sql+"】 failed: " + e.getMessage());
		}
		return null;
	}
	
	/**
	 * 执行SQL语句
	 * @param sql 需要执行的SQL语句，可以是Insert,Update，Delete
	 * 参考:ExecuteSQL("update account set acc_pwd=?","1234");
	 * @param items SQL语句中的参数
	 * @return 此操作是否执行正确
	 */
	public Boolean ExecuteSQL(String sql,Object...items)
	{
		
		 try {
			 PreparedStatement preStmt=conn.prepareStatement(sql);
			int i=1;
			for(Object item : items){  
				String type=Tools.getType(item);
				switch(type) {
				case "int":
					 preStmt.setInt(i, (int)item);
					break;
				case "string":
					preStmt.setString(i, (String)item);
					break;
				case "float":
					preStmt.setFloat(i, (Float)item);
					break;
				case "dobule":
					preStmt.setDouble(i, (Double)item);
					break;
				}
				i++; 
	        }  
			preStmt.executeUpdate();
			preStmt.close();
			return true;
		} catch (SQLException e) {
			// TODO Auto-generated catch block
		    e.printStackTrace();
			return false;
		}  
	}
	
	

	private List convertList(ResultSet rs) throws SQLException {
		List<HashMap>  list= new ArrayList();
		ResultSetMetaData md = rs.getMetaData();// 获取键名
		int columnCount = md.getColumnCount();// 获取行的数量
		while (rs.next()) {
			HashMap rowData = new HashMap();// 声明Map
			for (int i = 1; i <= columnCount; i++) {
				rowData.put(md.getColumnName(i), rs.getObject(i));// 获取键名及值
			}
			list.add(rowData);
		}
		return list;
	}
	
	
	public void CheckUserLogin(String user,String password) {
		
		//构建SQL语句
		String sql="select * from account where acc_user='"+user+"'";
		try {
			Statement stmt = conn.createStatement();
			//执行SQL语句
			ResultSet rs = stmt.executeQuery(sql);
			
		    Tools.print(rs.getRow());
		}catch(SQLException e) {
			Tools.print(e.getMessage());
		}
	}
}

