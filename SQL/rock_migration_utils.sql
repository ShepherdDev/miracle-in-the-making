USE Rock
GO

-------------------------------------------------------
--
-- Helper function to get the Rock PersonAliasId for the given Arena UserName.
--
IF OBJECT_ID('_Migrate_RockPersonAliasIdForUsername') IS NULL
	EXEC ('CREATE FUNCTION _Migrate_RockPersonAliasIdForUsername() RETURNS INT AS BEGIN RETURN NULL END')
GO
ALTER FUNCTION [dbo].[_Migrate_RockPersonAliasIdForUsername](@UserName VARCHAR(100)) RETURNS INT
AS
BEGIN
	IF SUBSTRING(@UserName, 0, 13) = 'THESHEPHERD\'
		SET @UserName = SUBSTRING(@UserName, 13, 99)

	RETURN (SELECT TOP 1 Id FROM PersonAlias WHERE PersonId = (SELECT PersonId FROM UserLogin WHERE UserName = @UserName))
END
GO


-------------------------------------------------------
--
-- Helper function to get the Rock PersonAliasId for the given Arena UserName.
--
IF OBJECT_ID('_Migrate_RockPersonAliasIdForArenaId') IS NULL
	EXEC ('CREATE FUNCTION _Migrate_RockPersonAliasIdForArenaId() RETURNS INT AS BEGIN RETURN NULL END')
GO
ALTER FUNCTION [dbo].[_Migrate_RockPersonAliasIdForArenaId](@ArenaId INT) RETURNS INT
AS
BEGIN
	RETURN (SELECT TOP 1 Id FROM Rock.dbo.PersonAlias WHERE PersonId = (SELECT Id FROM Rock.dbo.Person WHERE ForeignId = @ArenaId))
END
GO


---------------------------------------------------------
--
-- Helper sproc to create a schedule in Rock.
--
IF OBJECT_ID('_Migrate_CreateRockSchedule') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_CreateRockSchedule AS SELECT 1')
GO
ALTER PROCEDURE _Migrate_CreateRockSchedule
(
	@StartDate AS DATETIME,
	@ArenaFrequency AS INT,
	@RockId AS INT OUTPUT
)
AS
BEGIN
	DECLARE @ICAL AS VARCHAR(1000)
	DECLARE @RRule AS VARCHAR(200)

	IF @ArenaFrequency = 1
		SET @RRule = 'FREQ=DAILY'
	ELSE IF @ArenaFrequency = 2
		SET @RRule = 'FREQ=WEEKLY;BYDAY=' + UPPER(SUBSTRING(DATENAME(WEEKDAY, @StartDate), 1, 2))
	ELSE IF @ArenaFrequency = 3
		SET @RRule = 'FREQ=MONTHLY;BYMONTHDAY=' + CAST(DATEPART(DAY, @StartDate) AS VARCHAR(2))
	ELSE
	BEGIN
		SET @RockId = NULL
		RETURN
	END

	SET @ICAL =
		'BEGIN:VCALENDAR' + CHAR(13) +
		'BEGIN:VEVENT' + CHAR(13) +
		'DTEND:' + CONVERT(varchar(10), @StartDate, 112) + 'T000001' + CHAR(13) +
		'DTSTART:' + CONVERT(VARCHAR(10), @StartDate, 112) + 'T000000' + CHAR(13) +
		'RRULE:' + @RRule + CHAR(13) +
		'END:VEVENT' + CHAR(13) +
		'END:VCALENDAR' + CHAR(13)

	INSERT INTO Schedule
		([Name], iCalendarContent, EffectiveStartDate, EffectiveEndDate, CategoryId, [Guid], CreatedDateTime, ModifiedDateTime)
		SELECT
			'',
			@ICAL,
			CAST(@StartDate AS DATE),
			CAST(@StartDate AS DATE),
			(SELECT Id FROM Category WHERE [Guid] = '5A794741-5444-43F0-90D7-48E47276D426'),
			NEWID(),
			GETDATE(),
			GETDATE()
	SET @RockId = SCOPE_IDENTITY()
END
GO


