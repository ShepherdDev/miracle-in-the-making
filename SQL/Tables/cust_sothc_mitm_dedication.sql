SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cust_sothc_mitm_dedication] (
    [dedication_id] [int] IDENTITY(1,1) NOT NULL,
    [date_created] [datetime] NOT NULL,
    [date_modified] [datetime] NOT NULL,
    [created_by] [varchar](50) NOT NULL,
    [modified_by] [varchar](50) NOT NULL,
    [approved_by] [varchar](50) NOT NULL,
    [seat_pledge_id] [int] NOT NULL,
    [dedicated_to] [varchar](100) NOT NULL,
    [sponsored_by] [varchar](100) NOT NULL,
    [biography] [text] NOT NULL,
    [anonymous] [bit] NOT NULL,
    [blob_id] [int] NULL,
    CONSTRAINT [PK_cust_sothc_mitm_dedication] PRIMARY KEY CLUSTERED 
    (
        [dedication_id] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[cust_sothc_mitm_dedication] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_dedication_cust_sothc_mitm_seat_pledge] FOREIGN KEY([seat_pledge_id])
    REFERENCES [dbo].[cust_sothc_mitm_seat_pledge] ([seat_pledge_id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[cust_sothc_mitm_dedication] CHECK CONSTRAINT [FK_cust_sothc_mitm_dedication_cust_sothc_mitm_seat_pledge]
GO

ALTER TABLE [dbo].[cust_sothc_mitm_dedication] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_dedication_util_blob] FOREIGN KEY([blob_id])
    REFERENCES [dbo].[util_blob] ([blob_id]) ON DELETE SET NULL
GO

ALTER TABLE [dbo].[cust_sothc_mitm_dedication] CHECK CONSTRAINT [FK_cust_sothc_mitm_dedication_util_blob]
GO
