SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_salvation_byPersonID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_salvation_byPersonID]
GO

CREATE PROC cust_sothc_mitm_sp_get_salvation_byPersonID
    @PersonID INT
AS
    SELECT * 
        FROM cust_sothc_mitm_salvation
        WHERE [person_id] = @PersonID
GO
