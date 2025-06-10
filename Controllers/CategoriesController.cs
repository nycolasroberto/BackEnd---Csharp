using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameCatalog.Data;
using GameCatalog.Models;

namespace GameCatalog.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly GameContext _context;

        public CategoriesController(GameContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                var categories = await _context.Categories
                    .Include(c => c.Games) 
                    .ToListAsync();
                
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Games) // Inclui os jogos relacionados
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound($"Categoria com ID {id} não encontrada");
                }

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest("ID da categoria não confere");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound($"Categoria com ID {id} não encontrada");
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCategory", new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Games)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound($"Categoria com ID {id} não encontrada");
                }

                
                if (category.Games.Any())
                {
                    return BadRequest($"Não é possível excluir a categoria '{category.Nome}' pois ela possui {category.Games.Count} jogo(s) associado(s)");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}