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
    public class ToDoReadFunction
    {
        private readonly ILogger<ToDoReadFunction> _logger;

        public ToDoReadFunction(ILogger<ToDoReadFunction> logger)
        {
            _logger = logger;
        }

        /// <summary> Get an existing ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="todoId"> ID of ToDoItem to return. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoReadFunction))]
        public async Task<IActionResult> ReadAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{todoId}")] HttpRequest req, 
            long todoId, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 200
            // Spec Defines: HTTP 400
            // Spec Defines: HTTP 404

            throw new NotImplementedException();
        }
    }
}
