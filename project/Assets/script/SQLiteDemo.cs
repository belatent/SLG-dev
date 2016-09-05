using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;

public class SQLiteDemo : MonoBehaviour
{
    /// <summary>
    /// SQLite数据库辅助类
    /// </summary>
    private SQLiteHelper sql;

    void Start()
    {
        //打开名为Game.db的数据库
        sql = new SQLiteHelper("data source=Game.db");

        //创建名为Characters的数据表
        //sql.CreateTable("Characters", new string[] { "Name", "Type", "Level", "HP",    "MP",    "AP",    "ATK",   "DEF",   "MATK",  "MDEF",  "SPD",   "LUK" }, 
        //                            new string[] { "TEXT", "TEXT", "INTEGER", "FLOAT", "FLOAT", "FLOAT", "FLOAT", "FLOAT", "FLOAT", "FLOAT", "FLOAT", "FLOAT" });

        //插入数据
        sql.InsertValues("Characters", new string[] { "'Alice'", "'Witch'", "5", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });
        sql.InsertValues("Characters", new string[] { "'Arthur'", "'Warrior'", "6", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });
        sql.InsertValues("Characters", new string[] { "'John'", "'King'", "7", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });
        sql.InsertValues("Characters", new string[] { "'David'", "'Warrior'", "8", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });

        //sql.InsertValues("Characters", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });

        //更新数据，将Name="Alice"的记录中的Name改为"Ecila"
        //sql.UpdateValues("Characters", new string[] { "Name" }, new string[] { "'clia'" }, "Name", "=", "'Alice'");

        //插入3条数据
        //sql.InsertValues("Characters", new string[] { "'Samual'", "'Witch'", "9", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });
        //sql.InsertValues("Characters", new string[] { "'Jason'", "'Warrior'", "10", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });
        //sql.InsertValues("Characters", new string[] { "'Peter'", "'King'", "11", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0", "200.0" });

        //删除Name="王五"且Age=26的记录,DeleteValuesOR方法类似
        //sql.DeleteValuesAND("Characters", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'王五'", "'26'" });

        //读取整张表
        SqliteDataReader reader = sql.ReadFullTable("Characters");
        while (reader.Read())
        {
            //读取ID
            Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
            //读取Name
            Debug.Log(reader.GetString(reader.GetOrdinal("Type")));
            //读取Age
            Debug.Log(reader.GetFloat(reader.GetOrdinal("HP")));
            //读取Email
            Debug.Log(reader.GetFloat(reader.GetOrdinal("MP")));
        }

        //读取数据表中Age>=25的所有记录的ID和Name
        //reader = sql.ReadTable("Characters", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
        //while (reader.Read())
        //{
            //读取ID
            //Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
            //读取Name
            //Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
        //}

        //自定义SQL,删除数据表中所有Name="王五"的记录
        //sql.ExecuteQuery("DELETE FROM Characters WHERE NAME='王五'");

        //关闭数据库连接
        sql.CloseConnection();
    }
}