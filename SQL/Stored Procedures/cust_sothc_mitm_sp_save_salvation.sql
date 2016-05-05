SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_save_salvation]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_save_salvation]
GO

CREATE PROC cust_sothc_mitm_sp_save_salvation
    @SalvationID INT,
    @PersonID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Status BIT,
    @UserID VARCHAR(50),
    @ID INT OUTPUT
AS
    DECLARE @UpdateDateTime DateTime SET @UpdateDateTime = GETDATE()

    IF NOT EXISTS (
        SELECT * FROM cust_sothc_mitm_salvation
            WHERE [salvation_id] = @SalvationID
    )
    BEGIN
        INSERT INTO cust_sothc_mitm_salvation
        (	
             [date_created]
            ,[date_modified]
            ,[created_by]
            ,[modified_by]
            ,[person_id]
            ,[first_name]
            ,[last_name]
            ,[status]
        )
        VALUES
        (
             @UpdateDateTime
            ,@UpdateDateTime
            ,@UserID
            ,@UserID
            ,@PersonID
            ,@FirstName
            ,@LastName
            ,@Status
        )

        SET @ID = @@IDENTITY
    END
    ELSE
    BEGIN
        UPDATE cust_sothc_mitm_salvation SET
             [date_modified] = @UpdateDateTime
            ,[modified_by] = @UserID
            ,[person_id] = @PersonID
            ,[first_name] = @FirstName
            ,[last_name] = @LastName
            ,[status] = @Status
            WHERE [salvation_id] = @SalvationID

        SET @ID = @SalvationID
    END
GO
