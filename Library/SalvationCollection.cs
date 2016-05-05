using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class SalvationCollection : ArenaCollectionBase<Salvation>
	{
		public SalvationCollection()
		{
		}

		public void LoadByPersonID(int personID)
		{
			SqlDataReader salvationByPersonID = (new SalvationData()).GetSalvationByPersonID(personID);
			while (salvationByPersonID.Read())
			{
				base.Add(new Salvation(salvationByPersonID));
			}
			salvationByPersonID.Close();
		}
	}
}