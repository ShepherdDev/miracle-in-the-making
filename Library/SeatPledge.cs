using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class SeatPledge : ArenaObjectBase
	{
		private DateTime _dateCreated;

		private DateTime _dateModified;

		private string _createdBy;

		private string _modifiedBy;

		private Arena.Core.Person _person;

		private int _personID;

		private Seat _assignedSeat;

		private int _assignedSeatID;

		private Seat _requestedSeat;

		private int _requestedSeatID;

		private Arena.Custom.SOTHC.MiTM.Dedication _dedication;

		public decimal Amount
		{
			get;
			set;
		}

		public Seat AssignedSeat
		{
			get
			{
				if (this._assignedSeat == null)
				{
					this._assignedSeat = new Seat(this._assignedSeatID);
				}
				return this._assignedSeat;
			}
			set
			{
				this._assignedSeat = value;
				this._assignedSeatID = (this._assignedSeat != null ? this._assignedSeat.SeatID : -1);
			}
		}

		public int AssignedSeatID
		{
			get
			{
				return this._assignedSeatID;
			}
			set
			{
				Seat seat;
				this._assignedSeatID = value;
				if (this._assignedSeatID != -1)
				{
					seat = new Seat(this._assignedSeatID);
				}
				else
				{
					seat = null;
				}
				this._assignedSeat = seat;
			}
		}

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

		public Arena.Custom.SOTHC.MiTM.Dedication Dedication
		{
			get
			{
				if (this._dedication == null)
				{
					SqlDataReader dedicationBySeatPledgeID = (new DedicationData()).GetDedicationBySeatPledgeID(this.SeatPledgeID);
					if (!dedicationBySeatPledgeID.Read())
					{
						this._dedication = new Arena.Custom.SOTHC.MiTM.Dedication();
						if (this.SeatPledgeID != -1)
						{
							this.Dedication.SeatPledgeID = this.SeatPledgeID;
						}
					}
					else
					{
						this._dedication = new Arena.Custom.SOTHC.MiTM.Dedication(dedicationBySeatPledgeID);
					}
					dedicationBySeatPledgeID.Close();
				}
				return this._dedication;
			}
		}

		public string ModifiedBy
		{
			get
			{
				return this._modifiedBy;
			}
		}

		public Arena.Core.Person Person
		{
			get
			{
				if (this._person == null)
				{
					this._person = new Arena.Core.Person(this._personID);
				}
				return this._person;
			}
			set
			{
				this._person = value;
				this._personID = (this._person != null ? this._person.get_PersonID() : -1);
			}
		}

		public int PersonID
		{
			get
			{
				return this._personID;
			}
			set
			{
				Arena.Core.Person person;
				this._personID = value;
				if (this._personID != -1)
				{
					person = new Arena.Core.Person(this._personID);
				}
				else
				{
					person = null;
				}
				this._person = person;
			}
		}

		public static int PledgeFundID
		{
			get
			{
				return 49;
			}
		}

		public int RequestedArmLeft
		{
			get;
			set;
		}

		public int RequestedArmRight
		{
			get;
			set;
		}

		public int RequestedBackRest
		{
			get;
			set;
		}

		public int RequestedFullSeat
		{
			get;
			set;
		}

		public int RequestedLeg1
		{
			get;
			set;
		}

		public int RequestedLeg2
		{
			get;
			set;
		}

		public int RequestedLeg3
		{
			get;
			set;
		}

		public int RequestedLeg4
		{
			get;
			set;
		}

		public Seat RequestedSeat
		{
			get
			{
				if (this._requestedSeat == null)
				{
					this._requestedSeat = new Seat(this._requestedSeatID);
				}
				return this._requestedSeat;
			}
			set
			{
				this._requestedSeat = value;
				this._requestedSeatID = (this._requestedSeat != null ? this._requestedSeat.SeatID : -1);
			}
		}

		public int RequestedSeatID
		{
			get
			{
				return this._requestedSeatID;
			}
			set
			{
				Seat seat;
				this._requestedSeatID = value;
				if (this._requestedSeatID != -1)
				{
					seat = new Seat(this._requestedSeatID);
				}
				else
				{
					seat = null;
				}
				this._requestedSeat = seat;
			}
		}

		public int SeatPledgeID
		{
			get;
			set;
		}

		public SeatPledge()
		{
			this.SeatPledgeID = -1;
			this._dateCreated = DateTime.Now;
			this._dateModified = DateTime.Now;
			this._createdBy = string.Empty;
			this._modifiedBy = string.Empty;
			this._personID = -1;
			this.Amount = new decimal(0);
			this._assignedSeatID = -1;
			this._requestedSeatID = -1;
			this.RequestedFullSeat = 0;
			this.RequestedBackRest = 0;
			this.RequestedLeg1 = 0;
			this.RequestedLeg2 = 0;
			this.RequestedLeg3 = 0;
			this.RequestedLeg4 = 0;
			this.RequestedArmLeft = 0;
			this.RequestedArmRight = 0;
		}

		public SeatPledge(int seatPledgeID) : this()
		{
			SqlDataReader seatPledgeByID = (new SeatPledgeData()).GetSeatPledgeByID(seatPledgeID);
			if (seatPledgeByID.Read())
			{
				this.LoadSeatPledge(seatPledgeByID);
			}
			seatPledgeByID.Close();
		}

		public SeatPledge(SqlDataReader reader) : this()
		{
			this.LoadSeatPledge(reader);
		}

		public void Delete()
		{
			SeatPledge.Delete(this.SeatPledgeID);
			this.SeatPledgeID = -1;
		}

		public static void Delete(int seatPledgeID)
		{
			(new SeatPledgeData()).DeleteSeatPledge(seatPledgeID);
		}

		private void LoadSeatPledge(SqlDataReader reader)
		{
			this.SeatPledgeID = (int)reader["seat_pledge_id"];
			this._dateCreated = (DateTime)reader["date_created"];
			this._dateModified = (DateTime)reader["date_modified"];
			this._createdBy = reader["created_by"].ToString();
			this._modifiedBy = reader["modified_by"].ToString();
			this._personID = (int)reader["person_id"];
			this.Amount = (decimal)reader["amount"];
			this._assignedSeatID = (!reader.IsDBNull(reader.GetOrdinal("assigned_seat_id")) ? (int)reader["assigned_seat_id"] : -1);
			this._requestedSeatID = (!reader.IsDBNull(reader.GetOrdinal("requested_seat_id")) ? (int)reader["requested_seat_id"] : -1);
			this.RequestedFullSeat = (int)reader["requested_full_seat"];
			this.RequestedBackRest = (int)reader["requested_back_rest"];
			this.RequestedLeg1 = (int)reader["requested_leg1"];
			this.RequestedLeg2 = (int)reader["requested_leg2"];
			this.RequestedLeg3 = (int)reader["requested_leg3"];
			this.RequestedLeg4 = (int)reader["requested_leg4"];
			this.RequestedArmLeft = (int)reader["requested_arm_left"];
			this.RequestedArmRight = (int)reader["requested_arm_right"];
		}

		public void Save(string userID)
		{
			int? nullable;
			int? nullable1;
			int? nullable2;
			SeatPledgeData seatPledgeDatum = new SeatPledgeData();
			int seatPledgeID = this.SeatPledgeID;
			int num = this._personID;
			decimal amount = this.Amount;
			if (this._assignedSeatID != -1)
			{
				nullable1 = new int?(this._assignedSeatID);
			}
			else
			{
				nullable = null;
				nullable1 = nullable;
			}
			if (this._requestedSeatID != -1)
			{
				nullable2 = new int?(this._requestedSeatID);
			}
			else
			{
				nullable = null;
				nullable2 = nullable;
			}
			this.SeatPledgeID = seatPledgeDatum.SaveSeatPledge(seatPledgeID, num, amount, nullable1, nullable2, this.RequestedFullSeat, this.RequestedBackRest, this.RequestedLeg1, this.RequestedLeg2, this.RequestedLeg3, this.RequestedLeg4, this.RequestedArmLeft, this.RequestedArmRight, userID);
			if (this.Dedication.SeatPledgeID == -1)
			{
				this.Dedication.SeatPledgeID = this.SeatPledgeID;
			}
		}
	}
}