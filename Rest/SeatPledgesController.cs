using System.Linq;
using System.Net;
using System.Web.Http;

using com.shepherdchurch.MiracleInTheMaking.Model;

using Rock.Rest;
using Rock.Rest.Filters;

namespace com.shepherdchurch.MiracleInTheMaking.Rest
{
    /// <summary>
    /// REST API for SeatPledges
    /// </summary>
    public class SeatPledgesController : ApiController<SeatPledge>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatPledgesController"/> class.
        /// </summary>
        public SeatPledgesController() : base(new SeatPledgeService(new Data.MiracleInTheMakingContext())) { }
    }
}
