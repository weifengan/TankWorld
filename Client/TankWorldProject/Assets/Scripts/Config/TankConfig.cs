using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class TankConfig{


    private  List<TankData> _datas;
    public List<TankData> Data
    {
        get { return _datas; }
    }
    public void Init()
    {
        _datas = new List<TankData>();
        TextAsset ta = Resources.Load<TextAsset>("Config/TankConfig");

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(ta.text);

        XmlNodeList tanks = xml.SelectNodes("tanks/tank");

        foreach (XmlElement item in tanks)
        {
            TankData td = new TankData();
            td.title = item.GetAttribute("title");
            td.id = int.Parse(item.GetAttribute("id"));
            td.blood = int.Parse(item.GetAttribute("blood"));
            td.attack = int.Parse(item.GetAttribute("attack"));
            td.speed = float.Parse(item.GetAttribute("speed"));
            td.desc = item.SelectSingleNode("desc").InnerText;
            _datas.Add(td);
        }
    }
}

public struct TankData
{
    public int id;
    public string title;
    public int blood;
    public int attack;
    public float speed;

    public string desc;

}





