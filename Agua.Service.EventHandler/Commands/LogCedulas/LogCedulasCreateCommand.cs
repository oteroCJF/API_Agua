﻿using MediatR;
using Agua.Domain.DHistorial;
using System;

namespace Agua.Service.EventHandler.Commands.LogCedulas
{
    public class LogCedulasCreateCommand : IRequest<LogCedula>
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
