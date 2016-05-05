SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_del_seatPledge]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_del_seatPledge]
GO

CREATE PROC cust_sothc_mitm_sp_del_seatPledge
    @SeatPledgeID INT
AS
    DELETE cust_sothc_mitm_seat_pledge
        WHERE [seat_pledge_id] = @SeatPledgeID
GO