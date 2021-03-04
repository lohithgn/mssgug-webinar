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
    public class ToDoCreateFunction
    {
        private readonly IToDoService _toDoService;

        public ToDoCreateFunction(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        /// <summary> Add a new ToDoItem. </summary>
        /// <param name="req"> Raw HTTP Request. </param>
        /// <param name="body"> ToDoItem object that needs to be added. </param>
        /// <param name="cancellationToken"> The cancellation token provided on Function shutdown. </param>
        [FunctionName(nameof(ToDoCreateFunction))]
        public async Task<IActionResult> CreateAsync(HttpRequest req, 
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todos")] ToDoItem newToDo = null, 
            CancellationToken cancellationToken = default)
        {
            if(string.IsNullOrEmpty(newToDo.User)) 
            {
                return new BadRequestObjectResult("No payload provided.");
            }
            if(string.IsNullOrEmpty(newToDo.Text))
            {
                newToDo.Text = "New Task";
            }

            await _toDoService.CreateTodoAsync(newToDo);

            return new CreatedResult($"/api/todos/{newToDo.Id}",newToDo);
        }
    }
}