-------------------------------------------------------
--
-- Create (or get) a generic metric category for metrics at the root level in Arena.
--
IF OBJECT_ID('_Migrate_GenericMetricCategory') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_GenericMetricCategory AS SELECT 1')
GO
ALTER PROCEDURE _Migrate_GenericMetricCategory
(
	@RockId INT OUTPUT
)
AS
BEGIN
	SELECT @RockId = Id FROM Category
		WHERE ParentCategoryId IS NULL AND EntityTypeId = (SELECT Id FROM EntityType WHERE [Name] = 'Rock.Model.MetricCategory')
	IF @RockId IS NOT NULL
		RETURN

	INSERT INTO Category
		(IsSystem, ParentCategoryId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Name], IconCssClass, [Guid], [Order], [Description], CreatedDateTime, ModifiedDateTime, HighlightColor)
		VALUES
		(
			0,
			NULL,
			(SELECT Id FROM EntityType WHERE [Name] = 'Rock.Model.MetricCategory'),
			'',
			'',
			'General Metrics',
			'',
			NEWID(),
			(SELECT COUNT(*) FROM Category WHERE EntityTypeId = (SELECT Id FROM EntityType WHERE [Name] = 'Rock.Model.MetricCategory') AND ParentCategoryId IS NULL),
			'Placeholder for metrics that were at the root level in Arena',
			GETDATE(),
			GETDATE(),
			''
		)
	SELECT @RockId = SCOPE_IDENTITY()
END
GO


-------------------------------------------------------
--
-- Migrate a metric as a category.
--
IF OBJECT_ID('_Migrate_MigrateMetricCategory') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_MigrateMetricCategory AS SELECT 1')
GO
ALTER PROCEDURE _Migrate_MigrateMetricCategory
(
	@ArenaMetricId INT,
	@RockCategoryId INT OUTPUT
)
AS
BEGIN
	DECLARE @ArenaParentMetricId AS INT
	DECLARE @ParentCategoryId AS INT

	--
	-- Check for NULL Metric ID, this means we need the generic root category.
	--
	IF @ArenaMetricId IS NULL
	BEGIN
		EXEC _Migrate_GenericMetricCategory @RockId = @RockCategoryId OUTPUT
		RETURN
	END

	--
	-- If we already have created this category then we don't need to do anything.
	--
	SELECT @RockCategoryId = c.Id FROM Category AS c INNER JOIN EntityType AS et ON et.Id = c.EntityTypeId WHERE et.Name = 'Rock.Model.MetricCategory' AND c.ForeignId = @ArenaMetricId
	IF @RockCategoryId IS NOT NULL
		RETURN

	--
	-- Get or create the parent metric category.
	--
	SELECT @ArenaParentMetricId = parent_metric_id FROM ArenaDb.dbo.mtrc_metric WHERE metric_id = @ArenaMetricId
	IF @ArenaParentMetricId IS NULL
		SET @RockCategoryId = NULL
	ELSE
		EXEC _Migrate_MigrateMetricCategory @ArenaMetricId = @ArenaParentMetricId, @RockCategoryId = @ParentCategoryId OUTPUT

	INSERT INTO Category
		(IsSystem, ParentCategoryId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, [Name], IconCssClass, [Guid], [Order], [Description], CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, ForeignKey, HighlightColor, ForeignGuid, ForeignId)
		SELECT
			0,
			@ParentCategoryId,
			(SELECT Id FROM EntityType WHERE [Name] = 'Rock.Model.MetricCategory'),
			'',
			'',
			title,
			'',
			NEWID(),
			(SELECT COUNT(*) FROM Category WHERE EntityTypeId = (SELECT Id FROM EntityType WHERE [Name] = 'Rock.Model.MetricCategory') AND (ParentCategoryId = @ParentCategoryId OR (@ParentCategoryId IS NULL AND ParentCategoryId IS NULL))),
			[description],
			date_created,
			date_modified,
			dbo._Migrate_RockPersonAliasIdForUsername(created_by),
			dbo._Migrate_RockPersonAliasIdForUsername(modified_by),
			NULL,
			'',
			NULL,
			metric_id
			FROM ArenaDb.dbo.mtrc_metric
			WHERE metric_id = @ArenaMetricId
	SELECT @RockCategoryId = SCOPE_IDENTITY()
END
GO


