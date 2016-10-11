using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;

using com.shepherdchurch.MiracleInTheMaking.Data;

using Rock.Data;
using Rock.Model;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// A SeatPledge/prayer story as shared by the person.
    /// </summary>
    [Table("_com_shepherdchurch_MiracleInTheMaking_SeatPledge")]
    [DataContract]
    public class SeatPledge : Rock.Data.Model<SeatPledge>, Rock.Security.ISecured
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the alias of the person who owns this seat pledge.
        /// </summary>
        /// <value>
        /// The first name
        /// </value>
        [Required(ErrorMessage = "PledgedPersonAliasId is required")]
        [DataMember(IsRequired = true)]
        public int PledgedPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the amount of this pledge.
        /// </summary>
        /// <value>
        /// The amount
        /// </value>
        [Required(ErrorMessage = "Amount is required")]
        [DataMember(IsRequired = true)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the Seat that this pledge was assigned to.
        /// </summary>
        /// <value>
        /// The assigned seat
        /// </value>
        [DataMember]
        public int? AssignedSeatId { get; set; }

        /// <summary>
        /// Gets or sets the Seat that was requested for this pledge.
        /// </summary>
        /// <value>
        /// The seat requested
        /// </value>
        [DataMember]
        public int? RequestedSeatId { get; set; }

        /// <summary>
        /// Gets or sets the number of full seats pledged.
        /// </summary>
        /// <value>
        /// The number of full seats
        /// </value>
        [Required(ErrorMessage = "RequestedFullSeat is required")]
        [DataMember(IsRequired = true)]
        public int RequestedFullSeat { get; set; }

        /// <summary>
        /// Gets or sets the number of back rests pledged.
        /// </summary>
        /// <value>
        /// The number of back rests
        /// </value>
        [Required(ErrorMessage = "RequestedBackRest is required")]
        [DataMember(IsRequired = true)]
        public int RequestedBackRest { get; set; }

        /// <summary>
        /// Gets or sets the number of leg1's pledged.
        /// </summary>
        /// <value>
        /// The number of leg1's
        /// </value>
        [Required(ErrorMessage = "RequestedLeg1 is required")]
        [DataMember(IsRequired = true)]
        public int RequestedLeg1 { get; set; }

        /// <summary>
        /// Gets or sets the number of leg2's pledged.
        /// </summary>
        /// <value>
        /// The number of leg2's
        /// </value>
        [Required(ErrorMessage = "RequestedLeg2 is required")]
        [DataMember(IsRequired = true)]
        public int RequestedLeg2 { get; set; }

        /// <summary>
        /// Gets or sets the number of leg3's pledged.
        /// </summary>
        /// <value>
        /// The number of leg3's
        /// </value>
        [Required(ErrorMessage = "RequestedLeg3 is required")]
        [DataMember(IsRequired = true)]
        public int RequestedLeg3 { get; set; }

        /// <summary>
        /// Gets or sets the number of leg4's pledged.
        /// </summary>
        /// <value>
        /// The number of leg4's
        /// </value>
        [Required(ErrorMessage = "RequestedLeg4 is required")]
        [DataMember(IsRequired = true)]
        public int RequestedLeg4 { get; set; }

        /// <summary>
        /// Gets or sets the number of left arms pledged.
        /// </summary>
        /// <value>
        /// The number of left arms
        /// </value>
        [Required(ErrorMessage = "RequestedArmLeft is required")]
        [DataMember(IsRequired = true)]
        public int RequestedArmLeft { get; set; }

        /// <summary>
        /// Gets or sets the number of right arms pledged.
        /// </summary>
        /// <value>
        /// The number of right arms
        /// </value>
        [Required(ErrorMessage = "RequestedRightArm is required")]
        [DataMember(IsRequired = true)]
        public int RequestedRightArm { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the person alias this Seat Pledge belongs to.
        /// </summary>
        /// <value>
        /// The person alias
        /// </value>
        public virtual PersonAlias PledgedPersonAlias { get; set; }

        /// <summary>
        /// Gets or sets the Seat assigned to this Seat Pledge.
        /// </summary>
        /// <value>
        /// The seat
        /// </value>
        public virtual Seat AssignedSeat { get; set; }

        /// <summary>
        /// Gets or sets the Seat requested for this Seat Pledge.
        /// </summary>
        /// <value>
        /// The seat
        /// </value>
        public virtual Seat RequestedSeat { get; set; }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class SeatPledgeConfiguration : EntityTypeConfiguration<SeatPledge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatPledgeConfiguration"/> class.
        /// </summary>
        public SeatPledgeConfiguration()
        {
            this.HasRequired(r => r.PledgedPersonAlias).WithMany().HasForeignKey(p => p.PledgedPersonAliasId).WillCascadeOnDelete(true);
            this.HasOptional(r => r.AssignedSeat).WithMany().HasForeignKey(p => p.AssignedSeatId).WillCascadeOnDelete(false);
            this.HasOptional(r => r.RequestedSeat).WithMany().HasForeignKey(p => p.RequestedSeatId).WillCascadeOnDelete(false);
        }
    }

    #endregion

}
