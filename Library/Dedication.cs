using System;
using System.Data.SqlClient;

using Arena.Core;
using Arena.Utility;

using Arena.Custom.SOTHC.MiTM.DataLayer;


namespace Arena.Custom.SOTHC.MiTM
{
    [Serializable]
    public class Dedication : ArenaObjectBase
    {
        #region Private Properties

        private DateTime _dateCreated, _dateModified;
        private String _createdBy, _modifiedBy;

        private SeatPledge _seatPledge;
        private int _seatPledgeID;

        private ArenaImage _blob;
        private int _blobID;

        #endregion


        #region Public Properties

        public int DedicationID { get; set; }

        public DateTime DateCreated { get { return _dateCreated; } }

        public DateTime DateModified { get { return _dateModified; } }

        public String CreatedBy { get { return _createdBy; } }

        public String ModifiedBy { get { return _modifiedBy; } }

        public String ApprovedBy { get; set; }

        //public SeatPledge SeatPledge
        //{
        //    get
        //    {
        //        if (_seatPledge == null)
        //            _seatPledge = new SeatPledge(_seatPledgeID);

        //        return _seatPledge;
        //    }
        //    set
        //    {
        //        _seatPledge = value;
        //        _seatPledgeID = (_seatPledge != null ? _seatPledge.SeatPledgeID : -1);
        //    }
        //}
        public int SeatPledgeID
        {
            get
            {
                return _seatPledgeID;
            }
            set
            {
                _seatPledgeID = value;
                _seatPledge = (_seatPledgeID != -1 ? new SeatPledge(_seatPledgeID) : null);
            }
        }

        public String DedicatedTo { get; set; }

        public String SponsoredBy { get; set; }

        public String Biography { get; set; }

        public Boolean Anonymous { get; set; }

        public ArenaImage Blob
        {
            get
            {
                if (_blob == null)
                    _blob = new ArenaImage(_blobID);

                return _blob;
            }
            set
            {
                _blob = value;
                _blobID = (_blob != null ? _blob.BlobID : -1);
            }
        }
        public int BlobID
        {
            get
            {
                return _blobID;
            }

            set
            {
                _blobID = value;
                _blob = (_blobID != -1 ? new ArenaImage(_blobID) : null);
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a new, blank, Dedication record that can be filled in and saved.
        /// </summary>
        public Dedication()
        {
            DedicationID = -1;
            _dateCreated = DateTime.Now;
            _dateModified = DateTime.Now;
            _createdBy = String.Empty;
            _modifiedBy = String.Empty;
            ApprovedBy = String.Empty;
            _seatPledgeID = -1;
            DedicatedTo = String.Empty;
            SponsoredBy = String.Empty;
            Biography = String.Empty;
            Anonymous = false;
            _blobID = -1;
        }


        /// <summary>
        /// Load an existing Dedication record given it's unique ID number.
        /// </summary>
        /// <param name="dedicationID">The ID number of the Dedication record to be loaded from the database.</param>
        public Dedication(int dedicationID) : this()
        {
            SqlDataReader reader = new DedicationData().GetDedicationByID(dedicationID);


            if (reader.Read())
                LoadDedication(reader);

            reader.Close();
        }


        /// <summary>
        /// Load an existing Dedication record with the data provided in the SqlDataReader.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the columns to read from.</param>
        public Dedication(SqlDataReader reader) : this()
        {
            LoadDedication(reader);
        }


        /// <summary>
        /// Load the dedication that is associated with the given seat pledge ID.
        /// </summary>
        /// <param name="seatPledgeID">The seat_pledge_id that the dedication record is associated with.</param>
        /// <returns>An existing Dedication record or null if no dedication record for that seat pledge exists.</returns>
        static public Dedication DedicationFromSeatPledgeID(int seatPledgeID)
        {
            SqlDataReader reader = new DedicationData().GetDedicationBySeatPledgeID(seatPledgeID);
            Dedication dedication = null;


            if (reader.Read())
                dedication = new Dedication(reader);
            reader.Close();

            return dedication;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Save all changes that have been made to this Dedication record.
        /// </summary>
        /// <param name="userID">The username of the logged in person doing the modification.</param>
        public void Save(String userID)
        {
            new DedicationData().SaveDedication(DedicationID,
                ApprovedBy,
                _seatPledgeID,
                DedicatedTo,
                SponsoredBy,
                Biography,
                Anonymous,
                (_blobID != -1 ? _blobID : (int?)null),
                userID);
        }


        /// <summary>
        /// Delete the current Dedication record.
        /// </summary>
        public void Delete()
        {
            Dedication.Delete(DedicationID);
            DedicationID = -1;
        }


        /// <summary>
        /// Delete the specified Dedication record given it's ID number.
        /// </summary>
        /// <param name="dedicationID">The ID number of the Dedication record to be deleted.</param>
        public static void Delete(int dedicationID)
        {
            new DedicationData().DeleteDedication(dedicationID);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Use the data in reader to fill in the fields for this object.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the column information for the Dedication record.</param>
        private void LoadDedication(SqlDataReader reader)
        {
            DedicationID = (int)reader["dedication_id"];
            _dateCreated = (DateTime)reader["date_created"];
            _dateModified = (DateTime)reader["date_modified"];
            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();
            ApprovedBy = reader["approved_by"].ToString();
            _seatPledgeID = (int)reader["seat_pledge_id"];
            DedicatedTo = reader["dedicated_to"].ToString();
            SponsoredBy = reader["sponsored_by"].ToString();
            Biography = reader["biography"].ToString();
            Anonymous = (Boolean)reader["anonymous"];
            _blobID = (!reader.IsDBNull(reader.GetOrdinal("blob_id")) ? (int)reader["blob_id"] : -1);
        }

        #endregion
    }
}
