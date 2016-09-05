using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;
public class Character : MonoBehaviour
{
    /// SQLite数据库辅助类
    public static SQLiteHelper sql = new SQLiteHelper("data source=Game.db");
    public static SqliteDataReader reader;

    public Attribute attr { get; set; }

    // Use this for initialization
    void Start()
    {
        attr = new Attribute(sql, reader);
    }

    public class Attribute
    {
        public int movingRange { get; set; }
        private string name;
        private string type;
        private int level;
        private float hp;
        private float mp;
        private float ap;
        private float atk;
        private float def;
        private float magicAtk;
        private float magicDef;
        private float speed;
        // private float avo;
        private float luck;

        public Attribute(SQLiteHelper sql, SqliteDataReader reader)
        {
            movingRange = 4;
            loadInfo(sql, reader, "Alice");
            Debug.Log(toString());
        }


        private void loadInfo(SQLiteHelper sql, SqliteDataReader reader, string name)
        {
            reader = sql.ReadFullTable("Characters");
            while (reader.Read())
            {
                if (reader.GetString(reader.GetOrdinal("Name")).Equals(name))
                {
                    this.name = reader.GetString(reader.GetOrdinal("Name"));
                    this.type = reader.GetString(reader.GetOrdinal("Type"));
                    this.level = reader.GetInt32(reader.GetOrdinal("Level"));
                    this.hp = reader.GetFloat(reader.GetOrdinal("HP"));
                    this.mp = reader.GetFloat(reader.GetOrdinal("MP"));
                    this.ap = reader.GetFloat(reader.GetOrdinal("AP"));
                    this.atk = reader.GetFloat(reader.GetOrdinal("ATK"));
                    this.def = reader.GetFloat(reader.GetOrdinal("DEF"));
                    this.magicAtk = reader.GetFloat(reader.GetOrdinal("MATK"));
                    this.magicDef = reader.GetFloat(reader.GetOrdinal("MDEF"));
                    this.speed = reader.GetFloat(reader.GetOrdinal("SPD"));
                    this.luck = reader.GetFloat(reader.GetOrdinal("LUK"));
                }
            }
        }

        public string toString()
        {
            return "Name: " + name + "\nType: " + type + "\nLevel: " + level + "\nHP: " + hp + "\nMP: " + mp + "\nAP: " + ap +
                   "\nATK: " + atk + "\nDEF" + def + "\nMATK: " + magicAtk + "\nMDEF: " + magicDef + "\nSPD: " + speed + "\nLUK: " + luck;
        }



    } // End class Attribute

    // Update is called once per frame
    void Update()
    {

    }
}
