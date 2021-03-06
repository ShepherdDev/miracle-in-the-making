﻿using System.Linq;
using System.Net;
using System.Web.Http;

using com.shepherdchurch.MiracleInTheMaking.Model;

using Rock.Rest;
using Rock.Rest.Filters;

namespace com.shepherdchurch.MiracleInTheMaking.Rest
{
    /// <summary>
    /// REST API for Seats
    /// </summary>
    public class SeatsController : ApiController<Seat>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeatsController"/> class.
        /// </summary>
        public SeatsController() : base(new SeatService(new Data.MiracleInTheMakingContext())) { }
    }
}
