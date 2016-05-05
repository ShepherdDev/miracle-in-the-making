using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class DedicationCollection : ArenaCollectionBase<Dedication>
	{
		public DedicationCollection()
		{
		}

		public void LoadUnapproved()
		{
			SqlDataReader dedicationByUnapproved = (new DedicationData()).GetDedicationByUnapproved();
			while (dedicationByUnapproved.Read())
			{
				base.Add(new Dedication(dedicationByUnapproved));
			}
			dedicationByUnapproved.Close();
		}
	}
}