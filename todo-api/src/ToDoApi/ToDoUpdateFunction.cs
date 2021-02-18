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
    public class ToDoUpdateFunction
    {
        private readonly ILogger<ToDoUpdateFunction> _logger;

        public ToDoUpdateFunction(ILogger<ToDoUpdateFunction> logger)
        {
            _logger = logger;
        }

        /// <summary> Update an existing ToDoItem. </summary>
        /// <param name="todoId"> ID of ToDoItem to return. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="body"> ToDoItem object that needs to be updated. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoUpdateFunction))]
        public async Task<IActionResult> UpdateAsync(long todoId, 
            HttpRequest req, 
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos/{todoId}")] ToDoItem body = null, 
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("HTTP trigger function processed a request.");

            // TODO: Handle Documented Responses.
            // Spec Defines: HTTP 200
            // Spec Defines: HTTP 400
            // Spec Defines: HTTP 405

            throw new NotImplementedException();
        }
    }
}
