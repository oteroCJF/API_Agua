using Agua.Domain.DHistorial;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class LogCedulasConfiguration
    {
        public LogCedulasConfiguration(EntityTypeBuilder<LogCedula> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
