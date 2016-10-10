using Rock.Plugin;

namespace com.shepherdchurch.MiracleInTheMaking.Migrations
{
    [MigrationNumber(1, "1.4.0")]
    public class CreateTable_Seat : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql(@"
    CREATE TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] (
	    [Id] [int] IDENTITY(1,1) NOT NULL,
        [Section] [nchar](1) NOT NULL,
        [SeatNumber] [int] NOT NULL,
	    [Guid] [uniqueidentifier] NOT NULL,
	    [CreatedDateTime] [datetime] NULL,
	    [ModifiedDateTime] [datetime] NULL,
	    [CreatedByPersonAliasId] [int] NULL,
	    [ModifiedByPersonAliasId] [int] NULL,
	    [ForeignId] [nvarchar](50) NULL,
        CONSTRAINT [PK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat] PRIMARY KEY CLUSTERED 
        (
	        [Id] ASC
        ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
        CONSTRAINT [IX_dbo._com_shepherdchurch_MiracleInTheMaking_Seat] UNIQUE NONCLUSTERED 
        (
            [Section] ASC,
            [SeatNumber] ASC
        ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    )
    
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_CreatedByPersonAliasId] FOREIGN KEY([CreatedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_CreatedByPersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_ModifiedByPersonAliasId] FOREIGN KEY([ModifiedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_ModifiedByPersonAliasId]
    ");
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql(@"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_ModifiedByPersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Seat_dbo.PersonAlias_CreatedByPersonAliasId]
    DROP TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Seat]
    ");
        }
    }
}
