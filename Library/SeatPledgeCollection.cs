using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class SeatPledgeCollection : ArenaCollectionBase<SeatPledge>
	{
		public SeatPledgeCollection()
		{
		}

		public void LoadByAssignedSeat(int seatID)
		{
			SqlDataReader seatPledgeByAssignedSeatID = (new SeatPledgeData()).GetSeatPledgeByAssignedSeatID(seatID);
			while (seatPledgeByAssignedSeatID.Read())
			{
				base.Add(new SeatPledge(seatPledgeByAssignedSeatID));
			}
			seatPledgeByAssignedSeatID.Close();
		}

		public void LoadByPerson(int personID)
		{
			SqlDataReader seatPledgeByPersonID = (new SeatPledgeData()).GetSeatPledgeByPersonID(personID);
			while (seatPledgeByPersonID.Read())
			{
				base.Add(new SeatPledge(seatPledgeByPersonID));
			}
			seatPledgeByPersonID.Close();
		}

		public void LoadByUnassignedSeat()
		{
			SqlDataReader seatPledgeByUnassignedSeat = (new SeatPledgeData()).GetSeatPledgeByUnassignedSeat();
			while (seatPledgeByUnassignedSeat.Read())
			{
				base.Add(new SeatPledge(seatPledgeByUnassignedSeat));
			}
			seatPledgeByUnassignedSeat.Close();
		}
	}
}