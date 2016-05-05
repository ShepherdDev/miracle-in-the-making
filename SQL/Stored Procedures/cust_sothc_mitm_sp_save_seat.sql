SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_save_seat]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_save_seat]
GO

CREATE PROC cust_sothc_mitm_sp_save_seat
    @SeatID INT,
    @Section NCHAR(1),
    @SeatNumber INT,
    @UserID VARCHAR(50),
    @ID INT OUTPUT
AS
    DECLARE @UpdateDateTime DateTime SET @UpdateDateTime = GETDATE()

    IF NOT EXISTS (
        SELECT * FROM cust_sothc_mitm_seat
            WHERE [seat_id] = @SeatID
    )
    BEGIN
        INSERT INTO cust_sothc_mitm_seat
        (	
             [date_created]
            ,[date_modified]
            ,[created_by]
            ,[modified_by]
            ,[section]
            ,[seat_number]
        )
        VALUES
        (
             @UpdateDateTime
            ,@UpdateDateTime
            ,@UserID
            ,@UserID
            ,@Section
            ,@SeatNumber
        )

        SET @ID = @@IDENTITY
    END
    ELSE
    BEGIN
        UPDATE cust_sothc_mitm_seat SET
             [date_modified] = @UpdateDateTime
            ,[modified_by] = @UserID
            ,[section] = @Section
            ,[seat_number] = @SeatNumber
            WHERE [seat_id] = @SeatID

        SET @ID = @SeatID
    END
GO
