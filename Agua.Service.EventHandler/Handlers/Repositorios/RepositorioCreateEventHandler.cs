using Agua.Persistence.Database;
using Agua.Domain.DRepositorios;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Agua.Service.EventHandler.Commands.Repositorios;

namespace Agua.Service.EventHandler.Handlers.Repositorios
{
    public class RepositorioCreateEventHandler : IRequestHandler<RepositorioCreateCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public RepositorioCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(RepositorioCreateCommand Repositorio, CancellationToken cancellationToken)
        {
            var nRepositorio = new Repositorio
            {
                ContratoId = Repositorio.ContratoId,
                UsuarioId = Repositorio.UsuarioId,
                MesId = Repositorio.MesId,
                Anio = Repositorio.Anio,
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            };

            try
            {
                await _context.AddAsync(nRepositorio);
                await _context.SaveChangesAsync();
                return 201;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 500;
            }
        }
    }
}
