using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Agua.Persistence.Database;
using Agua.Domain.DCedulaEvaluacion;
using Agua.Domain.DFacturas;
using Agua.Domain.DIncidencias;
using Agua.Domain.DCuestionario;
using Agua.Service.EventHandler.Commands.CedulasEvaluacion;

namespace Agua.Service.EventHandler.Handlers.CedulasEvaluacion
{
    public class EnviarCedulaEvaluacionUpdateEventHandler : IRequestHandler<EnviarCedulaEvaluacionUpdateCommand, CedulaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public EnviarCedulaEvaluacionUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CedulaEvaluacion> Handle(EnviarCedulaEvaluacionUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CedulaEvaluacion cedula = _context.CedulaEvaluacion.FirstOrDefault(c => c.Id == request.Id);

                if (request.Calcula)
                {
                    List<Factura> facturas = _context.Facturas
                                                                   .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                               f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("Factura")
                                                                               && f.Facturacion.Equals("Mensual"))
                                                                   .ToList();

                    List<CuestionarioMensual> cuestionarioMensual = _context.CuestionarioMensual
                                                                .Where(cm => cm.Anio == cedula.Anio && cm.MesId == cedula.MesId && cm.ContratoId == cedula.ContratoId)
                                                                .ToList();

                    var respuestas = await Obtienetotales(request.Id, cuestionarioMensual);
                    var calificacion = await GetCalificacionCedula(request.Id, cuestionarioMensual);

                    cedula.EstatusId = request.EstatusId;
                    if (calificacion < 10)
                    {
                        //string calif = calificacion.ToString().Substring(0, 3);
                        //cedula.Calificacion = Convert.ToDouble(calif);

                        string calif = (Math.Round(calificacion, 1)).ToString();
                        cedula.Calificacion = Convert.ToDouble(calif);
                    }
                    else
                    {
                        cedula.Calificacion = (double)calificacion;
                    }

                    if (Convert.ToDecimal(cedula.Calificacion) < Convert.ToDecimal(8))
                    {
                        //cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / calificacion;
                        cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / Convert.ToDecimal(cedula.Calificacion);
                        cedula.Penalizacion = Math.Round(cedula.Penalizacion, 2);
                    }
                    else
                    {
                        cedula.Penalizacion = 0;
                    }
                    
                    cedula.FechaActualizacion = request.FechaActualizacion;
                    
                    await _context.SaveChangesAsync();

                    foreach (var fac in facturas)
                    {
                        fac.EstatusId = request.EFacturaId;

                        await _context.SaveChangesAsync();
                    }
                }

                return cedula;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }        
        
        private async Task<List<RespuestaEvaluacion>> Obtienetotales(int cedula, List<CuestionarioMensual> cuestionario)
        {
            var incidencias = new List<Incidencia>();

            foreach (var cm in cuestionario)
            {
                incidencias = _context.Incidencias.Where(i => i.Pregunta == cm.Consecutivo && i.CedulaEvaluacionId == cedula
                                                            && !i.FechaEliminacion.HasValue).ToList();
                var respuesta = _context.Respuestas.SingleOrDefault(r => r.CedulaEvaluacionId == cedula && r.Pregunta == cm.Consecutivo);

                if (!respuesta.Detalles.Equals("N/A")) {
                    respuesta.Detalles = incidencias.Count() + "";
                }
                respuesta.Penalizable = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? true : false);
                respuesta.MontoPenalizacion = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? incidencias.Sum(i => i.MontoPenalizacion) : 0);

                await _context.SaveChangesAsync();
            }
            var respuestas = _context.Respuestas.ToList();

            return respuestas;
        }

        private async Task<decimal> GetCalificacionCedula(int cedula, List<CuestionarioMensual> cuestionario)
        {
            decimal calificacion = 0;
            decimal ponderacion = 0;
            bool calidad = true;
            var incidencias = 0;

            var respuestas = _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula).ToList();

            //Bucle que recorre cada una de las preguntas con su respuesta e informacion como tipo, ponderacion, etc.
            foreach (var rs in respuestas)
            {
                var cm = cuestionario.Single(c => c.Consecutivo == rs.Pregunta);
                if (cm.ACLRS == rs.Respuesta && !rs.Detalles.Equals("N/A"))
                {
                    calidad = false;
                    incidencias = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula && i.Pregunta == cm.Consecutivo
                                                            && !i.FechaEliminacion.HasValue).Count();
                                    
                    //La Ponderacion de la pregunta (9) se divide a la mita, dejandola como 4.5 
                    ponderacion = Convert.ToDecimal(cm.Ponderacion) / Convert.ToDecimal(2);
                    

                    calificacion += ponderacion;

                    rs.Detalles = incidencias+"";
                    rs.Penalizable = true;
                    rs.MontoPenalizacion = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula && 
                                                                      i.Pregunta == cm.Consecutivo && 
                                                                      !i.FechaEliminacion.HasValue).Sum(i => i.MontoPenalizacion);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    calificacion += Convert.ToDecimal(cm.Ponderacion);
                    rs.Penalizable = false;
                }
            }

            calificacion = Convert.ToDecimal(calificacion / respuestas.Count());

            return calidad ? calificacion + (decimal)1 : calificacion;
        }
    }
}
