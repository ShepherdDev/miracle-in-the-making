using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Arena.DataLib;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
    /// <summary>
    /// Data class for working with Dedication records in the database.
    /// </summary>
    public class DedicationData : SqlData
    {
        /// <summary>
        /// Recommended Constructor.
        /// </summary>
        public DedicationData()
        {
        }


        /// <summary>
        /// Retrieve a single Dedication record based on it's unique ID number.
        /// </summary>
        /// <param name="dedicationID">The dedication_id value of the record to retrieve.</param>
        /// <returns>A SqlDataReader which identifies the record(s) that have been found in the database.</returns>
        public SqlDataReader GetDedicationByID(int dedicationID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@DedicationID", dedicationID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_byID", parms);
        }


        /// <summary>
        /// Retrieve a single Dedication record based on their seat_pledge_id value.
        /// </summary>
        /// <param name="seatPledgeID">The seat_pledge_id value of the records to retrieve.</param>
        /// <returns>A SqlDataReader which identifies the record that has been found in the database.</returns>
        public SqlDataReader GetDedicationBySeatPledgeID(int seatPledgeID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SeatPledgeID", seatPledgeID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_bySeatPledgeID", parms);
        }


        /// <summary>
        /// Retrieve all the Dedication records that have not yet been approved.
        /// </summary>
        /// <returns>A SqlDataReader which identifies the record(s) that have been found in the database.</returns>
        public SqlDataReader GetDedicationByUnapproved()
        {
            ArrayList parms = new ArrayList();


            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_dedication_byUnapproved", parms);
        }


        /// <summary>
        /// Retrieve a DataTable that contains a list of SQL rows with the results of the search.
        /// </summary>
        /// <param name="section">1 for only approved dedications, 0 for unapproved, -1 for all.</param>
        /// <returns>A new DataTable reference.</returns>
        public DataTable GetDedication_DT(int approvalStatus)
        {
            SqlConnection dbConnection = new SqlDbConnection().GetDbConnection();
            SqlCommand select = new SqlCommand();
            StringBuilder statement = new StringBuilder();
            SqlDataAdapter adapter = new SqlDataAdapter(select);
            DataSet dataSet = new DataSet("Dedication");


            //
            // Build up the SQL select statement.
            //
            statement.Append("SELECT * FROM cust_sothc_mitm_v_dedicationList");

            //
            // Build up the WHERE statement.
            //
            statement.Append(" WHERE 1 = 1");
            if (approvalStatus != -1)
                statement.Append(" AND is_approved = @ApprovalStatus");

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
            select.Parameters.Add(new SqlParameter("@ApprovalStatus", approvalStatus));

            //
            // Run the query.
            //
            dbConnection.Open();
            adapter.Fill(dataSet);
            dbConnection.Close();

            return dataSet.Tables[0];
        }


        /// <summary>
        /// Delete the identified Dedication record from the database.
        /// </summary>
        /// <param name="dedicationID">The record's dedication_id that the user wants to remove.</param>
        public void DeleteDedication(int dedicationID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@DedicationID", dedicationID));

            this.ExecuteSqlDataReader("cust_sothc_mitm_sp_del_dedication", parms);
        }


        /// <summary>
        /// Update or Create a single Dedication record in the database with the values from the parameters.
        /// </summary>
        /// <param name="dedicationID">The dedication_id of the record to update or -1 to create a new Dedication record.</param>
        /// <param name="approvedBy">The username of the person who approved the Dedication, or String.Empty if it is not approved yet.</param>
        /// <param name="seatPledgeID">The ID number of the SeatPledge to link this Dedication to.</param>
        /// <param name="dedicatedTo">A short dedication message.</param>
        /// <param name="sponsoredBy">The name of the person or company that sponsored the pledge.</param>
        /// <param name="biography">A short biography of the pledge and why it was made.</param>
        /// <param name="anonymous">Wether or not the pledger wishes to remain anonymous.</param>
        /// <param name="blobID">The ID number of the blob which contains a picture for the Dedication, or -1 if no picture was uploaded.</param>
        /// <param name="userID">The username of the person who is making this change.</param>
        /// <returns>The ID number of the Dedication record that was created/updated.</returns>
        public int SaveDedication(int dedicationID, String approvedBy, int seatPledgeID, String dedicatedTo, String sponsoredBy, String biography, Boolean anonymous, int? blobID, String userID)
        {
            ArrayList parms = new ArrayList();
            SqlParameter parmOut = new SqlParameter();


            parms.Add(new SqlParameter("@DedicationID", dedicationID));
            parms.Add(new SqlParameter("@ApprovedBy", approvedBy));
            parms.Add(new SqlParameter("@SeatPledgeID", seatPledgeID));
            parms.Add(new SqlParameter("@DedicatedTo", dedicatedTo));
            parms.Add(new SqlParameter("@SponsoredBy", sponsoredBy));
            parms.Add(new SqlParameter("@Biography", biography));
            parms.Add(new SqlParameter("@Anonymous", anonymous));
            parms.Add(new SqlParameter("@BlobID", (blobID.HasValue ? (object)blobID.Value : DBNull.Value)));
            parms.Add(new SqlParameter("@UserID", userID));

            parmOut.ParameterName = "@ID";
            parmOut.Direction = ParameterDirection.Output;
            parmOut.SqlDbType = SqlDbType.Int;
            parms.Add(parmOut);

            this.ExecuteNonQuery("cust_sothc_mitm_sp_save_dedication", parms);

            return (int)parmOut.Value;
        }
    }
}
