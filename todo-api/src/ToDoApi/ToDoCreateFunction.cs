using Contoso.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Contoso.ToDo.Api
{
    public class ToDoCreateFunction
    {
        private readonly ILogger<ToDoCreateFunction> _logger;

        public ToDoCreateFunction(ILogger<ToDoCreateFunction> logger)
        {
            _logger = logger;
        }

        /// <summary> Add a new ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="body"> ToDoItem object that needs to be added. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoCreateFunction))]
        public async Task<IActionResult> CreateAsync(HttpRequest req, 
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] ToDoItem body = null, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 201
            // Spec Defines: HTTP 405

            throw new NotImplementedException();
        }
    }
}
