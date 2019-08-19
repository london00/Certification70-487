using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http.Filters;

namespace WebApiFromScratch.Filters
{
    public class CustomFilter: ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Debug.WriteLine(actionExecutedContext.ActionContext.ActionDescriptor.ActionName + " has been executed" );
            base.OnActionExecuted(actionExecutedContext);
        }
    }
}