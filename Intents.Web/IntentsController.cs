using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Intents.Models;

namespace Intents.Web
{
    public class IntentsController: Controller
    {
        public ActionResult Subscribe(string trigger, string name, string data, string redirectUri = null)
        {
            try
            {
                IntentManager.GetIntentManager().AddIntentData(trigger, name, data);
                if (!String.IsNullOrEmpty(redirectUri)) return Redirect(redirectUri);
                if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.ToString());
                return Json(Models.Response.GetDefaultSuccessMessage(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(Models.Response.GetDefaultErrorMessage(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Trigger(string trigger)
        {
            try
            {
                if (!String.IsNullOrEmpty(trigger))
                {
                    if (IntentManager.GetIntentManager().TriggerExists(trigger))
                    {
                        var intents = IntentManager.GetIntentManager().GetUserIntentData(trigger);
                        IntentManager.GetIntentManager().Trigger(trigger);
                        return Json(Models.Response<dynamic>.Success(intents.Select(intent =>
                        {
                            return new
                            {
                                name = intent.name,
                                trigger = intent.trigger,
                                action = intent.action.ToString() //because we can't serialize entire Actions (methods), nor should we attempt to
                            };
                        })), JsonRequestBehavior.AllowGet);
                    }
                    else return Json(Models.Response.GetDefaultErrorMessage("Invalid Trigger: Trigger not found"), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(Models.Response.GetDefaultErrorMessage("Invalid Trigger: Empty String"), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(Models.Response.GetDefaultErrorMessage(ex.Message), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetIntents()
        {
            return Json(Models.Response<dynamic>.Success(IntentManager.GetIntentManager().GetIntents().Select(intent =>
            {
                return new
                {
                    name = intent.name,
                    trigger = intent.trigger,
                    action = intent.action.ToString() //because we can't serialize entire Actions (methods), nor should we attempt to
                };
            })), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserIntents(string trigger = null)
        {
            return Json(Models.Response<dynamic>.Success(IntentManager.GetIntentManager().GetUserIntentData(trigger).Select(intent =>
            {
                return new
                {
                    name = intent.name,
                    trigger = intent.trigger,
                    data = intent.data,
                    action = intent.action.ToString() //because we can't serialize entire Actions (methods), nor should we attempt to
                };
            })), JsonRequestBehavior.AllowGet);
        }
    }
}
