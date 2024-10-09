using CadastroApi.Data;
using CadastroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Repository;

public class ContatoRepository : Repository<Contato>, IContatoRepository
{
    public ContatoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Contato>> GetByDDDAsync(char ddd)
    {
        return await _context.Contatos.AsNoTracking().Where(c => c.DDD == ddd).ToListAsync();
    }
}
