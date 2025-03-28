﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Domain.DFirmantes
{
    public class Firmante
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int InmuebleId { get; set; }
        public string Tipo { get; set; }
        public string Escolaridad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public System.Nullable<DateTime> FechaActualizacion { get; set; }
        public System.Nullable<DateTime> FechaEliminacion { get; set; }
    }
}
