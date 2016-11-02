DECLARE @DedicationBinaryFileTypeGuid AS VARCHAR(100)
DECLARE @DedicationBinaryFileTypeID AS INT
SET @DedicationBinaryFileTypeGuid = '014f36c2-07ce-40da-a8e8-de97b70490b6'
SELECT @DedicationBinaryFileTypeID = (SELECT Id FROM Rock.dbo.BinaryFileType WHERE [Guid] = @DedicationBinaryFileTypeGuid)
DECLARE @BlobId AS INT

-------------------------------------------------------------
--
-- Sync the Seat table.
--
INSERT INTO Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat
	(Section, SeatNumber, [Guid], CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, ForeignId)
	SELECT
		adb.section,
		adb.seat_number,
		NEWID(),
		adb.date_created,
		adb.date_modified,
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.created_by),
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by),
		adb.seat_id
		FROM ArenaDB.dbo.cust_sothc_mitm_seat AS adb
		WHERE adb.seat_id NOT IN (SELECT ForeignId FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat WHERE ForeignId IS NOT NULL)

UPDATE rdb SET
	rdb.Section = adb.section,
	rdb.SeatNumber = adb.seat_number,
	rdb.ModifiedDateTime = adb.date_modified,
	rdb.ModifiedByPersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForUsername(modified_by)
	FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat AS rdb
	INNER JOIN ArenaDB.dbo.cust_sothc_mitm_seat AS adb ON adb.seat_id = rdb.ForeignId
	WHERE adb.date_modified > rdb.ModifiedDateTime


--------------------------------------------------------------------
--
-- Sync the SeatPledge table.
--
INSERT INTO Rock.dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge
	(PledgedPersonAliasId, Amount, AssignedSeatId, RequestedSeatId, RequestedFullSeat, RequestedBackRest, RequestedLeg1, RequestedLeg2, RequestedLeg3, RequestedLeg4, RequestedArmLeft, RequestedArmRight, [Guid], CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, ForeignId)
	SELECT
		Rock.dbo._Migrate_RockPersonAliasIdForArenaId(adb.person_id),
		adb.amount,
		(SELECT Id FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat WHERE ForeignId = adb.assigned_seat_id),
		(SELECT Id FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat WHERE ForeignId = adb.requested_seat_id),
		adb.requested_full_seat,
		adb.requested_back_rest,
		adb.requested_leg1,
		adb.requested_leg2,
		adb.requested_leg3,
		adb.requested_leg4,
		adb.requested_arm_left,
		adb.requested_arm_right,
		NEWID(),
		adb.date_created,
		adb.date_modified,
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.created_by),
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by),
		adb.seat_pledge_id
		FROM ArenaDB.dbo.cust_sothc_mitm_seat_pledge AS adb
		WHERE adb.seat_pledge_id NOT IN (SELECT ForeignId FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge WHERE ForeignId IS NOT NULL)

UPDATE rdb SET
	rdb.PledgedPersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForArenaId(adb.person_id),
	rdb.Amount = adb.amount,
	rdb.AssignedSeatId = (SELECT Id FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat WHERE ForeignId = adb.assigned_seat_id),
	rdb.RequestedSeatId = (SELECT Id FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Seat WHERE ForeignId = adb.requested_seat_id),
	rdb.RequestedFullSeat = adb.requested_full_seat,
	rdb.RequestedBackRest = adb.requested_back_rest,
	rdb.RequestedLeg1 = adb.requested_leg1,
	rdb.RequestedLeg2 = adb.requested_leg2,
	rdb.RequestedLeg3 = adb.requested_leg3,
	rdb.RequestedLeg4 = adb.requested_leg4,
	rdb.RequestedArmLeft = adb.requested_arm_left,
	rdb.RequestedArmRight = adb.requested_arm_right,
	rdb.ModifiedByPersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by),
	rdb.ModifiedDateTime = adb.date_modified
	FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge AS rdb
	INNER JOIN ArenaDB.dbo.cust_sothc_mitm_seat_pledge AS adb ON adb.seat_pledge_id = rdb.ForeignId
	WHERE adb.date_modified > rdb.ModifiedDateTime