---------------------------------------------------------------------
--
-- Migrate a metric as a metric.
--
IF OBJECT_ID('_Migrate_MigrateMetric') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_MigrateMetric AS SELECT 1')
GO
ALTER PROCEDURE _Migrate_MigrateMetric
(
	@ArenaMetricId INT,
	@RockId INT OUTPUT
)
AS
BEGIN
	DECLARE @RockCategoryId AS INT

	--
	-- Determine if this is a metric category or a metric.
	--
	IF (SELECT COUNT(*) FROM ArenaDb.dbo.mtrc_metric WHERE parent_metric_id = @ArenaMetricId) > 0
		EXEC _Migrate_MigrateMetricCategory @ArenaMetricId = @ArenaMetricId, @RockCategoryId = @RockCategoryId OUTPUT
	ELSE
	BEGIN
		DECLARE @ArenaParentMetricId AS INT
		DECLARE @ArenaStartDate AS DATETIME
		DECLARE @ArenaMetricFrequency AS INT
		DECLARE @ArenaSQL AS VARCHAR(1000)
		DECLARE @RockScheduleId AS INT
		DECLARE @RockSourceTypeId AS INT

		--
		-- If we already have created this metric then we don't need to do anything.
		--
		SELECT @RockId = Id FROM Metric WHERE ForeignId = @ArenaMetricId
		IF @RockId IS NOT NULL
			RETURN

		--
		-- Get some data about the metric to use in the next few queries.
		--
		SELECT @ArenaParentMetricId = parent_metric_id,
			@ArenaStartDate = collection_last_date,
			@ArenaMetricFrequency = collection_frequency,
			@ArenaSQL = collection_sql_statement
			FROM ArenaDb.dbo.mtrc_metric WHERE metric_id = @ArenaMetricId

		--
		-- Get or create the parent metric category.
		--
		EXEC _Migrate_MigrateMetricCategory @ArenaMetricId = @ArenaParentMetricId, @RockCategoryId = @RockCategoryId OUTPUT

		--
		-- Create a new schedule for this metric.
		--
		EXEC _Migrate_CreateRockSchedule @StartDate = @ArenaStartDate, @ArenaFrequency = @ArenaMetricFrequency, @RockId = @RockScheduleId OUTPUT

		INSERT INTO Metric
			(IsSystem, Title, Subtitle, [Description], IconCssClass, IsCumulative, SourceValueTypeId, SourceSql, XAxisLabel, YAxisLabel, ScheduleId, CreatedDateTime, ModifiedDateTime, CreatedbyPersonAliasId, ModifiedbyPersonAliasId, [Guid], ForeignId)
			SELECT
				0,
				title,
				'',
				'',
				'',
				0,
				CASE LEN(@ArenaSQL) WHEN 0 THEN (SELECT Id FROM DefinedValue WHERE [Guid] = '1D6511D6-B15D-4DED-B3C4-459CD2A7EC0E') ELSE (SELECT Id FROM DefinedValue WHERE [Guid] = '6A1E1A1B-A636-4E12-B90C-D7FD1BDAE764') END,
				CASE LEN(@ArenaSQL) WHEN 0 THEN '' ELSE '--' + @ArenaSQL END,
				'',
				series_caption,
				@RockScheduleId,
				date_created,
				date_modified,
				dbo._Migrate_RockPersonAliasIdForUsername(created_by),
				dbo._Migrate_RockPersonAliasIdForUsername(modified_by),
				NEWID(),
				metric_id
				FROM ArenaDb.dbo.mtrc_metric
				WHERE metric_id = @ArenaMetricId
		SELECT @RockId = SCOPE_IDENTITY()

		INSERT INTO MetricCategory
			(MetricId, CategoryId, [Order], [Guid], ForeignId)
			VALUES
			(
				@RockId,
				@RockCategoryId,
				(SELECT COUNT(*) FROM MetricCategory WHERE CategoryId = @RockCategoryId),
				NEWID(),
				@ArenaMetricId
			)

		IF LEN(@ArenaSQL) > 0
			PRINT 'Created Rock Metric ID ' + CAST(@RockId AS VARCHAR(10)) + ' from Arena Metric ID ' + CAST(@ArenaMetricId AS VARCHAR(10)) + ' with commented out SQL: ' + @ArenaSQL
	END
END
GO


---------------------------------------------------------------------
--
-- Migrate a metric as a metric.
--
IF OBJECT_ID('_Migrate_MigrateMetricValues') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_MigrateMetricValues AS SELECT 1')
GO
ALTER PROCEDURE _Migrate_MigrateMetricValues
(
	@ArenaMetricId INT
)
AS
BEGIN
	DECLARE @RockMetricId AS INT

	--
	-- Get the rock Metric ID and make sure we actually have one.
	--
	SELECT @RockMetricId = Id FROM Metric WHERE ForeignId = @ArenaMetricId
	IF @RockMetricId IS NULL
		RETURN

	INSERT INTO MetricValue
		(MetricValueType, XValue, YValue, [Order], MetricId, Note, MetricValueDateTime, CreatedDateTime, ModifiedDateTime, CreatedByPersonAliasId, ModifiedByPersonAliasId, [Guid], ForeignId)
		SELECT
			0,
			'',
			metric_value,
			0,
			@RockMetricId,
			ISNULL(note,''),
			collection_date,
			date_created,
			date_modified,
			dbo._Migrate_RockPersonAliasIdForUsername(created_by),
			dbo._Migrate_RockPersonAliasIdForUsername(modified_by),
			NEWID(),
			metric_item_id
			FROM ArenaDB.dbo.mtrc_metric_item
			WHERE metric_id = @ArenaMetricId
			  AND (SELECT COUNT(*) FROM MetricValue WHERE ForeignId = metric_item_id) = 0
			ORDER BY collection_date
