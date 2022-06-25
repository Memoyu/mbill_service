namespace mbill.Core.AOP.Filters;

public class InputFieldActionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //throw new NotImplementedException();
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        ServiceResult result = new();
        if (!context.ModelState.IsValid)
        {
            var errorMessage = context.ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();
            result.Code = ServiceResultCode.ParameterError;
            result.Message = errorMessage.ErrorMessage;
            var set = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(result, set),
                StatusCode = StatusCodes.Status400BadRequest,
                ContentType = "application/json",
            };
        }
    }
}
