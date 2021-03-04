using System;
using System.Threading;
using System.Threading.Tasks;
using Contoso.ToDo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Contoso.ToDo.Api
{
    public class ToDoDeleteFunction
    {
        private readonly IToDoService _service;

        public ToDoDeleteFunction(IToDoService service)
        {
            _service = service;
        }

        /// <summary> Delete an existing ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="todoId"> ID of ToDoItem to delete. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoDeleteFunction))]
        public async Task<IActionResult> DeleteAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{todoId}")] HttpRequest req, 
            string todoId, 
            CancellationToken cancellationToken = default)
        {

            if(string.IsNullOrEmpty(todoId)) return new BadRequestObjectResult("ToDo identifier is required.");

            try
            {
                await _service.DeleteTodoAsync(todoId);
                return new OkResult();
            }
            catch(InvalidOperationException)
            {
                return new NotFoundObjectResult($"ToDo with identifier:{todoId} not found");
            }
        }
    }
}
