using Arena.Custom.SOTHC.MiTM;
using Arena.DataLib;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
	public class SeatPledgeData : SqlData
	{
		public SeatPledgeData()
		{
		}

		public void DeleteSeatPledge(int seatPledgeID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)seatPledgeID));
			base.ExecuteNonQuery("cust_sothc_mitm_sp_del_seatPledge", arrayLists);
		}

		public DataTable GetSeatPledge_DT(string section, int seatNumber, bool unassignedSeat)
		{
			SqlConnection dbConnection = (new SqlDbConnection()).GetDbConnection();
			SqlCommand sqlCommand = new SqlCommand();
			StringBuilder stringBuilder = new StringBuilder();
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			DataSet dataSet = new DataSet("SeatPledge");
			stringBuilder.Append("SELECT * FROM cust_sothc_mitm_v_seatPledgeList");
			stringBuilder.Append(" WHERE (1 = 1)");
			if (!string.IsNullOrEmpty(section))
			{
				stringBuilder.Append(" AND assigned_section = @Section");
			}
			if (seatNumber != -1)
			{
				stringBuilder.Append(" AND assigned_seat_number = @SeatNumber");
			}
			if (unassignedSeat)
			{
				stringBuilder.Append(" AND assigned_seat_id IS NULL");
			}
			sqlCommand.CommandTimeout = 360;
			sqlCommand.Connection = dbConnection;
			sqlCommand.CommandType = CommandType.Text;
			sqlCommand.CommandText = stringBuilder.ToString();
			sqlCommand.Parameters.Add(new SqlParameter("@Section", section));
			sqlCommand.Parameters.Add(new SqlParameter("@SeatNumber", (object)seatNumber));
			sqlCommand.Parameters.Add(new SqlParameter("@PledgeFundID", (object)SeatPledge.PledgeFundID));
			dbConnection.Open();
			sqlDataAdapter.Fill(dataSet);
			dbConnection.Close();
			return dataSet.Tables[0];
		}

		public SqlDataReader GetSeatPledgeByAssignedSeatID(int assignedSeatID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@AssignedSeatID", (object)assignedSeatID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byAssignedSeatID", arrayLists);
		}

		public SqlDataReader GetSeatPledgeByID(int seatPledgeID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)seatPledgeID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byID", arrayLists);
		}

		public SqlDataReader GetSeatPledgeByPersonID(int personID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@PersonID", (object)personID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byPersonID", arrayLists);
		}

		public SqlDataReader GetSeatPledgeByUnassignedSeat()
		{
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byUnassignedSeat", new ArrayList());
		}

		public int SaveSeatPledge(int seatPledgeID, int personID, decimal amount, int? assignedSeatID, int? requestedSeatID, int requestedFullSeat, int requestedBackRest, int requestedLeg1, int requestedLeg2, int requestedLeg3, int requestedLeg4, int requestedArmLeft, int requestedArmRight, string userID)
		{
			object value;
			object obj;
			ArrayList arrayLists = new ArrayList();
			SqlParameter sqlParameter = new SqlParameter();
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)seatPledgeID));
			arrayLists.Add(new SqlParameter("@PersonID", (object)personID));
			arrayLists.Add(new SqlParameter("@Amount", (object)amount));
			ArrayList arrayLists1 = arrayLists;
			if (assignedSeatID.HasValue)
			{
				value = assignedSeatID.Value;
			}
			else
			{
				value = DBNull.Value;
			}
			arrayLists1.Add(new SqlParameter("@AssignedSeatID", value));
			ArrayList arrayLists2 = arrayLists;
			if (requestedSeatID.HasValue)
			{
				obj = requestedSeatID.Value;
			}
			else
			{
				obj = DBNull.Value;
			}
			arrayLists2.Add(new SqlParameter("@RequestedSeatID", obj));
			arrayLists.Add(new SqlParameter("@RequestedFullSeat", (object)requestedFullSeat));
			arrayLists.Add(new SqlParameter("@RequestedBackRest", (object)requestedBackRest));
			arrayLists.Add(new SqlParameter("@RequestedLeg1", (object)requestedLeg1));
			arrayLists.Add(new SqlParameter("@RequestedLeg2", (object)requestedLeg2));
			arrayLists.Add(new SqlParameter("@RequestedLeg3", (object)requestedLeg3));
			arrayLists.Add(new SqlParameter("@RequestedLeg4", (object)requestedLeg4));
			arrayLists.Add(new SqlParameter("@RequestedArmLeft", (object)requestedArmLeft));
			arrayLists.Add(new SqlParameter("@RequestedArmRight", (object)requestedArmRight));
			arrayLists.Add(new SqlParameter("@UserID", userID));
			sqlParameter.ParameterName = "@ID";
			sqlParameter.Direction = ParameterDirection.Output;
			sqlParameter.SqlDbType = SqlDbType.Int;
			arrayLists.Add(sqlParameter);
			base.ExecuteNonQuery("cust_sothc_mitm_sp_save_seatPledge", arrayLists);
			return (int)sqlParameter.Value;
		}
	}
}