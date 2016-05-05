using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Arena.DataLib;

namespace Arena.Custom.SOTHC.MiTM.DataLayer
{
    /// <summary>
    /// Data-layer code for working with Salvation records in the database.
    /// </summary>
    public class SalvationData : SqlData
    {
        /// <summary>
        /// Recommended Constructor.
        /// </summary>
        public SalvationData()
        {
        }


        /// <summary>
        /// Retrieve a single salvation record based on it's unique ID.
        /// </summary>
        /// <param name="salvationID">The salvation_id of the record to retrieve.</param>
        /// <returns>A SqlDataReader that contains the information about the rows retrieved.</returns>
        public SqlDataReader GetSalvationByID(int salvationID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SalvationID", salvationID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_salvation_byID", parms);
        }


        /// <summary>
        /// Retrieve all salvation records based on their person_id.
        /// </summary>
        /// <param name="personID">The person_id we wish to retrieve records for.</param>
        /// <returns>A SqlDataReader that contains the information about the rows retrieved.</returns>
        public SqlDataReader GetSalvationByPersonID(int personID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@PersonID", personID));

            return this.ExecuteSqlDataReader("cust_sothc_mitm_sp_get_salvation_byPersonID", parms);
        }


        /// <summary>
        /// Retrieve a DataTable that contains a list of SQL rows with the results of the search.
        /// </summary>
        /// <param name="personID">The personID to filter by, or -1 to not filter.</param>
        /// <returns>A new DataTable reference.</returns>
        public DataTable GetSalvation_DT(int personID)
        {
            SqlConnection dbConnection = new SqlDbConnection().GetDbConnection();
            SqlCommand select = new SqlCommand();
            StringBuilder statement = new StringBuilder();
            SqlDataAdapter adapter = new SqlDataAdapter(select);
            DataSet dataSet = new DataSet("Salvation");


            //
            // Build up the SQL select statement.
            //
            statement.Append("SELECT * FROM cust_sothc_mitm_salvation");

            //
            // Build up the WHERE statement.
            //
            statement.Append(" WHERE (1 = 1)");
            if (personID != -1)
                statement.Append(" AND person_id = @PersonID");

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
            select.Parameters.Add(new SqlParameter("@PersonID", personID));

            //
            // Run the query.
            //
            dbConnection.Open();
            adapter.Fill(dataSet);
            dbConnection.Close();

            return dataSet.Tables[0];
        }


        /// <summary>
        /// Delete a single salvation record based on it's salvation_id.
        /// </summary>
        /// <param name="salvationID">The ID number of the record to delete from the database.</param>
        public void DeleteSalvation(int salvationID)
        {
            ArrayList parms = new ArrayList();


            parms.Add(new SqlParameter("@SalvationID", salvationID));

            this.ExecuteNonQuery("cust_sothc_mitm_sp_del_salvation", parms);
        }


        /// <summary>
        /// Update or Create a salvation record with the given parameter values.
        /// </summary>
        /// <param name="salvationID">The ID number of the existing record to update or -1 to create a new record.</param>
        /// <param name="personID">The person_id value to be updated.</param>
        /// <param name="firstName">The first_name of the person who is being prayed for.</param>
        /// <param name="lastName">The last_name of the person who is being prayed for.</param>
        /// <param name="status">The salvation status of this person, wether or not they have accepted Christ.</param>
        /// <param name="userID">The username of the person who is performing the update.</param>
        /// <returns>The salvation_id of the record that was either updated or created.</returns>
        public int SaveSalvation(int salvationID, int personID, String firstName, string lastName, Boolean status, String userID)
        {
            ArrayList parms = new ArrayList();
            SqlParameter parmOut = new SqlParameter();


            parms.Add(new SqlParameter("@SalvationID", salvationID));
            parms.Add(new SqlParameter("@PersonID", personID));
            parms.Add(new SqlParameter("@FirstName", firstName));
            parms.Add(new SqlParameter("@LastName", lastName));
            parms.Add(new SqlParameter("@Status", status));
            parms.Add(new SqlParameter("@UserID", userID));

            parmOut.ParameterName = "@ID";
            parmOut.Direction = ParameterDirection.Output;
            parmOut.SqlDbType = SqlDbType.Int;
            parms.Add(parmOut);

            this.ExecuteNonQuery("cust_sothc_mitm_sp_save_salvation", parms);

            return (int)parmOut.Value;
        }
    }
}
