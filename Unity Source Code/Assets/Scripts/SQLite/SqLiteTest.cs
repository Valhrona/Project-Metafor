using UnityEngine;
using System;
using DataBank;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class SqLiteTest : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {
        // Obtain the enemyprefab resource
        GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        // Debug.Log(enemyPrefab);

        // Create database
        ReportDB mReportDB = new ReportDB();

        // Add data (can also load in csv file through Streamreader)

        //mReportDB.addData(new ReportEntry("0", "retrieval", "3", "John Doe"));
        //mReportDB.addData(new ReportEntry("1", "deletion", "2", "Steve Will"));
        //mReportDB.addData(new ReportEntry("2", "manipulation", "2", "Peter Beer"));
        //mReportDB.addData(new ReportEntry("3", "publication", "1", "Peter Beer"));
        //mReportDB.addData(new ReportEntry("4", "publication", "1", "John Doe"));
        //mReportDB.addData(new ReportEntry("5", "manipulation", "3", "John Doe"));
        //mReportDB.addData(new ReportEntry("6", "deletion", "2", "Steve Will"));
        //mReportDB.close();

        //Fetch All Data
        ReportDB mReportDB2 = new ReportDB();
        System.Data.IDataReader reader = mReportDB2.getAllData();

        int fieldCount = reader.FieldCount;
        List<ReportEntry> myList = new List<ReportEntry>();
        while (reader.Read())
        {
            ReportEntry entity = new ReportEntry(reader[0].ToString(),
                                    reader[1].ToString(),
                                    reader[2].ToString(),
                                    reader[3].ToString(),
                                    reader[4].ToString());

            //Debug.Log("id: " + entity._id);
            myList.Add(entity);
        }

        // Generate enemy prefabs based of the number of entries in the database
        for (int i = 0; i < myList.Count; i++)
        {
            int size = Convert.ToInt32(myList[i]._authorizationLevel);
            //Generate object
            int x = Random.Range(-10, 10);
            int z = Random.Range(-10, 10);

            GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(x, 2, z), enemyPrefab.transform.rotation);
            //Adjust scale
            newEnemy.transform.localScale = new Vector3(newEnemy.transform.localScale.x, size, newEnemy.transform.localScale.z);
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
