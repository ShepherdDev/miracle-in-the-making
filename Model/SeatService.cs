using com.shepherdchurch.MiracleInTheMaking.Data;
using Rock.Model;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SeatService : MiracleInTheMakingService<Seat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiracleInTheMakingService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SeatService(MiracleInTheMakingContext context) : base(context) { }

    }
}
