using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;

namespace Arena.Custom.SOTHC.MiTM
{
    [Serializable]
    public class SeatPledgeCollection : ArenaCollectionBase<SeatPledge>
    {
        #region Constructors

        /// <summary>
        /// Create an empty collection.
        /// </summary>
        public SeatPledgeCollection()
        {
        }

        #endregion

        #region Load Methods

        /// <summary>
        /// Populate the collection with all SeatPledge objects that have been made by a specific Person.
        /// </summary>
        /// <param name="personID">The ID number of the Person who's SeatPledges we are interested in.</param>
        public void LoadByPerson(int personID)
        {
            SqlDataReader reader = new SeatPledgeData().GetSeatPledgeByPersonID(personID);


            while (reader.Read())
            {
                base.Add(new SeatPledge(reader));
            }
            reader.Close();
        }


        /// <summary>
        /// Populate the collection with all SeatPledges that have been assigned to a specific seat.
        /// </summary>
        /// <param name="seatID">The ID number of the Seat who's SeatPledges we are interested in.</param>
        public void LoadByAssignedSeat(int seatID)
        {
            SqlDataReader reader = new SeatPledgeData().GetSeatPledgeByAssignedSeatID(seatID);


            while (reader.Read())
            {
                base.Add(new SeatPledge(reader));
            }
            reader.Close();
        }


        /// <summary>
        /// Populate the collection with all Seat objects that have not yet been assigned
        /// to a seat.
        /// </summary>
        public void LoadByUnassignedSeat()
        {
            SqlDataReader reader = new SeatPledgeData().GetSeatPledgeByUnassignedSeat();


            while (reader.Read())
            {
                base.Add(new SeatPledge(reader));
            }
            reader.Close();
        }
        
        #endregion
    }
}
