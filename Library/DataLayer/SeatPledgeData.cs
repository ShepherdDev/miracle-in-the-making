using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Arena.DataLib;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
    public class SeatPledgeData : SqlData
    {
        /// <summary>
        /// Recommended Constructor.
        /// </summary>
        public SeatPledgeData()
        {
        }


        /// <summary>
        /// Retrieve a single SeatPledge record based on it's unique ID.
        /// </summary>
        /// <param name="seatPledgeID">The seat_pledge_id of the record to retrieve.</param>
        /// <returns>A SqlDataReader that contains the information about the record(s) being retrieved.</returns>
        public SqlDataReader GetSeatPledgeByID(int seatPledgeID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SeatPledgeID", seatPledgeID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byID", parms);
        }


        /// <summary>
        /// Retrieve all the SeatPledges that have been made by the given Person.
        /// </summary>
        /// <param name="personID">The person_id of the Person whose pledges we are interested in.</param>
        /// <returns>A SqlDataReader that contains the information about the record(s) being retrieved.</returns>
        public SqlDataReader GetSeatPledgeByPersonID(int personID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@PersonID", personID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byPersonID", parms);
        }


        /// <summary>
        /// Retrieve all the SeatPledges that are assigned to the specified seat.
        /// </summary>
        /// <param name="assignedSeatID">The seat_id which identifies the Seat whose pledges we are interested in.</param>
        /// <returns>A SqlDataReader that contains the information about the record(s) being retrieved.</returns>
        public SqlDataReader GetSeatPledgeByAssignedSeatID(int assignedSeatID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@AssignedSeatID", assignedSeatID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byAssignedSeatID", parms);
        }


        /// <summary>
        /// Retrieve all the SeatPledges that have not yet been assigned to a seat.
        /// </summary>
        /// <returns>A SqlDataReader that contains the information about the record(s) being retrieved.</returns>
        public SqlDataReader GetSeatPledgeByUnassignedSeat()
        {
            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_seatPledge_byUnassignedSeat", new ArrayList());
        }


        /// <summary>
        /// Retrieve a DataTable that contains a list of SQL rows with the results of the search.
        /// </summary>
        /// <param name="section">The section letter to search for, or empty for all sections.</param>
        /// <param name="seatNumber">The seat number to search for, or -1 for all seats.</param>
        /// <param name="unassignedSeat">If you only want to list unassigned seat pledges, set this value to true.</param>
        /// <returns>A new DataTable reference.</returns>
        public DataTable GetSeatPledge_DT(String section, int seatNumber, Boolean unassignedSeat)
        {
            SqlConnection dbConnection = new SqlDbConnection().GetDbConnection();
            SqlCommand select = new SqlCommand();
            StringBuilder statement = new StringBuilder();
            SqlDataAdapter adapter = new SqlDataAdapter(select);
            DataSet dataSet = new DataSet("SeatPledge");


            //
            // Build up the SQL select statement.
            //
            statement.Append("SELECT * FROM cust_sothc_mitm_v_seatPledgeList");

            //
            // Build up the WHERE statement.
            //
            statement.Append(" WHERE (1 = 1)");
            if (!String.IsNullOrEmpty(section))
                statement.Append(" AND assigned_section = @Section");
            if (seatNumber != -1)
                statement.Append(" AND assigned_seat_number = @SeatNumber");
            if (unassignedSeat == true)
                statement.Append(" AND assigned_seat_id IS NULL");

            //
            // Setup the actual query to run.
            //
            select.CommandTimeout = 360;
            select.Connection = dbConnection;
            select.CommandType = CommandType.Text;
            select.CommandText = statement.ToString();

            //
            // Add in all the parameters.
            //
            select.Parameters.Add(new SqlParameter("@Section", section));
            select.Parameters.Add(new SqlParameter("@SeatNumber", seatNumber));
            select.Parameters.Add(new SqlParameter("@PledgeFundID", SeatPledge.PledgeFundID));

            //
            // Run the query.
            //
            dbConnection.Open();
            adapter.Fill(dataSet);
            dbConnection.Close();

            return dataSet.Tables[0];
        }


        /// <summary>
        /// Delete a single SeatPledge record based on it's unique ID.
        /// </summary>
        /// <param name="seatPledgeID">The seat_pledge_id of the record to delete from the database.</param>
        public void DeleteSeatPledge(int seatPledgeID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SeatPledgeID", seatPledgeID));

            this.ExecuteNonQuery("cust_sothc_mitm_sp_del_seatPledge", parms);
        }


        /// <summary>
        /// Update or Create a SeatPledge record with the information passed in the parameters.
        /// </summary>
        /// <param name="seatPledgeID">The seat_pledge_id of the record to update or -1 to create a new record.</param>
        /// <param name="personID">The person who has promised to make this pledge.</param>
        /// <param name="amount">The amount of money pledged towards this seat.</param>
        /// <param name="assignedSeatID">The assigned seat_id for this pledge or -1 if it has not been assigned yet.</param>
        /// <param name="requestedSeatID">The requested seat_id that was made when the pledge was originally entered.</param>
        /// <param name="requestedFullSeat">The number of "full seats" the user requested.</param>
        /// <param name="requestedBackRest">The number of "back rests" the user requested.</param>
        /// <param name="requestedLeg1">The number of "leg 1s" the user requested.</param>
        /// <param name="requestedLeg2">The number of "leg 2s" the user requested.</param>
        /// <param name="requestedLeg3">The number of "leg 3s" the user requested.</param>
        /// <param name="requestedLeg4">The number of "leg 4s" the user requested.</param>
        /// <param name="requestedArmLeft">The number of "arm lefts" the user requested.</param>
        /// <param name="requestedArmRight">The number of "arm rights" the user requested.</param>
        /// <param name="userID">The username that is updating this record.</param>
        /// <returns>The seat_pledge_id of the record that was updated or created.</returns>
        public int SaveSeatPledge(int seatPledgeID, int personID, decimal amount, int? assignedSeatID, int? requestedSeatID, int requestedFullSeat, int requestedBackRest, int requestedLeg1, int requestedLeg2, int requestedLeg3, int requestedLeg4, int requestedArmLeft, int requestedArmRight, String userID)
        {
            ArrayList parms = new ArrayList();
            SqlParameter parmOut = new SqlParameter();


            parms.Add(new SqlParameter("@SeatPledgeID", seatPledgeID));
            parms.Add(new SqlParameter("@PersonID", personID));
            parms.Add(new SqlParameter("@Amount", amount));
            parms.Add(new SqlParameter("@AssignedSeatID", (assignedSeatID.HasValue ? (object)assignedSeatID.Value : DBNull.Value)));
            parms.Add(new SqlParameter("@RequestedSeatID", (requestedSeatID.HasValue ? (object)requestedSeatID.Value : DBNull.Value)));
            parms.Add(new SqlParameter("@RequestedFullSeat", requestedFullSeat));
            parms.Add(new SqlParameter("@RequestedBackRest", requestedBackRest));
            parms.Add(new SqlParameter("@RequestedLeg1", requestedLeg1));
            parms.Add(new SqlParameter("@RequestedLeg2", requestedLeg2));
            parms.Add(new SqlParameter("@RequestedLeg3", requestedLeg3));
            parms.Add(new SqlParameter("@RequestedLeg4", requestedLeg4));
            parms.Add(new SqlParameter("@RequestedArmLeft", requestedArmLeft));
            parms.Add(new SqlParameter("@RequestedArmRight", requestedArmRight));
            parms.Add(new SqlParameter("@UserID", userID));

            parmOut.ParameterName = "@ID";
            parmOut.Direction = ParameterDirection.Output;
            parmOut.SqlDbType = SqlDbType.Int;
            parms.Add(parmOut);

            this.ExecuteNonQuery("cust_sothc_mitm_sp_save_seatPledge", parms);

            return (int)parmOut.Value;
        }
    }
}
