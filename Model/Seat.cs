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
    /// A Seat as it exists in the building.
    /// </summary>
    [Table("_com_shepherdchurch_MiracleInTheMaking_Seat")]
    [DataContract]
    public class Seat : Rock.Data.Model<Seat>, Rock.Security.ISecured
    {

        #region Entity Properties

        /// <summary>
        /// Gets or sets the section.
        /// </summary>
        /// <value>
        /// The section letter.
        /// </value>
        [MaxLength(1)]
        [Required(ErrorMessage = "Section is required")]
        [DataMember(IsRequired = true)]
        public string Section { get; set; }

        /// <summary>
        /// Gets or sets the seat number.
        /// </summary>
        /// <value>
        /// The seat number.
        /// </value>
        [Required(ErrorMessage = "SeatNumber is required")]
        [DataMember(IsRequired = true)]
        public int SeatNumber { get; set; }

        #endregion

        #region Virtual Properties

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class SeatConfiguration : EntityTypeConfiguration<Seat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatConfiguration"/> class.
        /// </summary>
        public SeatConfiguration()
        {
        }
    }

    #endregion

}
