using Agua.Domain.DCedulaEvaluacion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agua.Persistence.Database.Configuration
{
    public class RespuestasEvaluacionConfiguration
    {
        public RespuestasEvaluacionConfiguration(EntityTypeBuilder<RespuestaEvaluacion> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.CedulaEvaluacionId, x.Pregunta });
        }
    }
}
