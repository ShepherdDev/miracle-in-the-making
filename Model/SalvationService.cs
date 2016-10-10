using com.shepherdchurch.MiracleInTheMaking.Data;
using Rock.Model;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SalvationService : MiracleInTheMakingService<Salvation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalvationService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SalvationService(MiracleInTheMakingContext context) : base(context) { }

    }
}
