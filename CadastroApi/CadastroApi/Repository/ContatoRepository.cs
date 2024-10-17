using CadastroApi.Data;
using CadastroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Repository;

public class ContatoRepository : Repository<Contato>, IContatoRepository
{
    public ContatoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Contato>> GetByDDDAsync(string ddd)
    {
        return await _context.Contatos.AsNoTracking().Where(c => c.DDD == ddd).ToListAsync();
    }
    public async Task AddContatoAsync(Contato contato)
    {
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContatoAsync(Contato contato)
    {
        var contatoToUpdate = await _context.Contatos.AsNoTracking().Where(c => c.Id == contato.Id).FirstOrDefaultAsync();

        if (contatoToUpdate == null)
            throw new ArgumentException($"Id: {contato.Id} não encontrado na base de dados.");

        _context.Contatos.Update(contato);
        await _context.SaveChangesAsync();
    }
}
