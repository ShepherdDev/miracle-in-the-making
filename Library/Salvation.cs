using System;
using System.Data.SqlClient;

using Arena.Core;

using Arena.Custom.SOTHC.MiTM.DataLayer;


namespace Arena.Custom.SOTHC.MiTM
{
    [Serializable]
    public class Salvation : ArenaObjectBase
    {
        #region Private Properties

        private DateTime _dateCreated, _dateModified;
        private String _createdBy, _modifiedBy;

        private Person _person;
        private int _personID;

        #endregion


        #region Public Properties

        public int SalvationID { get; set; }

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

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public Boolean Status { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Create a new, blank, Salvation record that can be filled in and saved.
        /// </summary>
        public Salvation()
        {
            SalvationID = -1;
            _dateCreated = DateTime.Now;
            _dateModified = DateTime.Now;
            _createdBy = String.Empty;
            _modifiedBy = String.Empty;
            _personID = -1;
            FirstName = String.Empty;
            LastName = String.Empty;
            Status = false;
        }


        /// <summary>
        /// Load an existing Salvation record given it's unique ID number.
        /// </summary>
        /// <param name="salvationID">The ID number of the Salvation record to be loaded from the database.</param>
        public Salvation(int salvationID) : this()
        {
            SqlDataReader reader = new SalvationData().GetSalvationByID(salvationID);


            if (reader.Read())
                LoadSalvation(reader);

            reader.Close();
        }


        /// <summary>
        /// Load an existing Salvation record with the data provided in the SqlDataReader.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the columns to read from.</param>
        public Salvation(SqlDataReader reader) : this()
        {
            LoadSalvation(reader);
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Save all changes that have been made to this Salvation record.
        /// </summary>
        /// <param name="userID">The username of the logged in person doing the modification.</param>
        public void Save(String userID)
        {
            new SalvationData().SaveSalvation(SalvationID,
                _personID,
                FirstName,
                LastName,
                Status,
                userID);
        }


        /// <summary>
        /// Delete the current Salvation record.
        /// </summary>
        public void Delete()
        {
            Salvation.Delete(SalvationID);
            SalvationID = -1;
        }


        /// <summary>
        /// Delete the specified Salvation record given it's ID number.
        /// </summary>
        /// <param name="salvationID">The ID number of the Salvation record to be deleted.</param>
        public static void Delete(int salvationID)
        {
            new SalvationData().DeleteSalvation(salvationID);
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Use the data in reader to fill in the fields for this object.
        /// </summary>
        /// <param name="reader">The SqlDataReader that contains the column information for the Salvation record.</param>
        private void LoadSalvation(SqlDataReader reader)
        {
            SalvationID = (int)reader["salvation_id"];
            _dateCreated = (DateTime)reader["date_created"];
            _dateModified = (DateTime)reader["date_modified"];
            _createdBy = reader["created_by"].ToString();
            _modifiedBy = reader["modified_by"].ToString();
            _personID = (int)reader["person_id"];
            FirstName = reader["first_name"].ToString();
            LastName = reader["last_name"].ToString();
            Status = (Boolean)reader["status"];
        }

        #endregion
    }
}
