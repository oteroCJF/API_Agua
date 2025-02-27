﻿using Agua.Persistence.Database;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using Agua.Domain.DEntregables;
using Agua.Service.EventHandler.Commands.Entregables.Update;
using Agua.Service.EventHandler.Commands.Entregables.Create;

namespace Agua.Service.EventHandler.Handlers.Entregables.Create
{
    public class EntregableCreateEventHandler : IRequestHandler<EntregableCreateCommand, Entregable>
    {
        private readonly ApplicationDbContext _context;

        public EntregableCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entregable> Handle(EntregableCreateCommand request, CancellationToken cancellationToken)
        {
            DateTime fechaCreacion= DateTime.Now;

            var entregable = new Entregable
            {
                CedulaEvaluacionId = request.CedulaEvaluacionId,
                UsuarioId = request.UsuarioId,
                EntregableId = request.EntregableId,
                Observaciones = request.Observaciones,
                FechaCreacion = fechaCreacion
            };

            await _context.AddAsync(entregable);
            await _context.SaveChangesAsync();

            return entregable;
        }
    }
}
