using System.Collections.Generic;
using System.Linq;
using com.shepherdchurch.MiracleInTheMaking.Data;
using Rock.Model;
using Rock.Data;

namespace com.shepherdchurch.MiracleInTheMaking.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SeatService : MiracleInTheMakingService<Seat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SeatService(MiracleInTheMakingContext context) : base(context) { }

        /// <summary>
        /// Returns the next available <see cref="Seat"/> that can be assigned to the specified
        /// seat pledge for the amount requested.
        /// </summary>
        /// <param name="amountNeeded">The amount the seat pledge is (or will be) for. The returned <see cref="Seat"/> will have at least this much headroom in it's pledging.</param>
        /// <param name="seatPledgeId">The ID of the <see cref="SeatPledge"/> that needs a <see cref="Seat"/> assigned to it.</param>
        /// <returns>A <see cref="Seat"/> that may be assigned or null if no <see cref="Seat">Seats</see> were available.</returns>
        public Seat GetNextAvailableForSeatPledge( decimal amountNeeded, int seatPledgeId )
        {
            var qrySeatPledges = new SeatPledgeService( this.Context as MiracleInTheMakingContext )
                .Queryable()
                .Where( sp => sp.Id != seatPledgeId );

            var result = Queryable()
                .GroupJoin( qrySeatPledges, s => s.Id, sp => sp.AssignedSeatId, ( s, sp ) => new { Seat = s, Amount = (sp.Sum( x => (decimal?)x.Amount ) ?? 0) + amountNeeded } )
                .Where( o => o.Amount <= 10000 )
                .OrderBy( o => o.Seat.Section )
                .ThenBy( o => o.Seat.SeatNumber )
                .FirstOrDefault();

            return ( result != null ? result.Seat : null );
        }

        /// <summary>
        /// Returns an enumerable collection of all <see cref="Seat">Seats</see> in the given section.
        /// </summary>
        /// <param name="section">The one character section the seats must be located in.</param>
        /// <returns>An enumerable collection of all <see cref="Seat">Seats</see> in the given section.</returns>
        public IEnumerable<Seat> GetBySection( string section )
        {
            return Queryable()
                .Where( s => s.Section == section );
        }

        /// <summary>
        /// Returns the single <see cref="Seat"/> that is located in the specified section and at the seat number. 
        /// </summary>
        /// <param name="section">The one character section this seat is located in.</param>
        /// <param name="seatNumber">The seat number within the section.</param>
        /// <returns>A <see cref="Seat"/> which is located at the specified location or null if one was not found.</returns>
        public Seat GetBySectionAndNumber( string section, int seatNumber )
        {
            return Queryable()
                .Where( s => s.Section == section && s.SeatNumber == seatNumber )
                .FirstOrDefault();
        }
    }
}
