 using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameCatalog.Data;
using GameCatalog.Models;

namespace GameCatalog.Controllers
{
    // Controller para gerenciar as operações CRUD de jogos
    // Implementa endpoints para Game
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameContext _context;

        public GamesController(GameContext context)
        {
            _context = context;
        }

        // Retorna todos os jogos com suas categorias
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            try
            {
                var games = await _context.Games
                    .Include(g => g.Categoria) // Inclui a categoria relacionada
                    .ToListAsync();

                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Retorna um jogo específico por ID
        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            try
            {
                var game = await _context.Games
                    .Include(g => g.Categoria) // Inclui a categoria relacionada
                    .FirstOrDefaultAsync(g => g.Id == id);

                if (game == null)
                {
                    return NotFound($"Jogo com ID {id} não encontrado");
                }

                return Ok(game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Busca jogos por nome
        // GET: api/Games/search?nome=zelda
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Game>>> SearchGames([FromQuery] string nome)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    return BadRequest("O parâmetro 'nome' é obrigatório para a busca");
                }

                var games = await _context.Games
                    .Include(g => g.Categoria)
                    .Where(g => g.Nome.ToLower().Contains(nome.ToLower()))
                    .ToListAsync();

                return Ok(games);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Atualiza um jogo existente
        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest("ID do jogo não confere");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificação da categoria
            if (!await _context.Categories.AnyAsync(c => c.Id == game.CategoriaId))
            {
                return BadRequest($"Categoria com ID {game.CategoriaId} não encontrada");
            }

            try
            {
                _context.Entry(game).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound($"Jogo com ID {id} não encontrado");
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

        // Cria um novo jogo
        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verifica se a categoria existe
            if (!await _context.Categories.AnyAsync(c => c.Id == game.CategoriaId))
            {
                return BadRequest($"Categoria com ID {game.CategoriaId} não encontrada");
            }

            try
            {
                _context.Games.Add(game);
                await _context.SaveChangesAsync();

                // Carrega a categoria para retornar objeto
                await _context.Entry(game)
                    .Reference(g => g.Categoria)
                    .LoadAsync();

                return CreatedAtAction("GetGame", new { id = game.Id }, game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Remove um jogo
        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            try
            {
                var game = await _context.Games.FindAsync(id);
                if (game == null)
                {
                    return NotFound($"Jogo com ID {id} não encontrado");
                }

                _context.Games.Remove(game);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Verifica se um jogo existe
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}