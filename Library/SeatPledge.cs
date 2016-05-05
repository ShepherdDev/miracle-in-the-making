using System;
using System.Data.SqlClient;

using Arena.Core;

using Arena.Custom.SOTHC.MiTM.DataLayer;


namespace Arena.Custom.SOTHC.MiTM
{
    [Serializable]
    public class SeatPledge : ArenaObjectBase
    {
        #region Private Properties

        private DateTime _dateCreated, _dateModified;
        private String _createdBy, _modifiedBy;

        private Person _person;
        private int _personID;

        private Seat _assignedSeat;
        private int _assignedSeatID;

        private Seat _requestedSeat;
        private int _requestedSeatID;

        private Dedication _dedication;

        #endregion


        #region Public Properties

        public int SeatPledgeID { get; set; }

        public DateTime DateCreated { get { return _dateCreated; } }

        public DateTime DateModified { get { return _dateModified; } }

        public String CreatedBy { get { return _createdBy; } }

        public String ModifiedBy { get { return _modifiedBy; } }

        public Person Person
        {
            get
            {
                if (_person == null)
                    _person = new Person(_personID);

                return _person;
            }
            set
            {
                _person = value;
                _personID = (_person != null ? _person.PersonID : -1);
            }
        }
        public int PersonID
        {
            get
            {
                return _personID;
            }
            set
            {
                _personID = value;
                _person = (_personID != -1 ? new Person(_personID) : null);
            }
        }

        public Decimal Amount { get; set; }

        public Seat AssignedSeat
        {
            get
            {
                if (_assignedSeat == null)
                    _assignedSeat = new Seat(_assignedSeatID);

                return _assignedSeat;
            }
            set
            {
                _assignedSeat = value;
                _assignedSeatID = (_assignedSeat != null ? _assignedSeat.SeatID : -1);
            }
        }
        public int AssignedSeatID
        {
            get
            {
                return _assignedSeatID;
            }
            set
            {
                _assignedSeatID = value;
                _assignedSeat = (_assignedSeatID != -1 ? new Seat(_assignedSeatID) : null);
            }
        }

        public Seat RequestedSeat
        {
            get
            {
                if (_requestedSeat == null)
                    _requestedSeat = new Seat(_requestedSeatID);

                return _requestedSeat;
            }
            set
            {
                _requestedSeat = value;
                _requestedSeatID = (_requestedSeat != null ? _requestedSeat.SeatID : -1);
            }
        }
        public int RequestedSeatID
        {
            get
            {
                return _requestedSeatID;
            }
            set
            {
                _requestedSeatID = value;
                _requestedSeat = (_requestedSeatID != -1 ? new Seat(_requestedSeatID) : null);
            }
        }

        public int RequestedFullSeat { get; set; }

        public int RequestedBackRest { get; set; }

        public int RequestedLeg1 { get; set; }

        public int RequestedLeg2 { get; set; }

        public int RequestedLeg3 { get; set; }

        public int RequestedLeg4 { get; set; }

        public int RequestedArmLeft { get; set; }

        public int RequestedArmRight { get; set; }

        public Dedication Dedication
        {
            get
            {
                if (_dedication == null)
                {
                    SqlDataReader reader = new DedicationData().GetDedicationBySeatPledgeID(SeatPledgeID);

                    if (reader.Read())
                    {
                        _dedication = new Dedication(reader);
                    }
                    else
                    {
                        _dedication = new Dedication();
                        if (SeatPledgeID != -1)
                            Dedication.SeatPledgeID = SeatPledgeID;
                    }
                    reader.Close();
                }

                return _dedication;
            }
        }

        #endregion


        #region Static Properties

