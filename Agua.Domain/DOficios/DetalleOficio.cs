﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Domain.DOficios
{
    public class DetalleOficio
    {
        public int ServicioId { get; set; }
        public int OficioId { get; set; }
        public int FacturaId { get; set; }
        public int CedulaId { get; set; }
    }
}
