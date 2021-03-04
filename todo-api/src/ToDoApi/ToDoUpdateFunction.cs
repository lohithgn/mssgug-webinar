using System;
using System.Threading;
using System.Threading.Tasks;
using Contoso.Api.Models;
using Contoso.ToDo.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Contoso.ToDo.Api
{
    public class ToDoUpdateFunction
    {
        private readonly IToDoService _service;

        public ToDoUpdateFunction(IToDoService service)
        {
            _service = service;
        }

        /// <summary> Update an existing ToDoItem. </summary>
        /// <param name="todoId"> ID of ToDoItem to return. </param>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="body"> ToDoItem object that needs to be updated. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoUpdateFunction))]
        public async Task<IActionResult> UpdateAsync(
            HttpRequest req, 
            string todoId, 
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos/{todoId}")] ToDoItem todo = null,
            CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(todo.Id)) return new BadRequestObjectResult("Invalid payload.");

            try
            {
                await _service.UpdateTodoAsync(todo);
                return new OkResult();
            }
            catch(InvalidOperationException)
            {
                return new NotFoundObjectResult($"ToDo with identifier:{todoId} not found");
            }
        }
    }
}
