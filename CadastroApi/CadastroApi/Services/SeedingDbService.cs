using CadastroApi.Data;
using CadastroApi.Models;

namespace CadastroApi.Services
{
    public class SeedingDbService
    {
        private readonly AppDbContext _context;

        public SeedingDbService(AppDbContext context)
        {
            _context = context;
        }


        public void Seed()
        {
            if (_context.Contatos.Any())
            {
                return;
            }

            var c1 = new Contato { Nome = "João",  Email = "joao@gmail.com", Telefone = "988994199", DDD = "11"};
            var c2 = new Contato { Nome = "Maria",  Email = "maria@gmail.com", Telefone = "977448844", DDD = "11"};
            var c3= new Contato { Nome = "José",  Email = "jose@gmail.com", Telefone = "988112233", DDD = "14"};
            var c4 = new Contato { Nome = "Ana",  Email = "ana@gmail.com", Telefone = "977665533", DDD = "21"};
            var c5 = new Contato { Nome = "Alice",  Email = "alice@gmail.com", Telefone = "988447799", DDD = "41"};

            _context.AddRange(c1, c2, c3, c4, c5);
            _context.SaveChanges();
        }
    }
}
