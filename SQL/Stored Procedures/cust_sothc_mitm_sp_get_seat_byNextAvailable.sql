SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_seat_byNextAvailable]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_seat_byNextAvailable]
GO

CREATE PROC cust_sothc_mitm_sp_get_seat_byNextAvailable
    @Amount MONEY,
    @SeatPledgeID INT
AS
    SELECT TOP 1 s.*
	    FROM cust_sothc_mitm_seat AS s
	    WHERE (SELECT ISNULL(SUM(amount),0) FROM cust_sothc_mitm_seat_pledge AS sp WHERE sp.assigned_seat_id = s.seat_id AND sp.seat_pledge_id <> @SeatPledgeID) + @Amount <= 10000
	    ORDER BY s.section,s.seat_number
GO
