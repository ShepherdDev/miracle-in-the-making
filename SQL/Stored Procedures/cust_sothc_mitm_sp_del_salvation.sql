SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_del_salvation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_del_salvation]
GO

CREATE PROC cust_sothc_mitm_sp_del_salvation
    @SalvationID INT
AS
    DELETE cust_sothc_mitm_salvation
        WHERE [salvation_id] = @SalvationID
GO