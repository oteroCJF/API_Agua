﻿using Agua.Domain.DCuestionario;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class CuestionarioMensualConfiguration
    {
        public CuestionarioMensualConfiguration(EntityTypeBuilder<CuestionarioMensual> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.CuestionarioId, x.ContratoId, x.Consecutivo, x.Anio, x.MesId });
        }
    }
}
