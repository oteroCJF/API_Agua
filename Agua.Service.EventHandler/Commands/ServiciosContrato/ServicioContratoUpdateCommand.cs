﻿using MediatR;
using Agua.Domain.DServiciosContratos;

namespace Agua.Service.EventHandler.Commands.ServiciosContrato
{
    public class ServicioContratoUpdateCommand : IRequest<ServicioContrato>
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
    }
}
