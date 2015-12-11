
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using System.IO; 

namespace PruebaAndroid
{
	/*[BroadcastReceiver]
	public class DataBase : BroadcastReceiver
	{*/
	class Database
	{
		//SQLiteDatabase object for database handling
		private SQLiteDatabase sqldb;
		// Cadena para el manejo de consultas 
		private string sqldb_query;
		// Cadena para la manipulación del mensaje 
		private string sqldb_message;
		// Booleano para comprobar disponibilidad de base de datos 
		private bool sqldb_available;
		// Cero argumento del constructor, inicia una nueva instancia de la clase de base de datos 
		public Database()
		{
			sqldb_message = "";
			sqldb_available = false;
		}
		// Un argumento constructor, inicia una nueva instancia de la clase de base de datos con el parámetro nombre de la base 
		public Database(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				sqldb_available = false;
				CreateDatabase(sqldb_name);
			}
			catch (SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		// Obtiene o establece el valor de base de datos dependiendo de la disponibilidad 
		public bool DatabaseAvailable
		{
			get{ return sqldb_available; }
			set{ sqldb_available = value; }
		}
		// Obtiene o establece el valor para el manejo de mensajes 
		public string Message
		{
			get{ return sqldb_message; }
			set{ sqldb_message = value; }
		}
		// Crea una nueva base de datos que el nombre viene dado por el parámetro 
		public void CreateDatabase(string sqldb_name)
		{
			try
			{
				sqldb_message = "";
				string sqldb_location = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				string sqldb_path = Path.Combine(sqldb_location, sqldb_name);
				bool sqldb_exists = File.Exists(sqldb_path);
				if(!sqldb_exists)
				{
					sqldb = SQLiteDatabase.OpenOrCreateDatabase(sqldb_path,null);
					sqldb_query = "CREATE TABLE IF NOT EXISTS MyTable (_id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR, LastName VARCHAR, Age INT);";
					sqldb.ExecSQL(sqldb_query);
					sqldb_message = "Database: " + sqldb_name + " created";
				}
				else
				{
					sqldb = SQLiteDatabase.OpenDatabase(sqldb_path, null, DatabaseOpenFlags.OpenReadwrite);
					sqldb_message = "Database: " + sqldb_name + " opened";
				}
				sqldb_available=true;
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		// Añade un nuevo registro con los parámetros dados 
		public void AddRecord(string sName, string sLastName, int iAge)
		{
			try
			{
				sqldb_query = "INSERT INTO MyTable (Name, LastName, Age) VALUES ('" + sName + "','" + sLastName + "'," + iAge + ");";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record saved";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		// Actualiza un registro existente con los parámetros dados en función de parámetro id 
		public void UpdateRecord(int iId, string sName, string sLastName, int iAge)
		{
			try
			{
				sqldb_query="UPDATE MyTable SET Name ='" + sName + "', LastName ='" + sLastName + "', Age ='" + iAge + "' WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " updated";
			}
			catch(SQLiteException ex)
			{
				sqldb_message = ex.Message;
			}
		}
		// Elimina el registro asociado al parámetro id 
		public void DeleteRecord(int iId)
		{
			try
			{
				sqldb_query = "DELETE FROM MyTable WHERE _id ='" + iId + "';";
				sqldb.ExecSQL(sqldb_query);
				sqldb_message = "Record " + iId + " deleted";
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
		}
		// Busca en un registro y devuelve un cursor Android.Database.ICursor 
		// Muestra todos los registros de la tabla 
		public Android.Database.ICursor GetRecordCursor()
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT*FROM MyTable;";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}
		// Busca en un registro y devuelve un cursor Android.Database.ICursor 
		// Muestra los registros de acuerdo con los criterios de búsqueda 
		public Android.Database.ICursor GetRecordCursor(string sColumn, string sValue)
		{
			Android.Database.ICursor sqldb_cursor = null;
			try
			{
				sqldb_query = "SELECT*FROM MyTable WHERE " + sColumn + " LIKE '" + sValue + "%';";
				sqldb_cursor = sqldb.RawQuery(sqldb_query, null);
				if(!(sqldb_cursor != null))
				{
					sqldb_message = "Record not found";
				}
			}
			catch(SQLiteException ex) 
			{
				sqldb_message = ex.Message;
			}
			return sqldb_cursor;
		}
	}
}

