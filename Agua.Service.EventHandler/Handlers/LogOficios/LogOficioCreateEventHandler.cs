﻿using Agua.Persistence.Database;
using Agua.Service.EventHandler.Commands.Incidencias;
using Agua.Service.EventHandler.Commands.LogCedulas;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Agua.Domain.DHistorial;
using Agua.Service.EventHandler.Commands.LogOficios;
using Agua.Domain.DHistorialOficios;

namespace Agua.Service.EventHandler.Handlers.LogOficios
{
    public class LogOficiosCreateEventHandler : IRequestHandler<LogOficiosCreateCommand, LogOficio>
    {
        private readonly ApplicationDbContext _context;

        public LogOficiosCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LogOficio> Handle(LogOficiosCreateCommand logs, CancellationToken cancellationToken)
        {
            try
            {
                var log = new LogOficio
                {
                    OficioId = logs.OficioId,
                    UsuarioId = logs.UsuarioId,
                    EstatusId = logs.EstatusId,
                    Observaciones = logs.Observaciones,
                    FechaCreacion = DateTime.Now
                };

                await _context.AddAsync(log);
                await _context.SaveChangesAsync();

                return log;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
