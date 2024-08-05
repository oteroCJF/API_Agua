using Agua.Domain.DCuestionario;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class CuestionarioConfiguration
    {
        public CuestionarioConfiguration(EntityTypeBuilder<Cuestionario> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
