using Agua.Domain.DIncidencias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Persistence.Database.Configuration
{
    public class IncidenciasConfiguration
    {
        public IncidenciasConfiguration(EntityTypeBuilder<Incidencia> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.FechaProgramada).HasDefaultValue("1990-01-01T00:00:00.00");
            entityBuilder.Property(x => x.FechaEntrega).HasDefaultValue("1990-01-01T00:00:00.00");
            entityBuilder.Property(x => x.Cantidad).HasDefaultValue(0);
            entityBuilder.Property(x => x.Penalizable).HasDefaultValue(false);
            entityBuilder.Property(x => x.MontoPenalizacion).HasDefaultValue(0);
        }
    }
}
