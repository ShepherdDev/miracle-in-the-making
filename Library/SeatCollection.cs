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
    public class SeatCollection : ArenaCollectionBase<Seat>
    {
        #region Constructors

        /// <summary>
        /// Create an empty collection of Seat objects.
        /// </summary>
        public SeatCollection()
        {
        }


        /// <summary>
        /// Create a collection of Seat objects and fill it with all seats for the given section.
        /// </summary>
        /// <param name="section">The letter of the section to load seats for.</param>
        public SeatCollection(String section)
        {
            SeatData data = new SeatData();
            SqlDataReader reader;


            reader = data.GetSeatBySection(section);
            while (reader.Read())
            {
                base.Add(new Seat(reader));
            }
            reader.Close();
        }

        #endregion


        #region Load Methods

        #endregion
    }
}
