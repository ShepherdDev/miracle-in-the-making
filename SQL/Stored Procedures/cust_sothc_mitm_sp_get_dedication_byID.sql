﻿SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_get_dedication_byID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_get_dedication_byID]
GO

CREATE PROC cust_sothc_mitm_sp_get_dedication_byID
    @DedicationID INT
AS
    SELECT * 
        FROM cust_sothc_mitm_dedication
        WHERE [dedication_id] = @DedicationID
GO
