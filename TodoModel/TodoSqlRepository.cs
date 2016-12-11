using System;
using System.Collections.Generic;
using System.Linq;

namespace Interfaces
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public void Add(TodoItem todoItem)
        {
            if(Get(todoItem.Id, todoItem.UserId) != null)
            {
                throw new DuplicateTodoItemException(String.Format("duplicate id: {0}",todoItem.Id));
            }
            _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            var todoItem = _context.TodoItems.Where(s => s.Id == todoId).FirstOrDefault();
            if(todoItem != null && todoItem.UserId != userId)
            {
                throw new TodoAccessDeniedException("todoItem is not available for this user");
            }
            return todoItem;
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(s => s.UserId == userId && s.IsCompleted == false).ToList();
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(s => s.UserId == userId).OrderByDescending(s => s.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(s => s.UserId == userId && s.IsCompleted == true).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(s => s.UserId == userId && filterFunction(s)).ToList();
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);
            if(todoItem == null)
            {
                return false;
            }
            todoItem.IsCompleted = true;
            _context.SaveChanges();
            return true;
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            var todoItem = Get(todoId, userId);
            if(todoItem == null)
            {
                return false;
            }
            _context.TodoItems.Remove(todoItem);
            _context.SaveChanges();
            return true;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            var existingItem = Get(todoItem.Id, userId);
            if(existingItem == null)
            {
                Add(todoItem);
                return;
            }
            existingItem.DateCompleted = todoItem.DateCompleted;
            existingItem.DateCreated = todoItem.DateCreated;
            existingItem.IsCompleted = todoItem.IsCompleted;
            existingItem.Text = todoItem.Text;
            _context.SaveChanges();
        }
    }
}
