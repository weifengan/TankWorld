using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class ConfigReader {

    public string ServerIp = "127.0.0.1";
    public int ServerPort = 9933;
    public string info = "";

    private static ConfigReader _reader;
    public static ConfigReader GetInstance()
    {
        if (_reader == null)
        {
            _reader = new ConfigReader();

            if (File.Exists(Application.dataPath + "/config.ini"))
            {

                string[] lines = File.ReadAllLines(Application.dataPath + "/config.ini");
                foreach (string line in lines)
                {
                    string[] data = line.Split(new char[] { '=' });
                    string pro = data[0];
                    string value = data[1];
                    if (pro == "server")
                    {
                        _reader.ServerIp = value;
                    }
                    if (pro == "port")
                    {
                        _reader.ServerPort = int.Parse(value);
                    }
                }
            }else
            {
                FileStream fs = new FileStream(Application.dataPath + "/config.ini", FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs,Encoding.UTF8);
                sw.WriteLine("server=127.0.0.1");
                sw.WriteLine("port=9933");
                _reader.ServerIp = "127.0.0.1";
                _reader.ServerPort = 9933;
                sw.Flush();
                sw.Close();
                fs.Close();
                
            }

        }
        return _reader;
    }




    

   
   
}
