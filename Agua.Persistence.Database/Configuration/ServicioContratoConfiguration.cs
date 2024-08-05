using Agua.Domain.DServiciosContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class ServicioContratoConfiguration
    {
        public ServicioContratoConfiguration(EntityTypeBuilder<ServicioContrato> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
