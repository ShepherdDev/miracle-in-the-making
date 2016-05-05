SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_save_dedication]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_save_dedication]
GO

CREATE PROC cust_sothc_mitm_sp_save_dedication
    @DedicationID INT,
    @ApprovedBy VARCHAR(50),
    @SeatPledgeID INT,
    @DedicatedTo VARCHAR(100),
    @SponsoredBy VARCHAR(100),
    @Biography TEXT,
    @Anonymous BIT,
    @BlobID INT,
    @UserID VARCHAR(50),
    @ID INT OUTPUT
AS
    DECLARE @UpdateDateTime DateTime SET @UpdateDateTime = GETDATE()

    IF NOT EXISTS (
        SELECT * FROM cust_sothc_mitm_dedication
            WHERE [dedication_id] = @DedicationID
    )
    BEGIN
        INSERT INTO cust_sothc_mitm_dedication
        (	
             [date_created]
            ,[date_modified]
            ,[created_by]
            ,[modified_by]
            ,[approved_by]
            ,[seat_pledge_id]
            ,[dedicated_to]
            ,[sponsored_by]
            ,[biography]
            ,[anonymous]
            ,[blob_id]
        )
        VALUES
        (
             @UpdateDateTime
            ,@UpdateDateTime
            ,@UserID
            ,@UserID
            ,@ApprovedBy
            ,@SeatPledgeID
            ,@DedicatedTo
            ,@SponsoredBy
            ,@Biography
            ,@Anonymous
            ,@BlobID
        )

        SET @ID = @@IDENTITY
    END
    ELSE
    BEGIN
        UPDATE cust_sothc_mitm_dedication SET
             [date_modified] = @UpdateDateTime
            ,[modified_by] = @UserID
            ,[approved_by] = @ApprovedBy
            ,[seat_pledge_id] = @SeatPledgeID
            ,[dedicated_to] = @DedicatedTo
            ,[sponsored_by] = @SponsoredBy
            ,[biography] = @Biography
            ,[anonymous] = @Anonymous
            ,[blob_id] = @BlobID
            WHERE [dedication_id] = @DedicationID

        SET @ID = @DedicationID
    END
GO
