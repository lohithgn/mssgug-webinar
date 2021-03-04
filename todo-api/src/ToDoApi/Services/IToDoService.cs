using Contoso.Api.Models;
using System.Threading.Tasks;

namespace Contoso.ToDo.Api.Services
{
    public interface IToDoService
    {
        /// <summary>
        /// Lists all ToDos for a user
        /// </summary>
        /// <param name="user">user email address</param>
        /// <returns>Array for <see cref="ToDoItem"/></returns>
        Task<ToDoItem[]> ListTodosAsync(string user);

        /// <summary>
        /// Creates a new ToDo
        /// </summary>
        /// <param name="newItem">New ToDo to create</param>
        Task CreateTodoAsync(ToDoItem newItem);
        
        /// <summary>
        /// Get ToDo details
        /// </summary>
        /// <param name="identifier">Identifier of ToDo to get</param>
        /// <returns>Instance of <see cref="ToDoItem"/></returns>
        Task<ToDoItem> ReadTodoAsync(string identifier);
        
        /// <summary>
        /// Updates a ToDo details
        /// </summary>
        /// <param name="updatedItem">ToDo to Update</param>
        /// <returns></returns>
        Task UpdateTodoAsync(ToDoItem updatedItem);
        
        /// <summary>
        /// Deletes a ToDo
        /// </summary>
        /// <param name="identifier">identifier of ToDo to delete</param>
        /// <returns></returns>
        Task DeleteTodoAsync(string identifier);
    }
}
