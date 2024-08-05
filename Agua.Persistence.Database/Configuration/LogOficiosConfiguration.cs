using Agua.Domain.DHistorial;
using Agua.Domain.DHistorialOficios;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class LogOficiosConfiguration
    {
        public LogOficiosConfiguration(EntityTypeBuilder<LogOficio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
