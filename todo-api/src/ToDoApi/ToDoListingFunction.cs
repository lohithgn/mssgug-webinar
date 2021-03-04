using Contoso.ToDo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contoso.ToDo.Api
{
    public class ToDoListingFunction
    {
        private readonly IConfiguration _configuration;
        private readonly IToDoService _toDoService;

        public ToDoListingFunction(IConfiguration configuration, IToDoService toDoService)
        {
            _configuration = configuration;
            _toDoService = toDoService;
        }

        /// <summary> Get all ToDos for a user. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoListingFunction))]
        public async Task<IActionResult> ListAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{user}/todos")] HttpRequest req, 
            string user,
            CancellationToken cancellationToken = default)
        {
            return new OkObjectResult(await _toDoService.ListTodosAsync(user));
        }
    }
}
