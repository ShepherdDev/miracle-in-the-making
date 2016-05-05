using Arena.DataLib;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
	public class SalvationData : SqlData
	{
		public SalvationData()
		{
		}

		public void DeleteSalvation(int salvationID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SalvationID", (object)salvationID));
			base.ExecuteNonQuery("cust_sothc_mitm_sp_del_salvation", arrayLists);
		}

		public DataTable GetSalvation_DT(int personID)
		{
			SqlConnection dbConnection = (new SqlDbConnection()).GetDbConnection();
			SqlCommand sqlCommand = new SqlCommand();
			StringBuilder stringBuilder = new StringBuilder();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			DataSet dataSet = new DataSet("Salvation");
			stringBuilder.Append("SELECT * FROM cust_sothc_mitm_salvation");
			stringBuilder.Append(" WHERE (1 = 1)");
			if (personID != -1)
			{
				stringBuilder.Append(" AND person_id = @PersonID");
			}
			sqlCommand.CommandTimeout = 360;
			sqlCommand.Connection = dbConnection;
			sqlCommand.CommandType = CommandType.Text;
			sqlCommand.CommandText = stringBuilder.ToString();
			sqlCommand.Parameters.Add(new SqlParameter("@PersonID", (object)personID));
			dbConnection.Open();
			sqlDataAdapter.Fill(dataSet);
			dbConnection.Close();
			return dataSet.Tables[0];
		}

		public SqlDataReader GetSalvationByID(int salvationID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SalvationID", (object)salvationID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_salvation_byID", arrayLists);
		}

		public SqlDataReader GetSalvationByPersonID(int personID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@PersonID", (object)personID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_salvation_byPersonID", arrayLists);
		}

		public int SaveSalvation(int salvationID, int personID, string firstName, string lastName, bool status, string userID)
		{
			ArrayList arrayLists = new ArrayList();
			SqlParameter sqlParameter = new SqlParameter();
			arrayLists.Add(new SqlParameter("@SalvationID", (object)salvationID));
			arrayLists.Add(new SqlParameter("@PersonID", (object)personID));
			arrayLists.Add(new SqlParameter("@FirstName", firstName));
			arrayLists.Add(new SqlParameter("@LastName", lastName));
			arrayLists.Add(new SqlParameter("@Status", (object)status));
			arrayLists.Add(new SqlParameter("@UserID", userID));
			sqlParameter.ParameterName = "@ID";
			sqlParameter.Direction = ParameterDirection.Output;
			sqlParameter.SqlDbType = SqlDbType.Int;
			arrayLists.Add(sqlParameter);
			base.ExecuteNonQuery("cust_sothc_mitm_sp_save_salvation", arrayLists);
			return (int)sqlParameter.Value;
		}
	}
}