using BruteWall.Entities;
using BruteWall.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace BruteWall.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class BruteProtect : ActionFilterAttribute
    {
        private readonly string _replacePattern = ":secunds";
        private readonly ushort _statusCode;
        private readonly string _notConfiguredMessage = "BruteWall Service not configured.";

        public BruteProtect() {
            this._statusCode = 0;
        }

        public BruteProtect(ushort statusCode)
        {
            this._statusCode = statusCode;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            IBackendService service = (IBackendService) context.HttpContext.RequestServices.GetService(typeof(IBackendService));
            if (service == null)
            {
                throw new ArgumentNullException(this._notConfiguredMessage);
            }

            string remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
            service.EraseSender(remoteIpAddress);

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            
            IBackendService service = (IBackendService) context.HttpContext.RequestServices.GetService(typeof(IBackendService));
            if (service == null)
            {
                throw new ArgumentNullException(this._notConfiguredMessage);
            }

            ushort usedStatusCode = this._statusCode == 0 ? service._statusCode : this._statusCode;

            if (context.HttpContext.Response.StatusCode == usedStatusCode)
            {
                string remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                BlockedAddress entity = service.RegisterSender(remoteIpAddress);

                if (entity.IsLocked)
                {
                    ObjectResult objectResult = new ObjectResult(new BruteError()
                    {
                        StatusCode = service._error.StatusCode,
                        Error = service._error.Error,
                        Message = new string(service._error.Message).Replace(this._replacePattern, entity.DurationInSecunds.ToString())
                    });
                    objectResult.StatusCode = service._error.StatusCode;
                    context.Result = objectResult;
                }
            }
        }
    }
}
