SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'cust_sothc_mitm_v_seatPledgeList')
    DROP VIEW [cust_sothc_mitm_v_seatPledgeList]
GO

CREATE VIEW cust_sothc_mitm_v_seatPledgeList
AS
	SELECT
		csm_sp.*,
		csm_aseat.section AS 'assigned_section',
		csm_aseat.seat_number AS 'assigned_seat_number',
		csm_rseat.section AS 'requested_section',
		csm_rseat.seat_number AS 'requested_seat_number',
		cp.[guid] AS 'person_guid',
		cp.first_name AS 'first_name',
		cp.nick_name AS 'nick_name',
		cp.last_name AS 'last_name',
		CASE
			WHEN csm_sp.assigned_seat_id IS NULL THEN ''
			ELSE csm_aseat.section + CAST(csm_aseat.seat_number AS VARCHAR(5))
		END AS 'assigned_seat',
		CASE
			WHEN csm_sp.requested_seat_id IS NULL THEN ''
			ELSE csm_rseat.section + CAST(csm_rseat.seat_number AS VARCHAR(5))
		END AS 'requested_seat',
		ISNULL(ctrb_p.amount, 0) AS 'pledge_amount',
		(SELECT SUM(amount) FROM cust_sothc_mitm_seat_pledge AS total_sp WHERE total_sp.person_id = csm_sp.person_id) AS 'seat_total',
		CASE
			WHEN (SELECT SUM(amount) FROM cust_sothc_mitm_seat_pledge AS total_sp WHERE total_sp.person_id = csm_sp.person_id) = ISNULL(ctrb_p.amount, 0) THEN 1
			ELSE 0
		END AS 'is_balanced',
        ctrb_p.fund_id AS 'fund_id'
		FROM cust_sothc_mitm_seat_pledge AS csm_sp
		LEFT OUTER JOIN cust_sothc_mitm_seat AS csm_aseat ON csm_aseat.seat_id = csm_sp.assigned_seat_id
		LEFT OUTER JOIN cust_sothc_mitm_seat AS csm_rseat ON csm_rseat.seat_id = csm_sp.requested_seat_id
		LEFT JOIN core_person AS cp ON cp.person_id = csm_sp.person_id
		LEFT OUTER JOIN ctrb_pledge AS ctrb_p ON ctrb_p.person_id = csm_sp.person_id AND ctrb_p.fund_id = 49
