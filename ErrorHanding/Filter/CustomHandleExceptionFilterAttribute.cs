using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ErrorHanding.Filter
{
    public class CustomHandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        // hangi error sayfasına gideceğini belirlemek için ErrorPage adlı property oluşturulur.
        public string ErrorPage { get; set; }

        public override void OnException(ExceptionContext context)
        {
            // sayfa aşağıdaki gibi belirtilir.

            var result = new ViewResult() { ViewName = ErrorPage };

            result.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);

            // URL erişimi yapmak için aşağıdaki response ile erişim yapılmaktadır.
            result.ViewData.Add("Url",context.HttpContext.Request.Path.Value);
            result.ViewData.Add("Exception", context.Exception);

            context.Result = result;
        }
    }
}
