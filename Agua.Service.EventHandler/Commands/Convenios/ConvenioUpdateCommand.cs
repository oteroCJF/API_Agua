﻿using Agua.Domain.DContratos;
using MediatR;
using System;
using System.Collections.Generic;
using Agua.Domain.DRubrosConvenios;

namespace Agua.Service.EventHandler.Commands.Convenios
{
    public class ConvenioUpdateCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public string UsuarioId { get; set; }
        public string NoConvenio { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FinVigencia { get; set; }
        public decimal MontoMin { get; set; }
        public decimal MontoMax { get; set; }
        public int VolumetriaMin { get; set; }
        public int VolumetriaMax { get; set; }
        public DateTime FechaFirmaConvenio { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public List<RubroConvenio> Rubros { get; set; } = new List<RubroConvenio>();
    }
}
