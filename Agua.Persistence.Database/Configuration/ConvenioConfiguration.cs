using Agua.Domain.DConvenios;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class ConvenioConfiguration
    {
        public ConvenioConfiguration(EntityTypeBuilder<Convenio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
