﻿SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_seatPledge_byID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_seatPledge_byID]
GO

CREATE PROC cust_sothc_mitm_sp_get_seatPledge_byID
    @SeatPledgeID INT
AS
    SELECT * 
        FROM cust_sothc_mitm_seat_pledge
        WHERE [seat_pledge_id] = @SeatPledgeID
GO
