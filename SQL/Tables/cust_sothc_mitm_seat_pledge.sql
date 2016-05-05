SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[cust_sothc_mitm_seat_pledge](
    [seat_pledge_id] [int] IDENTITY(1,1) NOT NULL,
    [date_created] [datetime] NOT NULL,
    [date_modified] [datetime] NOT NULL,
    [created_by] [varchar](50) NOT NULL,
    [modified_by] [varchar](50) NOT NULL,
    [person_id] [int] NOT NULL,
    [amount] [money] NOT NULL,
    [assigned_seat_id] [int] NULL,
    [requested_seat_id] [int] NULL,
    [requested_full_seat] [int] NOT NULL,
    [requested_back_rest] [int] NOT NULL,
    [requested_leg1] [int] NOT NULL,
    [requested_leg2] [int] NOT NULL,
    [requested_leg3] [int] NOT NULL,
    [requested_leg4] [int] NOT NULL,
    [requested_arm_left] [int] NOT NULL,
    [requested_arm_right] [int] NOT NULL,
    CONSTRAINT [PK_cust_sothc_mitm_seat_pledge] PRIMARY KEY CLUSTERED 
    (
        [seat_pledge_id] ASC
    ) WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_core_person] FOREIGN KEY([person_id])
    REFERENCES [dbo].[core_person] ([person_id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] CHECK CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_core_person]
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_cust_sothc_mitm_seat] FOREIGN KEY([assigned_seat_id])
    REFERENCES [dbo].[cust_sothc_mitm_seat] ([seat_id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] CHECK CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_cust_sothc_mitm_seat]
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] WITH CHECK ADD CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_cust_sothc_mitm_seat1] FOREIGN KEY([requested_seat_id])
    REFERENCES [dbo].[cust_sothc_mitm_seat] ([seat_id])
GO

ALTER TABLE [dbo].[cust_sothc_mitm_seat_pledge] CHECK CONSTRAINT [FK_cust_sothc_mitm_seat_pledge_cust_sothc_mitm_seat1]
GO


