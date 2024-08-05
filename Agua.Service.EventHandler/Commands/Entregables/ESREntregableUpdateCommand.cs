using MediatR;
using Agua.Domain.DEntregables;
using System;

namespace Agua.Service.EventHandler.Commands.Entregables
{
    public class ESREntregableUpdateCommand : IRequest<Entregable>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public string Observaciones { get; set; }
        public bool Aprobada { get; set; }
        public DateTime FechaEliminacion { get; set; }
    }
}
