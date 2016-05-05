using Arena.DataLib;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
	public class SeatData : SqlData
	{
		public SeatData()
		{
		}

		public void DeleteSeat(int seatID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SeatID", (object)seatID));
			base.ExecuteNonQuery("cust_sothc_mitm_sp_del_seat", arrayLists);
		}

		public SqlDataReader GetSeatByID(int seatID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@SeatID", (object)seatID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_byID", arrayLists);
		}

		public SqlDataReader GetSeatByNextAvailable(decimal amount, int ignoreSeatPledgeID)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@Amount", (object)amount));
			arrayLists.Add(new SqlParameter("@SeatPledgeID", (object)ignoreSeatPledgeID));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_byNextAvailable", arrayLists);
		}

		public SqlDataReader GetSeatBySection(string section)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@Section", section));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_bySection", arrayLists);
		}

		public SqlDataReader GetSeatBySectionAndNumber(string section, int seatNumber)
		{
			ArrayList arrayLists = new ArrayList();
			arrayLists.Add(new SqlParameter("@Section", section));
			arrayLists.Add(new SqlParameter("@SeatNumber", (object)seatNumber));
			return base.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_bySectionAndNumber", arrayLists);
		}

		public int SaveSeat(int seatID, string section, int seatNumber, string userID)
		{
			ArrayList arrayLists = new ArrayList();
			SqlParameter sqlParameter = new SqlParameter();
			arrayLists.Add(new SqlParameter("@SeatID", (object)seatID));
			arrayLists.Add(new SqlParameter("@Section", section));
			arrayLists.Add(new SqlParameter("@SeatNumber", (object)seatNumber));
			arrayLists.Add(new SqlParameter("@UserID", userID));
			sqlParameter.ParameterName = "@ID";
			sqlParameter.Direction = ParameterDirection.Output;
			sqlParameter.SqlDbType = SqlDbType.Int;
			arrayLists.Add(sqlParameter);
			base.ExecuteNonQuery("cust_sothc_mitm_sp_save_seat", arrayLists);
			return (int)sqlParameter.Value;
		}
	}
}