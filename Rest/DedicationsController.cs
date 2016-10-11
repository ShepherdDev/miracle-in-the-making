using System.Linq;
using System.Net;
using System.Web.Http;

using com.shepherdchurch.MiracleInTheMaking.Model;

using Rock.Rest;
using Rock.Rest.Filters;

namespace com.shepherdchurch.MiracleInTheMaking.Rest
{
    /// <summary>
    /// REST API for Salvations
    /// </summary>
    public class DedicationsController : ApiController<Dedication>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DedicationsController"/> class.
        /// </summary>
        public DedicationsController() : base(new DedicationService(new Data.MiracleInTheMakingContext())) { }
    }
}
