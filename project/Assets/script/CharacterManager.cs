using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class CharacterManager : MonoBehaviour {


	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    /*
    private void accessCharacters() {
        using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
            dbConnection.Open();
            using (IDbCommand dbCommand = dbConnection.CreateCommand()) {
                string sqlQuery = "SELECT * FROM characters";
                dbCommand.CommandText = sqlQuery;

                using (IDataReader reader = dbCommand.ExecuteReader()) {
                    while (reader.Read()) {
                        Debug.Log(reader.GetString(1) + " - " + reader.GetString(2));
                    }
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }






    }*/
}
