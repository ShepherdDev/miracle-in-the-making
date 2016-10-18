using System.Collections.Generic;
using System.Linq;
using com.shepherdchurch.MiracleInTheMaking.Data;
using Rock.Model;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SeatPledgeService : MiracleInTheMakingService<SeatPledge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatPledgeService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SeatPledgeService(MiracleInTheMakingContext context) : base(context) { }

        /// <summary>
        /// Returns an enumerable collection of <see cref="SeatPledge">SeatPledges</see> that have been
        /// assigned to the specified Seat identifier.
        /// </summary>
        /// <param name="assignedSeatId">The seat the <see cref="SeatPledge"/> must be assignd to.</param>
        /// <returns>An enumerable collection of <see cref="SeatPledge">SeatPledges</see>.</returns>
        public IEnumerable<SeatPledge> GetByAssignedSeatId( int assignedSeatId )
        {
            return Queryable()
                .Where( sp => sp.AssignedSeatId == assignedSeatId );
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="SeatPledge">SeatPledges</see> that were pledged by
        /// the specified person ID.
        /// </summary>
        /// <param name="personId">The ID of the <see cref="Person"/> who pledged them.</param>
        /// <returns>An enumerable collection of <see cref="SeatPledge">SeatPledges</see>.</returns>
        public IEnumerable<SeatPledge> GetByPersonId( int personId )
        {
            return Queryable()
                .Where( sp => sp.PledgedPersonAlias.PersonId == personId );
        }

        /// <summary>
        /// Returns an enumerable collection of <see cref="SeatPledge">SeatPledges</see> by wether or
        /// not they have an assigned seat already.
        /// </summary>
        /// <param name="isAssigned">If true, return a collection of <see cref="SeatPledge">SeatPledges</see> that have an assigned seat, otherwise a collection that has no assigned seat yet.</param>
        /// <returns>An enumerable collection of <see cref="SeatPledge">SeatPledges</see>.</returns>
        public IEnumerable<SeatPledge> GetBySeatIsAssigned( bool isAssigned )
        {
            return Queryable()
                .Where( sp => !sp.AssignedSeatId.HasValue );
        }
    }
}
