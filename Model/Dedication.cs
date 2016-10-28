using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;

using com.shepherdchurch.MiracleInTheMaking.Data;

using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Security;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// A dedication of a seat pledge to another person.
    /// </summary>
    [Table("_com_shepherdchurch_MiracleInTheMaking_Dedication")]
    [DataContract]
    public class Dedication : Rock.Data.Model<Dedication>, ISecured
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the SeatPledge Id for this dedication.
        /// </summary>
        /// <value>
        /// The seat pledge identifier
        /// </value>
        [Required(ErrorMessage = "SeatPledgeId is required")]
        [DataMember(IsRequired = true)]
        public int SeatPledgeId { get; set; }

        /// <summary>
        /// Gets or sets the login name of the person who approved this dedication for display.
        /// </summary>
        /// <value>
        /// The login name
        /// </value>
        [MaxLength(100)]
        [DataMember]
        public string ApprovedBy { get; set; }

        /// <summary>
        /// Gets or sets the name of the person being dedicated to.
        /// </summary>
        /// <value>
        /// The name being dedicated to
        /// </value>
        [MaxLength(100)]
        [DataMember]
        public string DedicatedTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the person who sponsored this dedication.
        /// </summary>
        /// <value>
        /// The name of the sponsor
        /// </value>
        [MaxLength(100)]
        [DataMember]
        public string SponsoredBy { get; set; }

        /// <summary>
        /// Gets or sets the anonymous flag.
        /// </summary>
        /// <value>
        /// The anonymity flag
        /// </value>
        [DataMember]
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Gets or sets the biography to display with this dedication.
        /// </summary>
        /// <value>
        /// The biography text
        /// </value>
        [DataMember]
        public string Biography { get; set; }

        /// <summary>
        /// Gets or sets the binary file to display with this dedication.
        /// </summary>
        /// <value>
        /// The binary file idetifier
        /// </value>
        [DataMember]
        public int? BinaryFileId { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the seat pledge this Dedication belongs to.
        /// </summary>
        /// <value>
        /// The seat pledge
        /// </value>
        public virtual SeatPledge SeatPledge { get; set; }

        /// <summary>
        /// Gets or sets the BinaryFile associated with this Dedication.
        /// </summary>
        /// <value>
        /// The BinaryFile
        /// </value>
        public virtual BinaryFile BinaryFile { get; set; }

        #endregion

        #region ISecured Methods

        /// <summary>
        /// Gets the parent authority.
        /// </summary>
        /// <value>
        /// The parent authority.
        /// </value>
        public override ISecured ParentAuthority
        {
            get
            {
                return this.SeatPledge != null ? this.SeatPledge : base.ParentAuthority;
            }
        }

        /// <summary>
        /// Gets the supported actions.
        /// </summary>
        /// <value>
        /// The supported actions.
        /// </value>
        [NotMapped]
        public override Dictionary<string, string> SupportedActions
        {
            get
            {
                var supportedActions = base.SupportedActions;

                supportedActions.AddOrReplace(Authorization.APPROVE, "The roles and/or users that have access to approve.");

                return supportedActions;
            }
        }

        #endregion
    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class DedicationConfiguration : EntityTypeConfiguration<Dedication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DedicationConfiguration"/> class.
        /// </summary>
        public DedicationConfiguration()
        {
            this.HasRequired(r => r.SeatPledge).WithMany(sp => sp.Dedications).HasForeignKey(p => p.SeatPledgeId).WillCascadeOnDelete(true);
            this.HasOptional(r => r.BinaryFile).WithMany().HasForeignKey(p => p.BinaryFileId).WillCascadeOnDelete(false);
        }
    }

    #endregion
}