        public static int PledgeFundID { get { return 49; } }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a new, blank, SeatPledge record that can be filled in and saved.
        /// </summary>
        public SeatPledge()
        {
            SeatPledgeID = -1;
            _dateCreated = DateTime.Now;
            _dateModified = DateTime.Now;
            _createdBy = String.Empty;
            _modifiedBy = String.Empty;
            _personID = -1;
            Amount = 0;
            _assignedSeatID = -1;
            _requestedSeatID = -1;
            RequestedFullSeat = 0;
            RequestedBackRest = 0;
            RequestedLeg1 = 0;
            RequestedLeg2 = 0;
            RequestedLeg3 = 0;
            RequestedLeg4 = 0;
            RequestedArmLeft = 0;
            RequestedArmRight = 0;
        }


        /// <summary>
        /// Load an existing SeatPledge record given it's unique ID number.
        /// </summary>
        /// <param name="seatPledgeID">The ID number of the SeatPledge record to be loaded from the database.</param>
        public SeatPledge(int seatPledgeID) : this()
        {
            SqlDataReader reader = new SeatPledgeData().GetSeatPledgeByID(seatPledgeID);


            if (reader.Read())
                LoadSeatPledge(reader);

            reader.Close();
        }


        /// <summary>
        /// Load an existing SeatPledge record with the data provided in the SqlDataReader.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the columns to read from.</param>
        public SeatPledge(SqlDataReader reader) : this()
        {
            LoadSeatPledge(reader);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Save all changes that have been made to this SeatPledge record.
        /// </summary>
        /// <param name="userID">The username of the logged in person doing the modification.</param>
        public void Save(String userID)
        {
            SeatPledgeID = new SeatPledgeData().SaveSeatPledge(SeatPledgeID,
                _personID,
                Amount,
                (_assignedSeatID != -1 ? _assignedSeatID : (int?)null),
                (_requestedSeatID != -1 ? _requestedSeatID : (int?)null),
                RequestedFullSeat,
                RequestedBackRest,
                RequestedLeg1,
                RequestedLeg2,
                RequestedLeg3,
                RequestedLeg4,
                RequestedArmLeft,
                RequestedArmRight,
                userID);

            if (Dedication.SeatPledgeID == -1)
                Dedication.SeatPledgeID = SeatPledgeID;
        }


        /// <summary>
        /// Delete the current SeatPledge record.
        /// </summary>
        public void Delete()
        {
            SeatPledge.Delete(SeatPledgeID);
            SeatPledgeID = -1;
        }


        /// <summary>
        /// Delete the specified SeatPledge record given it's ID number.
        /// </summary>
        /// <param name="seatPledgeID">The ID number of the SeatPledge record to be deleted.</param>
        public static void Delete(int seatPledgeID)
        {
            new SeatPledgeData().DeleteSeatPledge(seatPledgeID);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Use the data in reader to fill in the fields for this object.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the column information for the SeatPledge record.</param>
        private void LoadSeatPledge(SqlDataReader reader)
        {
            SeatPledgeID = (int)reader["seat_pledge_id"];
            _dateCreated = (DateTime)reader["date_created"];
            _dateModified = (DateTime)reader["date_modified"];
            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();
            _personID = (int)reader["person_id"];
            Amount = (Decimal)reader["amount"];
            _assignedSeatID = (!reader.IsDBNull(reader.GetOrdinal("assigned_seat_id")) ? (int)reader["assigned_seat_id"] : -1);
            _requestedSeatID = (!reader.IsDBNull(reader.GetOrdinal("requested_seat_id")) ? (int)reader["requested_seat_id"] : -1);
            RequestedFullSeat = (int)reader["requested_full_seat"];
            RequestedBackRest = (int)reader["requested_back_rest"];
            RequestedLeg1 = (int)reader["requested_leg1"];
            RequestedLeg2 = (int)reader["requested_leg2"];
            RequestedLeg3 = (int)reader["requested_leg3"];
            RequestedLeg4 = (int)reader["requested_leg4"];
            RequestedArmLeft = (int)reader["requested_arm_left"];
            RequestedArmRight = (int)reader["requested_arm_right"];
        }

        #endregion
    }
}
