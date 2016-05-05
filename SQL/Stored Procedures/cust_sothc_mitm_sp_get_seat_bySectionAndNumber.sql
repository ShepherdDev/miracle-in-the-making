SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_seat_bySectionAndNumber]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_seat_bySectionAndNumber]
GO

CREATE PROC cust_sothc_mitm_sp_get_seat_bySectionAndNumber
    @Section NCHAR(1),
    @SeatNumber INT
AS
    SELECT * 
        FROM cust_sothc_mitm_seat
        WHERE [section] = @Section
          AND [seat_number] = @SeatNumber
GO
