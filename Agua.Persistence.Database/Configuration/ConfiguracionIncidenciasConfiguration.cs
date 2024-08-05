using Agua.Domain.DIncidencias;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agua.Persistence.Database.Configuration
{
    public class ConfiguracionIncidenciasConfiguration
    {
        public ConfiguracionIncidenciasConfiguration(EntityTypeBuilder<ConfiguracionIncidencias> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
