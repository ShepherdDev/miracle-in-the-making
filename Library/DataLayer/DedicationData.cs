using Arena.DataLib;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
	public class DedicationData : SqlData
	{
		public DedicationData()
		{
		}

		public void DeleteDedication(int dedicationID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@DedicationID", (object)dedicationID));
			base.ExecuteSqlDataReader("cust_sothc_mitm_sp_del_dedication", arrayLists);
		}

		public DataTable GetDedication_DT(int approvalStatus)
		{
			SqlConnection dbConnection = (new SqlDbConnection()).GetDbConnection();
			SqlCommand sqlCommand = new SqlCommand();
			StringBuilder stringBuilder = new StringBuilder();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			DataSet dataSet = new DataSet("Dedication");
			stringBuilder.Append("SELECT * FROM cust_sothc_mitm_v_dedicationList");
			stringBuilder.Append(" WHERE 1 = 1");
			if (approvalStatus != -1)
			{
				stringBuilder.Append(" AND is_approved = @ApprovalStatus");
			}
			sqlCommand.CommandTimeout = 360;
			sqlCommand.Connection = dbConnection;
			sqlCommand.CommandType = CommandType.Text;
			sqlCommand.CommandText = stringBuilder.ToString();
			sqlCommand.Parameters.Add(new SqlParameter("@ApprovalStatus", (object)approvalStatus));
			dbConnection.Open();
			sqlDataAdapter.Fill(dataSet);
			dbConnection.Close();
			return dataSet.Tables[0];
		}

		public SqlDataReader GetDedicationByID(int dedicationID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@DedicationID", (object)dedicationID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_byID", arrayLists);
		}

		public SqlDataReader GetDedicationBySeatPledgeID(int seatPledgeID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)seatPledgeID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_bySeatPledgeID", arrayLists);
		}

		public SqlDataReader GetDedicationByUnapproved()
		{
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_byUnapproved", new ArrayList());
		}

		public int SaveDedication(int dedicationID, string approvedBy, int seatPledgeID, string dedicatedTo, string sponsoredBy, string biography, bool anonymous, int? blobID, string userID)
		{
			object value;
			ArrayList arrayLists = new ArrayList();
			SqlParameter sqlParameter = new SqlParameter();
			arrayLists.Add(new SqlParameter("@DedicationID", (object)dedicationID));
			arrayLists.Add(new SqlParameter("@ApprovedBy", approvedBy));
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)seatPledgeID));
			arrayLists.Add(new SqlParameter("@DedicatedTo", dedicatedTo));
			arrayLists.Add(new SqlParameter("@SponsoredBy", sponsoredBy));
			arrayLists.Add(new SqlParameter("@Biography", biography));
			arrayLists.Add(new SqlParameter("@Anonymous", (object)anonymous));
			ArrayList arrayLists1 = arrayLists;
			if (blobID.HasValue)
			{
				value = blobID.Value;
			}
			else
			{
				value = DBNull.Value;
			}
			arrayLists1.Add(new SqlParameter("@BlobID", value));
			arrayLists.Add(new SqlParameter("@UserID", userID));
			sqlParameter.ParameterName = "@ID";
			sqlParameter.Direction = ParameterDirection.Output;
			sqlParameter.SqlDbType = SqlDbType.Int;
			arrayLists.Add(sqlParameter);
			base.ExecuteNonQuery("cust_sothc_mitm_sp_save_dedication", arrayLists);
			return (int)sqlParameter.Value;
		}
	}
}