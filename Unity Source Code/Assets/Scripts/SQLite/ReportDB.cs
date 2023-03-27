using DataBank;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DataBank
{
    public class ReportDB : SqliteHelper
    {
        private const String Tag = "ReportDB:\t";

        private const String TABLE_NAME = "Reports";
        private const String KEY_ID = "id";
        private const String KEY_TYPE = "type";
        private const String KEY_AUTH_LEVEL = "authorizationLevel";
        private const String KEY_NAME = "name";
        private const String KEY_DATE = "dateReported";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_TYPE, KEY_AUTH_LEVEL, KEY_NAME, KEY_DATE };

        public ReportDB() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_TYPE + " INT, " +
                KEY_AUTH_LEVEL + " TEXT, " +
                KEY_NAME + " TEXT, " +
                KEY_DATE + " DATETIME DEFAULT CURRENT_TIMESTAMP )";
            dbcmd.ExecuteNonQuery();
        }

        public void addData(ReportEntry report)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " ( "
                + KEY_ID + ", "
                + KEY_TYPE + ", "
                + KEY_AUTH_LEVEL + ", "
                + KEY_NAME + " ) "

                + "VALUES ( '"
                + report._id + "', '"
                + report._type + "', '"
                + report._authorizationLevel + "', '"
                + report._name + "' )";
            dbcmd.ExecuteNonQuery();
        }

        public override IDataReader getDataById(int id)
        {
            return base.getDataById(id);
        }

        public override IDataReader getDataByString(string str)
        {
            Debug.Log(Tag + "Getting Report: " + str);

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + str + "'";
            return dbcmd.ExecuteReader();
        }

        public override void deleteDataByString(string id)
        {
            Debug.Log(Tag + "Deleting Report: " + id);

            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "DELETE FROM " + TABLE_NAME + " WHERE " + KEY_ID + " = '" + id + "'";
            dbcmd.ExecuteNonQuery();
        }

        public override void deleteDataById(int id)
        {
            base.deleteDataById(id);
        }

        public override void deleteAllData()
        {
            Debug.Log(Tag + "Deleting Table");

            base.deleteAllData(TABLE_NAME);
        }

        public override IDataReader getAllData()
        {
            return base.getAllData(TABLE_NAME);
        }


        public IDataReader getLatestTimeStamp()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " ORDER BY " + KEY_DATE + " DESC LIMIT 1";
            return dbcmd.ExecuteReader();
        }
    }
}
