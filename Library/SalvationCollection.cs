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
    public class SalvationCollection : ArenaCollectionBase<Salvation>
    {
        #region Constructors

        /// <summary>
        /// Create an empty collection.
        /// </summary>
        public SalvationCollection()
        {
        }

        #endregion

        #region Load Methods

        /// <summary>
        /// Populate the collection with the salvation items for the given personID.
        /// </summary>
        /// <param name="personID">The ID number of the Person to load the salvation records of.</param>
        public void LoadByPersonID(int personID)
        {
            SqlDataReader reader = new SalvationData().GetSalvationByPersonID(personID);


            while (reader.Read())
            {
                base.Add(new Salvation(reader));
            }
            reader.Close();
        }


        #endregion
    }
}
