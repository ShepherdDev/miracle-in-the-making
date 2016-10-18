using Rock.Plugin;

namespace com.shepherdchurch.MiracleInTheMaking.Migrations
{
    [MigrationNumber(4, "1.4.0")]
    public class CreateTable_Dedication : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql(@"CREATE TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] (
	    [Id] [int] IDENTITY(1,1) NOT NULL,
        [SeatPledgeId] [int] NOT NULL,
	    [ApprovedBy] [nvarchar](100) NULL,
        [DedicatedTo] [nvarchar](100) NOT NULL,
        [SponsoredBy] [nvarchar](100) NOT NULL,
        [Biography] [text] NOT NULL,
        [IsAnonymous] [bit] NOT NULL,
        [BinaryFileId] [int] NULL,
        [Guid] [uniqueidentifier] NOT NULL,
	    [CreatedDateTime] [datetime] NULL,
	    [ModifiedDateTime] [datetime] NULL,
	    [CreatedByPersonAliasId] [int] NULL,
	    [ModifiedByPersonAliasId] [int] NULL,
	    [ForeignKey] [nvarchar](50) NULL,
        CONSTRAINT [PK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication] PRIMARY KEY CLUSTERED 
        (
	        [Id] ASC
        ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    )
    
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId] FOREIGN KEY([SeatPledgeId]) REFERENCES [dbo].[_com_shepherdchurch_MiracleInTheMaking_SeatPledge] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.BinaryFile_BinaryFileId] FOREIGN KEY([BinaryFileId]) REFERENCES [dbo].[BinaryFile] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.BinaryFile_BinaryFileId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_CreatedByPersonAliasId] FOREIGN KEY([CreatedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_CreatedByPersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_ModifiedByPersonAliasId] FOREIGN KEY([ModifiedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_ModifiedByPersonAliasId]
    ");
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql(@"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge_SeatPledgeId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.BinaryFile_BinaryFileId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_ModifiedByPersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Dedication_dbo.PersonAlias_CreatedByPersonAliasId]
    DROP TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Dedication]
    ");
        }
    }
}
