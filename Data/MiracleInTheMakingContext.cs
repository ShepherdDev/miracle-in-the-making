﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web;

using com.shepherdchurch.MiracleInTheMaking.Model;

namespace com.shepherdchurch.MiracleInTheMaking.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MiracleInTheMakingContext : Rock.Data.DbContext
    {

        #region Models

        public DbSet<Seat> Seats { get; set; }

        public DbSet<SeatPledge> SeatPledges { get; set; }

        public DbSet<Salvation> Salvations { get; set; }

        public DbSet<Dedication> Dedications { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MiracleInTheMakingContext"/> class.
        /// </summary>
        public MiracleInTheMakingContext()
            : base("RockContext")
        {
            //intentionally left blank
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // we don't want this context to create a database or look for EF Migrations, do set the Initializer to null
            Database.SetInitializer<MiracleInTheMakingContext>(new NullDatabaseInitializer<MiracleInTheMakingContext>());

            Rock.Data.ContextHelper.AddConfigurations(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

    }
}
