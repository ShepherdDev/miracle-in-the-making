using com.shepherdchurch.MiracleInTheMaking.Data;
using System.Linq;
using Rock.Model;
using System.Collections.Generic;

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

        /// <summary>
        /// Returns a collection of <see cref="Salvation">Salvations</see> that are owned by
        /// the specified <see cref="Person"/> Id.
        /// </summary>
        /// <param name="personId">The identifier of the <see cref="Person"/> whose salvation requests are to be retrieved.</param>
        /// <returns>An enumerable collection of <see cref="Salvation">Salvations</see> that are linked to the requested personId.</returns>
        public IEnumerable<Salvation> GetByPersonId( int personId )
        {
            return Queryable()
                .Where( s => s.PersonAlias.PersonId == personId );
        }
    }
}
