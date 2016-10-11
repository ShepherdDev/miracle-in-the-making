using com.shepherdchurch.MiracleInTheMaking.Data;
using Rock.Model;

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

    }
}
