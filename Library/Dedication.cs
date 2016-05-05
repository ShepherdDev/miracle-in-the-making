using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using Arena.Utility;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class Dedication : ArenaObjectBase
	{
		private DateTime _dateCreated;

		private DateTime _dateModified;

		private string _createdBy;

		private string _modifiedBy;

		private SeatPledge _seatPledge;

		private int _seatPledgeID;

		private ArenaImage _blob;

		private int _blobID;

		public bool Anonymous
		{
			get;
			set;
		}

		public string ApprovedBy
		{
			get;
			set;
		}

		public string Biography
		{
			get;
			set;
		}

		public ArenaImage Blob
		{
			get
			{
				if (this._blob == null)
				{
					this._blob = new ArenaImage(this._blobID);
				}
				return this._blob;
			}
			set
			{
				this._blob = value;
				this._blobID = (this._blob != null ? this._blob.get_BlobID() : -1);
			}
		}

		public int BlobID
		{
			get
			{
				return this._blobID;
			}
			set
			{
				ArenaImage arenaImage;
				this._blobID = value;
				if (this._blobID != -1)
				{
					arenaImage = new ArenaImage(this._blobID);
				}
				else
				{
					arenaImage = null;
				}
				this._blob = arenaImage;
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

		public string DedicatedTo
		{
			get;
			set;
		}

		public int DedicationID
		{
			get;
			set;
		}

		public string ModifiedBy
		{
			get
			{
				return this._modifiedBy;
			}
		}

		public int SeatPledgeID
		{
			get
			{
				return this._seatPledgeID;
			}
			set
			{
				SeatPledge seatPledge;
				this._seatPledgeID = value;
				if (this._seatPledgeID != -1)
				{
					seatPledge = new SeatPledge(this._seatPledgeID);
				}
				else
				{
					seatPledge = null;
				}
				this._seatPledge = seatPledge;
			}
		}

		public string SponsoredBy
		{
			get;
			set;
		}

		public Dedication()
		{
			this.DedicationID = -1;
			this._dateCreated = DateTime.Now;
			this._dateModified = DateTime.Now;
			this._createdBy = string.Empty;
			this._modifiedBy = string.Empty;
			this.ApprovedBy = string.Empty;
			this._seatPledgeID = -1;
			this.DedicatedTo = string.Empty;
			this.SponsoredBy = string.Empty;
			this.Biography = string.Empty;
			this.Anonymous = false;
			this._blobID = -1;
		}

		public Dedication(int dedicationID) : this()
		{
			SqlDataReader dedicationByID = (new DedicationData()).GetDedicationByID(dedicationID);
			if (dedicationByID.Read())
			{
				this.LoadDedication(dedicationByID);
			}
			dedicationByID.Close();
		}

		public Dedication(SqlDataReader reader) : this()
		{
			this.LoadDedication(reader);
		}

		public static Dedication DedicationFromSeatPledgeID(int seatPledgeID)
		{
			SqlDataReader dedicationBySeatPledgeID = (new DedicationData()).GetDedicationBySeatPledgeID(seatPledgeID);
			Dedication dedication = null;
			if (dedicationBySeatPledgeID.Read())
			{
				dedication = new Dedication(dedicationBySeatPledgeID);
			}
			dedicationBySeatPledgeID.Close();
			return dedication;
		}

		public void Delete()
		{
			Dedication.Delete(this.DedicationID);
			this.DedicationID = -1;
		}

		public static void Delete(int dedicationID)
		{
			(new DedicationData()).DeleteDedication(dedicationID);
		}

		private void LoadDedication(SqlDataReader reader)
		{
			this.DedicationID = (int)reader["dedication_id"];
			this._dateCreated = (DateTime)reader["date_created"];
			this._dateModified = (DateTime)reader["date_modified"];
			this._createdBy = reader["created_by"].ToString();
			this._modifiedBy = reader["modified_by"].ToString();
			this.ApprovedBy = reader["approved_by"].ToString();
			this._seatPledgeID = (int)reader["seat_pledge_id"];
			this.DedicatedTo = reader["dedicated_to"].ToString();
			this.SponsoredBy = reader["sponsored_by"].ToString();
			this.Biography = reader["biography"].ToString();
			this.Anonymous = (bool)reader["anonymous"];
			this._blobID = (!reader.IsDBNull(reader.GetOrdinal("blob_id")) ? (int)reader["blob_id"] : -1);
		}

		public void Save(string userID)
		{
			int? nullable;
			DedicationData dedicationDatum = new DedicationData();
			int dedicationID = this.DedicationID;
			string approvedBy = this.ApprovedBy;
			int num = this._seatPledgeID;
			string dedicatedTo = this.DedicatedTo;
			string sponsoredBy = this.SponsoredBy;
			string biography = this.Biography;
			bool anonymous = this.Anonymous;
			if (this._blobID != -1)
			{
				nullable = new int?(this._blobID);
			}
			else
			{
				nullable = null;
			}
			dedicationDatum.SaveDedication(dedicationID, approvedBy, num, dedicatedTo, sponsoredBy, biography, anonymous, nullable, userID);
		}
	}
}