END
GO


---------------------------------------------------------------------
--
-- Migrate a metric as a metric.
--
IF OBJECT_ID('_Migrate_MigrateBlob') IS NULL
	EXEC ('CREATE PROCEDURE _Migrate_MigrateBlob AS SELECT 1')
GO
ALTER PROCEDURE [dbo].[_Migrate_MigrateBlob]
-- Borrowed from Minecart's spArenaRockSyncBinaryFile
@BlobId int,
@BinaryFileTypeId int = null,
@StorageEntityTypeId int = null

AS 

IF @BinaryFileTypeId IS NULL
BEGIN
	-- Default File Type
	SET @BinaryFileTypeID = ( SELECT TOP 1 [Id] FROM [BinaryFileType] WHERE [Guid] = 'C1142570-8CD6-4A20-83B1-ACB47C1CD377' )
END

IF @StorageEntityTypeId IS NULL
BEGIN
	-- Database Storage Type
	SET @StorageEntityTypeId = ( SELECT TOP 1 [StorageEntityTypeId] FROM [BinaryFileType] WHERE [Guid] = '03BD8476-8A9F-4078-B628-5B538F967AFC' )
END

DECLARE @RockId int
DECLARE @FileExt varchar(50)
DECLARE @MimeType varchar(100)
DECLARE @OriginalFileName varchar(100)
DECLARE @Blob varbinary(max)
DECLARE @Description varchar(255)
DECLARE @CreatedDateTime datetime
DECLARE @ModifiedDateTime datetime

SELECT
	 @RockId = F.[Id]
	,@FileExt = B.[file_ext]
	,@MimeType = B.[mime_type]
	,@OriginalFileName = ISNULL( REPLACE(B.[original_file_name],',',''), '' )
	,@Blob = B.[blob]
	,@Description = B.[Description]
	,@CreatedDateTime = B.[date_created]
	,@ModifiedDateTime = B.[date_modified]
FROM ArenaDB.dbo.[util_blob] B WITH (NOLOCK)
LEFT OUTER JOIN Rock.dbo.[BinaryFile] F WITH (NOLOCK)
	ON F.[ForeignId] = B.[blob_id]
WHERE B.[blob_id] = @BlobId

IF @Blob IS NOT NULL
BEGIN

	IF @RockId IS NULL 
	BEGIN

		INSERT INTO Rock.dbo.[BinaryFile] (
			 [IsTemporary]
			,[IsSystem]
			,[BinaryFileTypeId]
			,[FileName]
			,[MimeType]
			,[Description]
			,[StorageEntityTypeId]
			,[Guid]
			,[CreatedDateTime]
			,[ModifiedDateTime]
			,[ForeignId]
		)
		VALUES (
			 0
			,0
			,@BinaryFileTypeID
			,@OriginalFileName
			,@MimeType
			,@Description
			,@StorageEntityTypeId
			,NEWID()
			,@CreatedDateTime
			,@ModifiedDateTime
			,@BlobId
		)

		SET @RockId = ( SELECT TOP 1 [Id] FROM Rock.dbo.[BinaryFile] WITH (NOLOCK) WHERE [ForeignId] = @BlobId )

	END
	ELSE
	BEGIN

		UPDATE Rock.dbo.[BinaryFile] SET
			 [FileName] = @OriginalFileName
			,[MimeType] = @MimeType
			,[Description] = @Description
			,[ModifiedDateTime] = @ModifiedDateTime
		WHERE [Id] = @RockId

	END

	IF NOT EXISTS ( SELECT [Id] FROM Rock.dbo.[BinaryFileData] WITH (NOLOCK) WHERE [Id] = @RockId )
	BEGIN

		INSERT INTO Rock.dbo.[BinaryFileData] (
			 [Id]
			,[Content]
			,[Guid]
		)
		VALUES (
			 @RockId
			,@Blob
			,NEWID()
		)

	END
	ELSE
	BEGIN

		UPDATE Rock.dbo.[BinaryFileData] SET [Content] = @Blob
		WHERE [Id] = @RockId

	END

END
