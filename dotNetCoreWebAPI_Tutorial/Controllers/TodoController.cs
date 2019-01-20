using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNetCoreWebAPI_Tutorial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNetCoreWebAPI_Tutorial.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase {
        private readonly TodoContext _context;

        //DbContext
        public TodoController(TodoContext context) {
            _context = context;

            if (_context.TodoItems.Count() == 0) {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }
        
        //タスクのリスト呼び出し
        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() {
            return await _context.TodoItems.ToListAsync();
        }

        //タスク毎の呼び出し
        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id) {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null) {
                return NotFound();
            }

            return todoItem;
        }

        //新規登録
        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem) {
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }
    }
}
