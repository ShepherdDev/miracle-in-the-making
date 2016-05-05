using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Arena.DataLib;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
    /// <summary>
    /// Provides the data-layer interface for working with individual Seat objects
    /// in the database.
    /// </summary>
    public class SeatData : SqlData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SeatData()
        {
        }


        /// <summary>
        /// Retrieve the data of a seat given it's ID number.
        /// </summary>
        /// <param name="seatID">The seat_id for the seat that we wish to retrieve.</param>
        /// <returns>A SqlDataReader that contains the row data for the requested seat.</returns>
        public SqlDataReader GetSeatByID(int seatID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SeatID", seatID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_byID", parms);
        }


        /// <summary>
        /// Retrieve the data of a collection of seats given their section code.
        /// </summary>
        /// <param name="section">The section for the seats that we wish to retrieve.</param>
        /// <returns>A SqlDataReader that contains the row data for the requested seats.</returns>
        public SqlDataReader GetSeatBySection(String section)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@Section", section));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_bySection", parms);
        }


        /// <summary>
        /// Retrieve the data of a seat given it's section code and seat number.
        /// </summary>
        /// <param name="section">The section for the seat that we wish to retrieve.</param>
        /// <param name="seatNumber">The seat_number for the seat that we wish to retrieve.</param>
        /// <returns>A SqlDataReader that contains the row data for the requested seat.</returns>
        public SqlDataReader GetSeatBySectionAndNumber(String section, int seatNumber)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@Section", section));
            parms.Add(new SqlParameter("@SeatNumber", seatNumber));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_bySectionAndNumber", parms);
        }


        /// <summary>
        /// Retrieve the data of a seat by finding the next available seat for the amount of
        /// money specified.
        /// </summary>
        /// <param name="amount">The amount of money that is required for the new pledge.</param>
        /// <param name="ignoreSeatPledgeID">The SeatPledgeID that should be ignored, for example when creating a new pledge.</param>
        /// <returns>A SqlDataReader that contains the row data for the requested seat.</returns>
        public SqlDataReader GetSeatByNextAvailable(Decimal amount, int ignoreSeatPledgeID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@Amount", amount));
            parms.Add(new SqlParameter("@SeatPledgeID", ignoreSeatPledgeID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seat_byNextAvailable", parms);
        }


        /// <summary>
        /// Delete the seat as specified by it's unique seat_id number.
        /// </summary>
        /// <param name="seatID">The seat to be deleted from the database.</param>
        public void DeleteSeat(int seatID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SeatID", seatID));

            this.ExecuteNonQuery("cust_sothc_mitm_sp_del_seat", parms);
        }


        /// <summary>
        /// Save (or create) a seat given the information describing it.
        /// </summary>
        /// <param name="seatID">The existing seat_id to update or -1 to create a new seat.</param>
        /// <param name="section">A single character string which identifies the section the seat is in.</param>
        /// <param name="seatNumber">The seat's number in the section.</param>
        /// <param name="userID">The user who is performing the save.</param>
        /// <returns>The seat_id of the seat that was updated or created.</returns>
        public int SaveSeat(int seatID, String section, int seatNumber, String userID)
        {
            ArrayList parms = new ArrayList();
            SqlParameter parmOut = new SqlParameter();


            parms.Add(new SqlParameter("@SeatID", seatID));
            parms.Add(new SqlParameter("@Section", section));
            parms.Add(new SqlParameter("@SeatNumber", seatNumber));
            parms.Add(new SqlParameter("@UserID", userID));

            parmOut.ParameterName = "@ID";
            parmOut.Direction = ParameterDirection.Output;
            parmOut.SqlDbType = SqlDbType.Int;
            parms.Add(parmOut);

            this.ExecuteNonQuery("cust_sothc_mitm_sp_save_seat", parms);

            return (int)parmOut.Value;
        }
    }
}
