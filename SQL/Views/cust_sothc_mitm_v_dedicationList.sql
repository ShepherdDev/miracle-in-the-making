SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'cust_sothc_mitm_v_dedicationList')
    DROP VIEW [cust_sothc_mitm_v_dedicationList]
GO

CREATE VIEW cust_sothc_mitm_v_dedicationList
AS
	SELECT
		csm_d.*,
        CASE
            WHEN LEN(approved_by) = 0 THEN 0
            ELSE 1
        END AS 'is_approved',
		cp.[guid] AS 'person_guid',
		cp.first_name AS 'first_name',
		cp.nick_name AS 'nick_name',
		cp.last_name AS 'last_name',
		CASE
			WHEN csm_sp.assigned_seat_id IS NULL THEN ''
			ELSE csm_aseat.section + CAST(csm_aseat.seat_number AS VARCHAR(5))
		END AS 'assigned_seat'
		FROM cust_sothc_mitm_dedication AS csm_d
        LEFT OUTER JOIN cust_sothc_mitm_seat_pledge AS csm_sp ON csm_sp.seat_pledge_id = csm_d.seat_pledge_id
		LEFT OUTER JOIN cust_sothc_mitm_seat AS csm_aseat ON csm_aseat.seat_id = csm_sp.assigned_seat_id
		LEFT JOIN core_person AS cp ON cp.person_id = csm_sp.person_id
