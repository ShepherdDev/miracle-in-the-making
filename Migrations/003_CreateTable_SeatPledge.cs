using Rock.Plugin;

namespace com.shepherdchurch.MiracleInTheMaking.Migrations
{
    [MigrationNumber(3, "1.4.0")]
    public class CreateTable_SeatPledge : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql(@"CREATE TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] (
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [PledgedPersonAliasId] [int] NOT NULL,
        [Amount] [money] NOT NULL,
        [AssignedSeatId] [int] NULL,
        [RequestedSeatId] [int] NULL,
        [RequestedFullSeat] [int] NOT NULL,
        [RequestedBackRest] [int] NOT NULL,
        [RequestedLeg1] [int] NOT NULL,
        [RequestedLeg2] [int] NOT NULL,
        [RequestedLeg3] [int] NOT NULL,
        [RequestedLeg4] [int] NOT NULL,
        [RequestedArmLeft] [int] NOT NULL,
        [RequestedArmRight] [int] NOT NULL,
        [Guid] [uniqueidentifier] NOT NULL,
	    [CreatedDateTime] [datetime] NULL,
	    [ModifiedDateTime] [datetime] NULL,
	    [CreatedByPersonAliasId] [int] NULL,
	    [ModifiedByPersonAliasId] [int] NULL,
	    [ForeignKey] [nvarchar](50) NULL,
        CONSTRAINT [PK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge] PRIMARY KEY CLUSTERED 
        (
	        [Id] ASC
        ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    )
    
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_PledgedPersonAliasId] FOREIGN KEY([PledgedPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_PledgedPersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_AssignedSeatId] FOREIGN KEY([AssignedSeatId]) REFERENCES [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_AssignedSeatId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_RequestedSeatId] FOREIGN KEY([RequestedSeatId]) REFERENCES [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_RequestedSeatId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_CreatedByPersonAliasId] FOREIGN KEY([CreatedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_CreatedByPersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_ModifiedByPersonAliasId] FOREIGN KEY([ModifiedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_ModifiedByPersonAliasId]
    ");
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql(@"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_PledgedPersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_RequestedSeatId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_AssignedSeatId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_ModifiedByPersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_dbo.PersonAlias_CreatedByPersonAliasId]
    DROP TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge]
    ");
        }
    }
}
