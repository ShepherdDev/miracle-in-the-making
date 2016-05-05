using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class Seat : ArenaObjectBase
	{
		private DateTime _dateCreated;

		private DateTime _dateModified;

		private string _createdBy;

		private string _modifiedBy;

		public string CreatedBy
		{
			get
			{
				return this._createdBy;
			}
		}

		public DateTime DateCreated
		{
			get
			{
				return this._dateCreated;
			}
		}

		public DateTime DateModified
		{
			get
			{
				return this._dateModified;
			}
		}

		public string ModifiedBy
		{
			get
			{
				return this._modifiedBy;
			}
		}

		public int Number
		{
			get;
			set;
		}

		public int SeatID
		{
			get;
			set;
		}

		public string Section
		{
			get;
			set;
		}

		public Seat()
		{
			this.SeatID = -1;
			this._dateCreated = DateTime.Now;
			this._dateModified = DateTime.Now;
			this._createdBy = string.Empty;
			this._modifiedBy = string.Empty;
			this.Section = string.Empty;
			this.Number = -1;
		}

		public Seat(int seatID) : this()
		{
			SqlDataReader seatByID = (new SeatData()).GetSeatByID(seatID);
			if (seatByID.Read())
			{
				this.LoadSeat(seatByID);
			}
			seatByID.Close();
		}

		public Seat(string section, int seatNumber) : this()
		{
			SqlDataReader seatBySectionAndNumber = (new SeatData()).GetSeatBySectionAndNumber(section, seatNumber);
			if (seatBySectionAndNumber.Read())
			{
				this.LoadSeat(seatBySectionAndNumber);
			}
			seatBySectionAndNumber.Close();
		}

		public Seat(SqlDataReader reader) : this()
		{
			this.LoadSeat(reader);
		}

		public void Delete()
		{
			Seat.Delete(this.SeatID);
			this.SeatID = -1;
		}

		public static void Delete(int seatID)
		{
			(new SeatData()).DeleteSeat(seatID);
		}

		private void LoadSeat(SqlDataReader reader)
		{
			this.SeatID = (int)reader["seat_id"];
			this._dateCreated = (DateTime)reader["date_created"];
			this._dateModified = (DateTime)reader["date_modified"];
			this._createdBy = reader["created_by"].ToString();
			this._modifiedBy = reader["modified_by"].ToString();
			this.Section = reader["section"].ToString();
			this.Number = (int)reader["seat_number"];
		}

		public static Seat NextAvailableSeat(decimal amount, int ignoreSeatPledgeID)
		{
			SqlDataReader seatByNextAvailable = (new SeatData()).GetSeatByNextAvailable(amount, ignoreSeatPledgeID);
			Seat seat = null;
			if (seatByNextAvailable.Read())
			{
				seat = new Seat(seatByNextAvailable);
			}
			seatByNextAvailable.Close();
			return seat;
		}

		public void Save(string userID)
		{
			(new SeatData()).SaveSeat(this.SeatID, this.Section, this.Number, userID);
		}
	}
}