using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class SeatCollection : ArenaCollectionBase<Seat>
	{
		public SeatCollection()
		{
		}

		public SeatCollection(string section)
		{
			SqlDataReader seatBySection = (new SeatData()).GetSeatBySection(section);
			while (seatBySection.Read())
			{
				base.Add(new Seat(seatBySection));
			}
			seatBySection.Close();
		}
	}
}