--------------------------------------------------
--
-- Create the BinaryFileType for Dedications.
--
IF NOT EXISTS (SELECT * FROM Rock.dbo.BinaryFileType WHERE [Guid] = @DedicationBinaryFileTypeGuid)
BEGIN
	INSERT INTO Rock.dbo.BinaryFileType
		(IsSystem, [Name], [Description], IconCssClass, StorageEntityTypeId, AllowCaching, [Guid], CreatedDateTime, ModifiedDateTime, RequiresViewSecurity, MaxWidth, MaxHeight, PreferredFormat, PreferredResolution, PreferredColorDepth, PreferredRequired)
		VALUES
		(
			0,
			'MiTM Dedication Image',
			'Image associated with Miracle in The Making Dedications',
			'',
			(SELECT Id FROM Rock.dbo.EntityType WHERE [Guid] = '0AA42802-04FD-4AEC-B011-FEB127FC85CD'),
			1,
			@DedicationBinaryFileTypeGuid,
			GETDATE(),
			GETDATE(),
			0,
			0,
			0,
			-1,
			-1,
			-1,
			-1
		)
END


---------------------------------------------------
--
-- Sync Blobs for the Dedications
--
DECLARE ArenaCursor INSENSITIVE CURSOR FOR
	SELECT
		 B.[blob_id]
	FROM ArenaDB.dbo.cust_sothc_mitm_dedication AS D WITH (NOLOCK)
	INNER JOIN ArenaDB.dbo.[util_blob] B WITH (NOLOCK) ON B.[blob_id] = D.[blob_id]

OPEN ArenaCursor
FETCH NEXT FROM ArenaCursor INTO @BlobId
WHILE (@@FETCH_STATUS <> -1)
BEGIN
	IF (@@FETCH_STATUS = 0)
	BEGIN
		EXEC Rock.dbo._Migrate_MigrateBlob @BlobId, @DedicationBinaryFileTypeID, NULL
		FETCH NEXT FROM ArenaCursor INTO @BlobId
	END
END
CLOSE ArenaCursor
DEALLOCATE ArenaCursor


--------------------------------------------------------------
--
-- Sync dedications
--
INSERT INTO Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Dedication
	(SeatPledgeId, ApprovedBy, DedicatedTo, SponsoredBy, Biography, IsAnonymous, BinaryFileId, [Guid], CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, ForeignId)
	SELECT
		(SELECT Id FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_SeatPledge WHERE ForeignId = adb.seat_pledge_id),
		adb.approved_by,
		adb.dedicated_to,
		adb.sponsored_by,
		adb.biography,
		adb.[anonymous],
		(SELECT Id FROM Rock.dbo.BinaryFile WHERE ForeignId = adb.blob_id),
		NEWID(),
		adb.date_created,
		adb.date_modified,
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.created_by),
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by),
		adb.dedication_id
		FROM ArenaDB.dbo.cust_sothc_mitm_dedication AS adb
		WHERE adb.dedication_id NOT IN (SELECT ForeignId FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Dedication)

UPDATE rdb SET
	rdb.ApprovedBy = adb.approved_by,
	rdb.DedicatedTo = adb.dedicated_to,
	rdb.SponsoredBy = adb.sponsored_by,
	rdb.Biography = adb.biography,
	rdb.IsAnonymous = adb.[anonymous],
	rdb.BinaryFileId = (SELECT Id FROM Rock.dbo.BinaryFile WHERE ForeignId = adb.blob_id),
	rdb.ModifiedDateTime = adb.date_modified,
	rdb.ModifiedByPersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by)
	FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Dedication AS rdb
	INNER JOIN ArenaDB.dbo.cust_sothc_mitm_dedication AS adb ON adb.dedication_id = rdb.Id
	WHERE adb.date_modified > rdb.ModifiedDateTime


-----------------------------------------------
--
-- Sync the Salvation table.
--
INSERT INTO Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Salvation
	(PersonAliasId, FirstName, LastName, IsSaved, [Guid], CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, ForeignId)
	SELECT
		Rock.dbo._Migrate_RockPersonAliasIdForArenaId(adb.person_id),
		adb.first_name,
		adb.last_name,
		adb.[status],
		NEWID(),
		adb.date_created,
		adb.date_modified,
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.created_by),
		Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by),
		adb.salvation_id
		FROM ArenaDB.dbo.cust_sothc_mitm_salvation AS adb
		WHERE adb.salvation_id NOT IN (SELECT ForeignId FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Salvation)

UPDATE rdb SET
	rdb.PersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForArenaId(adb.person_id),
	rdb.FirstName = adb.first_name,
	rdb.LastName = adb.last_name,
	rdb.IsSaved = adb.[status],
	rdb.ModifiedDateTime = adb.date_modified,
	rdb.ModifiedByPersonAliasId = Rock.dbo._Migrate_RockPersonAliasIdForUsername(adb.modified_by)
	FROM Rock.dbo._com_shepherdchurch_MiracleInTheMaking_Salvation AS rdb
	INNER JOIN ArenaDB.dbo.cust_sothc_mitm_salvation AS adb ON adb.salvation_id = rdb.Id
	WHERE adb.date_modified > rdb.ModifiedDateTime
