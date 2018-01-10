package utlis;

import com.smartfoxserver.v2.extensions.ExtensionLogLevel;
import com.smartfoxserver.v2.extensions.SFSExtension;

/***
 * 
 *  通用工具类
 * * 
 * @author ZRFrank
 **/


public class Tools {

	/*
	 * 获取某个对象的类型
	 * 
	 * @param o 需要获取的对象
	 * 
	 * @return 返回对象对应的字符类型
	 */
	public static String getType(Object o) {
		String type = o.getClass().getName();
		if (type == "java.lang.Integer") {
			return "int";
		} else if (type == "java.lang.String") {
			return "string";
		} else if (type == "java.lang.Boolean") {
			return "bool";
		} else if (type == "java.lang.Double") {
			return "double";
		} else if (type == "java.lang.Character") {
			return "char";
		} else if (type == "java.lang.Float") {
			return "float";
		} else if (type == "java.lang.Long") {
			return "long";
		}
		return o.getClass().getName();
	}

	public String getType(int o) {
		return "int";
	}

	public static String getType(byte o) {
		return "byte";
	}

	public static String getType(char o) {
		return "char";
	}

	public static String getType(double o) {
		return "double";
	}

	public static String getType(float o) {
		return "float";
	}

	public static String getType(long o) {
		return "long";
	}

	public static String getType(boolean o) {
		return "boolean";
	}

	public static String getType(short o) {
		return "short";
	}

	public static String getType(String o) {
		return "String";
	}
	
	private static SFSExtension ext = null;

	/**
	 * 初始化全局对象，此对象存储全局对象或管理器
	 * 
	 * @param startExt
	 *            服务器扩展
	 * @param dbmgr
	 *            SFS数据库管理器
	 */
	public static void Init(SFSExtension startExt) {
		ext = startExt;

	}

	/**
	 * 全局控制台输出函数
	 * 
	 * @param args
	 */
	public static void print(Object args) {
		if (ext != null) {
			ext.trace(args);
		}
	}
	
	
	public static void print(Object...args) {
		if (ext != null) {
			StringBuilder sb=new StringBuilder();
			for (Object o : args) {
				sb.append(o+",");
			}
			
			ext.trace(sb.toString());
			
		}
	}

	/**
	 * 全局控制台输出函数
	 * 
	 * @param level
	 * @param args
	 */
	public static void print(ExtensionLogLevel level, Object args) {
		if (ext != null) {
			ext.trace(level, args);
		}
	}
}
