namespace MyWebAPI.Models {
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.AspNetCore.Mvc;

    public class MyCustomStatusCode : OkObjectResult {
        private readonly string _myCustomResponseHeader;

        public const int ResponseCode = StatusCodes.Status200OK;

        public MyCustomStatusCode (string myCustomResponseHeader) 
            : base(ResponseCode) {
            _myCustomResponseHeader = myCustomResponseHeader;
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            BuildResult(context);
            return base.ExecuteResultAsync(context);
        }

        public override void ExecuteResult(ActionContext context) 
        {
            base.ExecuteResult(context);

            BuildResult(context);
        }

        private void BuildResult(ActionContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "KamCode";

            response.Headers.Add("My-Custom-Response", _myCustomResponseHeader.ToString());

            response.Headers.Add("X-response-from-status", "123");
        }
  }
}