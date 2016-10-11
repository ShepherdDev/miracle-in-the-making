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
    /// A Salvation/prayer story as shared by the person.
    /// </summary>
    [Table("_com_shepherdchurch_MiracleInTheMaking_Salvation")]
    [DataContract]
    public class Salvation : Rock.Data.Model<Salvation>, Rock.Security.ISecured
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the alias of the person who owns this salvation.
        /// </summary>
        /// <value>
        /// The first name
        /// </value>
        [Required(ErrorMessage = "PersonAliasId is required")]
        [DataMember(IsRequired = true)]
        public int PersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the person being prayed for.
        /// </summary>
        /// <value>
        /// The first name
        /// </value>
        [MaxLength(100)]
        [Required(ErrorMessage = "FirstName is required")]
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the person being prayed for.
        /// </summary>
        /// <value>
        /// The last name
        /// </value>
        [MaxLength(100)]
        [Required(ErrorMessage = "LastName is required")]
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the saved status of this salvation prayer.
        /// </summary>
        /// <value>
        /// The status
        /// </value>
        [Required(ErrorMessage = "IsSaved is required")]
        [DataMember(IsRequired = true)]
        public bool IsSaved{ get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the person alias this salvation belongs to.
        /// </summary>
        /// <value>
        /// The person alias.
        /// </value>
        public virtual PersonAlias PersonAlias { get; set; }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// 
    /// </summary>
    public partial class SalvationConfiguration : EntityTypeConfiguration<Salvation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalvationConfiguration"/> class.
        /// </summary>
        public SalvationConfiguration()
        {
            this.HasRequired(r => r.PersonAlias).WithMany().HasForeignKey(p => p.PersonAliasId).WillCascadeOnDelete(true);
        }
    }

    #endregion

}
