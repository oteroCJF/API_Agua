using MediatR;
using Agua.Domain.DHistorialEntregables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Service.EventHandler.Commands.LogEntregables
{
    public class LogEntregablesCreateCommand : IRequest<LogEntregable>
    {
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public int EntregableId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
