using System;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Api.Models;
using Contoso.ToDo.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Contoso.ToDo.Api.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoDBContext _dbContext;

        public ToDoService(ToDoDBContext dbContext)
        {
            
            _dbContext = dbContext;
        }

        public async Task<ToDoItem[]> ListTodosAsync(string user)
        {
            var results = await _dbContext.ToDos
                            .Where(item => item.User == user)
                            .ToArrayAsync();            
            return results;
        }

        public async Task CreateTodoAsync(ToDoItem newItem)
        {
            newItem.Id = Guid.NewGuid().ToString();
            await _dbContext.AddAsync(newItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ToDoItem> ReadTodoAsync(string identifier)
        {
            var todo = await _dbContext.ToDos.Where(item => item.Id == identifier).FirstOrDefaultAsync();
            if(todo == null) throw new InvalidOperationException("Todo not found.");
            return todo;
        }

        public async Task UpdateTodoAsync(ToDoItem updatedItem)
        {
            var todo = await _dbContext.ToDos.Where(item => item.Id == updatedItem.Id).FirstOrDefaultAsync();
            if(todo == null) throw new InvalidOperationException("Todo not found.");
            todo.Text = updatedItem.Text;
            if(updatedItem.IsCompleted.HasValue)
            { 
                todo.IsCompleted = updatedItem.IsCompleted;
            }
            if(todo.IsCompleted.Value)
            {
                todo.Completed = DateTime.UtcNow;
            }
            _dbContext.Update(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteTodoAsync(string identifier)
        {
            var existingTodo = await _dbContext.ToDos.Where(item => item.Id == identifier).FirstOrDefaultAsync();
            if(existingTodo == null) throw new InvalidOperationException("Todo not found.");
            _dbContext.Remove(existingTodo);
            await _dbContext.SaveChangesAsync();
        }

        

        
    }
}
