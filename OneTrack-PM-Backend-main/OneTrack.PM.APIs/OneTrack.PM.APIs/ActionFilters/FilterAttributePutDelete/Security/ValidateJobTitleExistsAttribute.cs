using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneTrack.PM.APIs.Errors;
using OneTrack.PM.Core;
using OneTrack.PM.Core.Services;
using OneTrack.PM.Entities.Models.DB;
using OneTrack.PM.Repositories.Specifications.Security;
using System.Threading.Tasks;

namespace OneTrack.PM.APIs.ActionFilters.FilterAttributePutDelete.Security
{
    public class ValidateJobTitleExistsAttribute : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerService _logger;
        public ValidateJobTitleExistsAttribute(IUnitOfWork unitOfWork, ILoggerService logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT") ? true : false;
            var id = (int)context.ActionArguments["id"];
            var entity = await _unitOfWork.Repository<SecJobTitle>().GetWithSpecAsync(new JobTitlesSpecifications(id), trackChanges);
            if (entity == null)
            {
                string msg = $"JobTitle with id: {id} doesn't exist in the database.";
                _logger.LogInfo(msg);
                context.Result = new NotFoundObjectResult(new ApiResponse(404, msg));
            }
            else
            {
                context.HttpContext.Items.Add("JobTitle", entity);
                await next();
            }
        }
    }
}
