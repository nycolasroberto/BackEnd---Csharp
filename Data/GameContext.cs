using Microsoft.EntityFrameworkCore;
using GameCatalog.Models;

namespace GameCatalog.Data
{
    
    public class GameContext : DbContext
    {
        
        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        
        public DbSet<Game> Games { get; set; }

        
        public DbSet<Category> Categories { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Descricao).HasMaxLength(500);
                
                
                entity.HasMany(c => c.Games)
                      .WithOne(g => g.Categoria)
                      .HasForeignKey(g => g.CategoriaId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });

            
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(g => g.Id);
                entity.Property(g => g.Nome).IsRequired().HasMaxLength(200);
                entity.Property(g => g.Descricao).HasMaxLength(1000);
                entity.Property(g => g.DataLancamento).IsRequired();
                entity.Property(g => g.Preco).IsRequired().HasColumnType("decimal(10,2)");
                entity.Property(g => g.Desenvolvedor).IsRequired().HasMaxLength(150);
                entity.Property(g => g.CategoriaId).IsRequired();
            });

           
            SeedData(modelBuilder);
        }

        
        private void SeedData(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Nome = "Ação", Descricao = "Jogos de ação e aventura" },
                new Category { Id = 2, Nome = "RPG", Descricao = "Jogos de interpretação de papéis" },
                new Category { Id = 3, Nome = "Estratégia", Descricao = "Jogos de estratégia e simulação" },
                new Category { Id = 4, Nome = "Corrida", Descricao = "Jogos de corrida e simulação de direção" },
                new Category { Id = 5, Nome = "Esportes", Descricao = "Jogos de esportes em geral" }
            );

        
            modelBuilder.Entity<Game>().HasData(
                new Game 
                { 
                    Id = 1, 
                    Nome = "The Legend of Zelda: Breath of the Wild", 
                    Descricao = "Um épico jogo de aventura em mundo aberto", 
                    DataLancamento = new DateTime(2017, 3, 3), 
                    Preco = 59.99m, 
                    Desenvolvedor = "Nintendo EPD", 
                    CategoriaId = 1 
                },
                new Game 
                { 
                    Id = 2, 
                    Nome = "The Witcher 3: Wild Hunt", 
                    Descricao = "RPG de fantasia medieval com escolhas morais complexas", 
                    DataLancamento = new DateTime(2015, 5, 19), 
                    Preco = 39.99m, 
                    Desenvolvedor = "CD Projekt RED", 
                    CategoriaId = 2 
                },
                new Game 
                { 
                    Id = 3, 
                    Nome = "Civilization VI", 
                    Descricao = "Jogo de estratégia por turnos para construir impérios", 
                    DataLancamento = new DateTime(2016, 10, 21), 
                    Preco = 49.99m, 
                    Desenvolvedor = "Firaxis Games", 
                    CategoriaId = 3 
                }
            );
        }
    }
}