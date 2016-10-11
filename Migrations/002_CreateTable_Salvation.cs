using Rock.Plugin;

namespace com.shepherdchurch.MiracleInTheMaking.Migrations
{
    [MigrationNumber(2, "1.4.0")]
    public class CreateTable_Salvation : Migration
    {
        /// <summary>
        /// The commands to run to migrate plugin to the specific version
        /// </summary>
        public override void Up()
        {
            Sql(@"CREATE TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] (
	    [Id] [int] IDENTITY(1,1) NOT NULL,
	    [PersonAliasId] [int] NOT NULL,
        [FirstName] [nvarchar](100) NOT NULL,
        [LastName] [nvarchar](100) NOT NULL,
        [IsSaved] [bit] NOT NULL,
        [Guid] [uniqueidentifier] NOT NULL,
	    [CreatedDateTime] [datetime] NULL,
	    [ModifiedDateTime] [datetime] NULL,
	    [CreatedByPersonAliasId] [int] NULL,
	    [ModifiedByPersonAliasId] [int] NULL,
	    [ForeignKey] [nvarchar](50) NULL,
        CONSTRAINT [PK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation] PRIMARY KEY CLUSTERED 
        (
	        [Id] ASC
        ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
    )
    
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_PersonAliasId] FOREIGN KEY([PersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_PersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_CreatedByPersonAliasId] FOREIGN KEY([CreatedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_CreatedByPersonAliasId]

    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] WITH CHECK ADD CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_ModifiedByPersonAliasId] FOREIGN KEY([ModifiedByPersonAliasId]) REFERENCES [dbo].[PersonAlias] ([Id])
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] CHECK CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_ModifiedByPersonAliasId]
    ");
        }

        /// <summary>
        /// The commands to undo a migration from a specific version
        /// </summary>
        public override void Down()
        {
            Sql(@"
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_PersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_ModifiedByPersonAliasId]
    ALTER TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation] DROP CONSTRAINT [FK_dbo._com_shepherdchurch_MiracleInTheMaking_Salvation_dbo.PersonAlias_CreatedByPersonAliasId]
    DROP TABLE [dbo].[_com_shepherdchurch_MiracleInTheMaking_Salvation]
    ");
        }
    }
}
