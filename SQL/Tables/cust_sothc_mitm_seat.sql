SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cust_sothc_mitm_seat] (
    [seat_id] [int] IDENTITY(1,1) NOT NULL,
    [date_created] [datetime] NOT NULL,
    [date_modified] [datetime] NOT NULL,
    [created_by] [varchar](50) NOT NULL,
    [modified_by] [varchar](50) NOT NULL,
    [section] [nchar](1) NOT NULL,
    [seat_number] [int] NOT NULL,
    CONSTRAINT [PK_cust_sothc_mitm_seat] PRIMARY KEY CLUSTERED 
    (
        [seat_id] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
    CONSTRAINT [IX_cust_sothc_mitm_seat] UNIQUE NONCLUSTERED 
    (
        [section] ASC,
        [seat_number] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
