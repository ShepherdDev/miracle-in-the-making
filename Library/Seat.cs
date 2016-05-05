using System;
using System.Data.SqlClient;

using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;


namespace Arena.Custom.SOTHC.MiTM
{
    [Serializable]
    public class Seat : ArenaObjectBase
    {
        #region Private Properties

        private DateTime _dateCreated, _dateModified;
        private String _createdBy, _modifiedBy;

        #endregion


        #region Public Properties

        public int SeatID { get; set; }

        public DateTime DateCreated { get { return _dateCreated; } }

        public DateTime DateModified { get { return _dateModified; } }

        public String CreatedBy { get { return _createdBy; } }

        public String ModifiedBy { get { return _modifiedBy; } }

        public String Section { get; set; }

        public int Number { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a new, blank, Seat record that can be filled in and saved.
        /// </summary>
        public Seat()
        {
            SeatID = -1;
            _dateCreated = DateTime.Now;
            _dateModified = DateTime.Now;
            _createdBy = String.Empty;
            _modifiedBy = String.Empty;
            Section = String.Empty;
            Number = -1;
        }


        /// <summary>
        /// Load an existing Seat record given it's unique ID number.
        /// </summary>
        /// <param name="seatID">The ID number of the Seat record to be loaded from the database.</param>
        public Seat(int seatID) : this()
        {
            SqlDataReader reader = new SeatData().GetSeatByID(seatID);


            if (reader.Read())
                LoadSeat(reader);

            reader.Close();
        }


        /// <summary>
        /// Load an existing Seat record given it's section and number.
        /// </summary>
        /// <param name="section">The single character section of the seat.</param>
        /// <param name="seatNumber">The seat number in this section.</param>
        public Seat(string section, int seatNumber)
            : this()
        {
            SqlDataReader reader = new SeatData().GetSeatBySectionAndNumber(section, seatNumber);


            if (reader.Read())
                LoadSeat(reader);

            reader.Close();
        }


        /// <summary>
        /// Load an existing Seat record with the data provided in the SqlDataReader.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the columns to read from.</param>
        public Seat(SqlDataReader reader) : this()
        {
            LoadSeat(reader);
        }


        /// <summary>
        /// Find the next available seat which has room for the specified amount of
        /// money to be pledged towards it.
        /// </summary>
        /// <param name="amount">The amount of money that must be available on the seat.</param>
        /// <param name="ignoreSeatPledgeID">The SeatPledge ID number to ignore when calculating.</param>
        static public Seat NextAvailableSeat(Decimal amount, int ignoreSeatPledgeID)
        {
            SqlDataReader reader = new SeatData().GetSeatByNextAvailable(amount, ignoreSeatPledgeID);
            Seat seat = null;


            if (reader.Read())
                seat = new Seat(reader);

            reader.Close();

            return seat;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Save all changes that have been made to this Seat record.
        /// </summary>
        /// <param name="userID">The username of the logged in person doing the modification.</param>
        public void Save(String userID)
        {
            new SeatData().SaveSeat(SeatID,
                Section,
                Number,
                userID);
        }


        /// <summary>
        /// Delete the current Seat record.
        /// </summary>
        public void Delete()
        {
            Seat.Delete(SeatID);
            SeatID = -1;
        }


        /// <summary>
        /// Delete the specified Seat record given it's ID number.
        /// </summary>
        /// <param name="seatID">The ID number of the Seat record to be deleted.</param>
        public static void Delete(int seatID)
        {
            new SeatData().DeleteSeat(seatID);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Use the data in reader to fill in the fields for this object.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the column information for the Seat record.</param>
        private void LoadSeat(SqlDataReader reader)
        {
            SeatID = (int)reader["seat_id"];
            _dateCreated = (DateTime)reader["date_created"];
            _dateModified = (DateTime)reader["date_modified"];
            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();
            Section = reader["section"].ToString();
            Number = (int)reader["seat_number"];
        }

        #endregion
    }
}
