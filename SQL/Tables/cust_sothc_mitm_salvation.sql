SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cust_sothc_mitm_salvation] (
    [salvation_id] [int] IDENTITY(1,1) NOT NULL,
    [date_created] [datetime] NOT NULL,
    [date_modified] [datetime] NOT NULL,
    [created_by] [nvarchar](50) NOT NULL,
    [modified_by] [nvarchar](50) NOT NULL,
    [person_id] [int] NOT NULL,
    [first_name] [nvarchar](50) NOT NULL,
    [last_name] [nvarchar](50) NOT NULL,
    [status] [bit] NOT NULL,
    CONSTRAINT [PK_cust_sothc_mitm_salvation] PRIMARY KEY CLUSTERED 
    (
        [salvation_id] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[cust_sothc_mitm_salvation] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_salvation_core_person] FOREIGN KEY([person_id])
    REFERENCES [dbo].[core_person] ([person_id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[cust_sothc_mitm_salvation] CHECK CONSTRAINT [FK_cust_sothc_mitm_salvation_core_person]
GO
