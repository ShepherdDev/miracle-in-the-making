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

    }
}
