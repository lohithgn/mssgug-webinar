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
    public class ToDoListingFunction
    {
        private readonly ILogger<ToDoListingFunction> _logger;

        public ToDoListingFunction(ILogger<ToDoListingFunction> logger)
        {
            _logger = logger;
        }

        /// <summary> Get all ToDos for a user. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoListingFunction))]
        public async Task<IActionResult> ListAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequest req, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 200

            throw new NotImplementedException();
        }
    }
}
