using System.Collections.Generic;
using System.Linq;
using Rock.Model;
using com.shepherdchurch.MiracleInTheMaking.Data;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class DedicationService : MiracleInTheMakingService<Dedication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DedicationService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public DedicationService(MiracleInTheMakingContext context) : base(context) { }

        /// <summary>
        /// Returns a single <see cref="Dedication"/> for the given seatPledgeId.
        /// </summary>
        /// <param name="seatPledgeId"></param>
        /// <returns>A single <see cref="Dedication"/> that is related to the seatPledgeId or null if none found.</returns>
        public Dedication GetBySeatPledgeId(int seatPledgeId)
        {
            return Queryable()
                .Where( d => d.SeatPledgeId == seatPledgeId )
                .FirstOrDefault();
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="cref=Dedication"/> whose approved state matches
        /// the one specified.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="cref=Dedication"/> for the specified approved state.</returns>
        public IEnumerable<Dedication> GetApproved(bool isApproved)
        {
            if ( isApproved )
            {
                return Queryable()
                    .Where( d => d.ApprovedBy.Length > 0 );
            }
            else
            {
                return Queryable()
                    .Where( d => d.ApprovedBy.Length == 0 );
            }
        }
    }
}
