using System;
using B2BPortal.Data;
using B2BPortal.Infrastructure.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using B2BPortal.Infrastructure;
using System.IdentityModel.Claims;
using AzureB2BInvite.Models;
using AzureB2BInvite.Rules;
using B2BPortal.Common.Enums;
using B2BPortal.Common.Utils;
using AzureB2BInvite;

namespace B2BPortal.Areas.Admin.Controllers
{
    [AuthorizedInviter]
    public class HomeController : Controller
    {
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            if (!Settings.SiteConfigReady)
            {
                return RedirectToAction("Index", new { controller = "SiteConfig", action = "Index", area = "Admin" });
            }

            IEnumerable<GuestRequest> pendingRequests = await DocDBRepo.DB<GuestRequest>.GetItemsAsync(i => i.Disposition == Disposition.Pending);
            
            return View(pendingRequests);
        }

        [HttpPost]
        public async Task<ActionResult> Approve()
        {
            var approveCount = 0;
            var deniedCount = 0;
            var sentCount = 0;
            var requestList = await GuestRequestRules.GetPendingRequestsAsync();

            foreach (var request in requestList)
            {
                var strDisposition = Request.Form[string.Format("Disposition.{0}", request.Id)];
                var disposition = (Disposition)Enum.Parse(typeof(Disposition), strDisposition);
                switch (disposition)
                { 
                    case Disposition.Pending:
                        break;
                    case Disposition.Approved:
                        approveCount++;
                        break;
                    case Disposition.Denied:
                        deniedCount++;
                        break;
                }
                request.Disposition = disposition;
                
                request.InternalComment = Request.Form[string.Format("InternalComment.{0}", request.Id)];
                var domain = await GuestRequestRules.GetMatchedDomain(request.EmailAddress);
                var res = await GuestRequestRules.ExecuteDispositionAsync(request, User.Identity.Name, Utils.GetProfileUrl(Request.Url), domain);

                if (res !=null &&  ((res.InvitationResult?.SendInvitationMessage ?? false) || res.MailSent))
                {
                    sentCount++;
                }
            }

            requestList = await GuestRequestRules.GetPendingRequestsAsync();

            ViewBag.Message = string.Format("{0} {1} approved, {2} {3} denied, {4} {5} invitations sent.", approveCount, Utils.Pluralize(approveCount, "request"), deniedCount, Utils.Pluralize(deniedCount, "request"), sentCount, Utils.Pluralize(sentCount, "invitation"));
            return View("Index", requestList);
        }
    }
}