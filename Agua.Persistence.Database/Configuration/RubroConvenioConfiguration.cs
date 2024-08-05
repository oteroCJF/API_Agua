using Agua.Domain.DRubrosConvenios;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class RubroConvenioConfiguration
    {
        public RubroConvenioConfiguration(EntityTypeBuilder<RubroConvenio> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.RubroId, x.ConvenioId });
        }
    }
}
