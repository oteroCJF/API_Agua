﻿using MediatR;
using Agua.Domain.DFirmantes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Service.EventHandler.Commands.Firmantes
{
    public class FirmantesCreateCommand : IRequest<Firmante>
    {
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public string Tipo { get; set; }
        public string Escolaridad { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
