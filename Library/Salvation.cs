using Arena.Core;
using Arena.Custom.SOTHC.MiTM.DataLayer;
using System;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace Arena.Custom.SOTHC.MiTM
{
	[Serializable]
	public class Salvation : ArenaObjectBase
	{
		private DateTime _dateCreated;

		private DateTime _dateModified;

		private string _createdBy;

		private string _modifiedBy;

		private Arena.Core.Person _person;

		private int _personID;

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

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
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

		public int SalvationID
		{
			get;
			set;
		}

		public bool Status
		{
			get;
			set;
		}

		public Salvation()
		{
			this.SalvationID = -1;
			this._dateCreated = DateTime.Now;
			this._dateModified = DateTime.Now;
			this._createdBy = string.Empty;
			this._modifiedBy = string.Empty;
			this._personID = -1;
			this.FirstName = string.Empty;
			this.LastName = string.Empty;
			this.Status = false;
		}

		public Salvation(int salvationID) : this()
		{
			SqlDataReader salvationByID = (new SalvationData()).GetSalvationByID(salvationID);
			if (salvationByID.Read())
			{
				this.LoadSalvation(salvationByID);
			}
			salvationByID.Close();
		}

		public Salvation(SqlDataReader reader) : this()
		{
			this.LoadSalvation(reader);
		}

		public void Delete()
		{
			Salvation.Delete(this.SalvationID);
			this.SalvationID = -1;
		}

		public static void Delete(int salvationID)
		{
			(new SalvationData()).DeleteSalvation(salvationID);
		}

		private void LoadSalvation(SqlDataReader reader)
		{
			this.SalvationID = (int)reader["salvation_id"];
			this._dateCreated = (DateTime)reader["date_created"];
			this._dateModified = (DateTime)reader["date_modified"];
			this._createdBy = reader["created_by"].ToString();
			this._modifiedBy = reader["modified_by"].ToString();
			this._personID = (int)reader["person_id"];
			this.FirstName = reader["first_name"].ToString();
			this.LastName = reader["last_name"].ToString();
			this.Status = (bool)reader["status"];
		}

		public void Save(string userID)
		{
			(new SalvationData()).SaveSalvation(this.SalvationID, this._personID, this.FirstName, this.LastName, this.Status, userID);
		}
	}
}