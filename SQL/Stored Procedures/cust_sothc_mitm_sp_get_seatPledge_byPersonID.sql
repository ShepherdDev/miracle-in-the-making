SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_seatPledge_byPersonID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_seatPledge_byPersonID]
GO

CREATE PROC cust_sothc_mitm_sp_get_seatPledge_byPersonID
    @PersonID INT
AS
    SELECT * 
        FROM cust_sothc_mitm_seat_pledge
        WHERE [person_id] = @PersonID
GO
