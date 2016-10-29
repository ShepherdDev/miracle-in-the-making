using Rock.Plugin;

namespace com.shepherdchurch.MiracleInTheMaking.Migrations
{
    [MigrationNumber( 5, "1.4.0" )]
    public class ModifyTable_DedicationForeignKeyCascadeDelete : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql( @"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId] FOREIGN KEY([SeatPledgeId]) REFERENCES [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] ([Id]) ON DELETE CASCADE
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]
    " );
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql( @"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId] FOREIGN KEY([SeatPledgeId]) REFERENCES [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]
    " );
        }
    }
}
