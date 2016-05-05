SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[cust_sothc_mitm_sp_save_seatPledge]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE [cust_sothc_mitm_sp_save_seatPledge]
GO

CREATE PROC cust_sothc_mitm_sp_save_seatPledge
    @SeatPledgeID INT,
    @PersonID INT,
    @Amount MONEY,
    @AssignedSeatID INT,
    @RequestedSeatID INT,
    @RequestedFullSeat INT,
    @RequestedBackRest INT,
    @RequestedLeg1 INT,
    @RequestedLeg2 INT,
    @RequestedLeg3 INT,
    @RequestedLeg4 INT,
    @RequestedArmLeft INT,
    @RequestedArmRight INT,
    @UserID VARCHAR(50),
    @ID INT OUTPUT
AS
    DECLARE @UpdateDateTime DateTime SET @UpdateDateTime = GETDATE()

    IF NOT EXISTS (
        SELECT * FROM cust_sothc_mitm_seat_pledge
            WHERE [seat_pledge_id] = @SeatPledgeID
    )
    BEGIN
        INSERT INTO cust_sothc_mitm_seat_pledge
        (	
             [date_created]
            ,[date_modified]
            ,[created_by]
            ,[modified_by]
            ,[person_id]
            ,[amount]
            ,[assigned_seat_id]
            ,[requested_seat_id]
            ,[requested_full_seat]
            ,[requested_back_rest]
            ,[requested_leg1]
            ,[requested_leg2]
            ,[requested_leg3]
            ,[requested_leg4]
            ,[requested_arm_left]
            ,[requested_arm_right]
        )
        VALUES
        (
             @UpdateDateTime
            ,@UpdateDateTime
            ,@UserID
            ,@UserID
            ,@PersonID
            ,@Amount
            ,@AssignedSeatID
            ,@RequestedSeatID
            ,@RequestedFullSeat
            ,@RequestedBackRest
            ,@RequestedLeg1
            ,@RequestedLeg2
            ,@RequestedLeg3
            ,@RequestedLeg4
            ,@RequestedArmLeft
            ,@RequestedArmRight
        )

        SET @ID = @@IDENTITY
    END
    ELSE
    BEGIN
        UPDATE cust_sothc_mitm_seat_pledge SET
             [date_modified] = @UpdateDateTime
            ,[modified_by] = @UserID
            ,[person_id] = @PersonID
            ,[amount] = @Amount
            ,[assigned_seat_id] = @AssignedSeatID
            ,[requested_seat_id] = @RequestedSeatID
            ,[requested_full_seat] = @RequestedFullSeat
            ,[requested_back_rest] = @RequestedBackRest
            ,[requested_leg1] = @RequestedLeg1
            ,[requested_leg2] = @RequestedLeg2
            ,[requested_leg3] = @RequestedLeg3
            ,[requested_leg4] = @RequestedLeg4
            ,[requested_arm_left] = @RequestedArmLeft
            ,[requested_arm_right] = @RequestedArmRight
            WHERE [seat_pledge_id] = @SeatPledgeID

        SET @ID = @SeatPledgeID
    END
GO
