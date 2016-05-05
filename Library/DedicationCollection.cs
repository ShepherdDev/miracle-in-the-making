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
    public class DedicationCollection : ArenaCollectionBase<Dedication>
    {
        #region Constructors

        /// <summary>
        /// Create an empty collection.
        /// </summary>
        public DedicationCollection()
        {
        }

        #endregion

        #region Load Methods

        /// <summary>
        /// Populate the collection with all unapproved dedication requests.
        /// </summary>
        public void LoadUnapproved()
        {
            SqlDataReader reader = new DedicationData().GetDedicationByUnapproved();


            while (reader.Read())
            {
                base.Add(new Dedication(reader));
            }
            reader.Close();
        }


        #endregion
    }
}
