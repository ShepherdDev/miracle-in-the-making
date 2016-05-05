SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_del_seat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_del_seat]
GO

CREATE PROC cust_sothc_mitm_sp_del_seat
    @SeatID INT
AS
    DELETE cust_sothc_mitm_seat
        WHERE [seat_id] = @SeatID
GO
