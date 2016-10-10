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
    public class SalvationsController : ApiController<Salvation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SalvationsController"/> class.
        /// </summary>
        public SalvationsController() : base(new SalvationService(new Data.MiracleInTheMakingContext())) { }
    }
}
