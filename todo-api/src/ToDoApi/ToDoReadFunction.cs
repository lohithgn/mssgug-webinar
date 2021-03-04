using Contoso.ToDo.Api.Services;
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
        private readonly IToDoService _service;

        public ToDoReadFunction(IToDoService service)
        {
            _service = service;
        }

        /// <summary> Get an existing ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="todoId"> ID of ToDoItem to return. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoReadFunction))]
        public async Task<IActionResult> ReadAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos/{todoId}")] HttpRequest req, 
            string todoId, 
            CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(todoId)) return new BadRequestObjectResult("ToDo identifier is required.");

            try
            {
                var todo = await _service.ReadTodoAsync(todoId);
                return new OkObjectResult(todo);
            }
            catch(InvalidOperationException)
            {
                return new NotFoundObjectResult($"ToDo with identifier:{todoId} not found");
            }
        }
    }
}
