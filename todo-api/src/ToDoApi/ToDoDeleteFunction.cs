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
    public class ToDoDeleteFunction
    {
        private readonly ILogger<ToDoDeleteFunction> _logger;

        public ToDoDeleteFunction(ILogger<ToDoDeleteFunction> logger)
        {
            _logger = logger;
        }

        /// <summary> Delete an existing ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="todoId"> ID of ToDoItem to delete. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoDeleteFunction))]
        public async Task<IActionResult> DeleteAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{todoId}")] HttpRequest req, 
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
