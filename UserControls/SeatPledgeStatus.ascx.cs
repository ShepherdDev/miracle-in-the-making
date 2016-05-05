using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Arena.Core;
using Arena.Contributions;
using Arena.Portal;
using Arena.Custom.SOTHC.MiTM;

namespace ArenaWeb.UserControls.Custom.SOTHC.MiTM
{
    public partial class SeatPledgeStatus : PortalControl
    {
        #region Private Properties

        private SeatPledge _seatPledge;
        protected String fillAmount = "0.00";

        #endregion

        
        #region Module Settings

        [PageSetting("Give Now Page", "The page that will be used for the pay remaining balance/give now button.", true)]
        public string GiveNowPageSetting { get { return Setting("GiveNowPage", "", true); } }

        [PageSetting("Dedication Editor", "The page that will be used for editing seat dedications.", true)]
        public string DedicationEditorSetting { get { return Setting("DedicationEditor", "", true); } }

        [PageSetting("Adopt Full Seat", "The page that will be used for adopting full seat.", true)]
        public string AdoptFullSetting { get { return Setting("AdoptFull", "", true); } }

        #endregion


        /// <summary>
        /// Initialize variables that will be needed later during the lifespan of this module.
        /// </summary>
        protected void Page_Init(object sender, EventArgs e)
        {
            //
            // We only reference the _seatPledge so just set it to null if we were
            // told to reference one that we can't find.
            //
            if (!String.IsNullOrEmpty(Request.QueryString["seat_pledge_id"]))
            {
                _seatPledge = new SeatPledge(Convert.ToInt32(Request.QueryString["seat_pledge_id"]));
                if (_seatPledge.SeatPledgeID == -1)
                    _seatPledge = null;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Decimal pledgeTotal = 0, pledgeGiven = 0, percentage = 0;

                //
                // Verify that we got a valid seat pledge to work with.
                //
                if (_seatPledge == null)
                {
                    throw new System.Exception("Seat pledge must be specified.");
                }

                //
                // Verify the user has permission to edit the current dedication.
                //
                if (_seatPledge.PersonID != -1 && (CurrentArenaContext.Person == null || _seatPledge.PersonID != CurrentArenaContext.Person.PersonID))
                {
                    throw new System.Exception("Attempt to edit dedication for seat not owned by current user.");
                }

                //
                // Determine how much this person has pledged. If no pledge data is found then assume they
                // have pledged a total of how much they pledged towards this seat.
                //
                SeatPledgeCollection spc = new SeatPledgeCollection();
                spc.LoadByPerson(CurrentArenaContext.Person.PersonID);
                foreach (SeatPledge sp in spc)
                {
                    pledgeTotal += sp.Amount;
                }

                //
                // Determine how much this person has actually contributed towards this pledge.
                //
                ContributionCollection cc = new ContributionCollection();
                cc.LoadByPersonId(CurrentArenaContext.Person.PersonID);
                foreach (Contribution c in cc)
                {
                    foreach (ContributionFund cf in c.ContributionFunds)
                    {
                        if (cf.FundId == SeatPledge.PledgeFundID)
                            pledgeGiven += cf.Amount;
                    }
                }
                //
                // Add in all the money contributed by family members, but only if this person
                // is not marked as contribute individually and also only if the other member(s)
                // are also not marked to contribute individually.
                //
                if (CurrentArenaContext.Person.ContributeIndividually == false)
                {
                    Family f = CurrentArenaContext.Person.Family();
                    int n;

                    for (n = 0; n < f.FamilyMembers.Count; n++)
                    {
                        if (f.FamilyMembers[n].PersonID == CurrentArenaContext.Person.PersonID || f.FamilyMembers[n].ContributeIndividually == true)
                            continue;

                        cc = new ContributionCollection();
                        cc.LoadByPersonId(f.FamilyMembers[n].PersonID);
                        foreach (Contribution c in cc)
                        {
                            foreach (ContributionFund cf in c.ContributionFunds)
                            {
                                if (cf.FundId == SeatPledge.PledgeFundID)
                                    pledgeGiven += cf.Amount;
                            }
                        }
                    }
                }

                //
                // Now process the actual values for this seat. Work as a percentage of the total.
                //
                percentage = (pledgeGiven / pledgeTotal);
                fillAmount = (pledgeGiven / (Decimal)10000).ToString("0.00");
                lbPaidByMe.Text = String.Format("Paid by me ({0})", (_seatPledge.Amount * percentage).ToString("C"));
                lbRemainingForMe.Text = String.Format("Remaining for me ({0})", (_seatPledge.Amount - (_seatPledge.Amount * percentage)).ToString("C"));

                //
                // Process how much has been pledged by other people.
                //
                spc = new SeatPledgeCollection();
                spc.LoadByAssignedSeat(_seatPledge.AssignedSeatID);
                pledgeTotal = 0;
                foreach (SeatPledge sp in spc)
                {
                    if (sp.PersonID != CurrentArenaContext.Person.PersonID)
                        pledgeTotal += sp.Amount;
                }
                lbPledgedByOthers.Text = String.Format("Pledged by others ({0})", pledgeTotal.ToString("C"));

                //
                // Determine how much still needs to be pledged now.
                //
                pledgeTotal = 0;
                foreach (SeatPledge sp in spc)
                {
                    pledgeTotal += sp.Amount;
                }
                if (pledgeTotal > 10000)
                    pledgeTotal = 10000;
                lbNeedAdopted.Text = String.Format("Needs to be adopted ({0})", (10000 - pledgeTotal).ToString("C"));
                
                //
                // Update Links.
                //
                hlPayLink.NavigateUrl = String.Format("~/default.aspx?page={0}&redirect={1}",
                		GiveNowPageSetting,
                		HttpUtility.UrlEncode(String.Format("default.aspx?page={0}&seat_pledge_id={1}",
                				CurrentPortalPage.PortalPageID, _seatPledge.SeatPledgeID)));
                hlDedicate.NavigateUrl = String.Format("~/default.aspx?page={0}&seat_pledge_id={1}&redirect={2}",
                		DedicationEditorSetting,
                		_seatPledge.SeatPledgeID,
                		HttpUtility.UrlEncode(String.Format("default.aspx?page={0}&seat_pledge_id={1}",
                				CurrentPortalPage.PortalPageID, _seatPledge.SeatPledgeID)));
                hlAdoptFull.NavigateUrl = String.Format("~/default.aspx?page={0}&seat_pledge_id={1}&redirect={2}",
                		AdoptFullSetting,
                		_seatPledge.SeatPledgeID,
                		HttpUtility.UrlEncode(String.Format("default.aspx?page={0}&seat_pledge_id={1}",
                				CurrentPortalPage.PortalPageID, _seatPledge.SeatPledgeID)));
            }
        }
    }
}