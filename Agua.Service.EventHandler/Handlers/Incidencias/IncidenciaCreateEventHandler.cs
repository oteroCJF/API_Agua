using Agua.Persistence.Database;
using Agua.Domain.DFacturas;
using Agua.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using Agua.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Agua.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaCreateEventHandler : IRequestHandler<IncidenciaCreateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;
        public IncidenciaCreateEventHandler() { }
        public IncidenciaCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaCreateCommand request, CancellationToken cancellationToken)
        {
            var factura = GetFactura(request);

            var montoPenalizacion = GetPenaDeductiva(request, factura);

            var incidencia = new Incidencia
            {
                    UsuarioId = request.UsuarioId,
                    IncidenciaId = request.IncidenciaId,
                    CedulaEvaluacionId = request.CedulaEvaluacionId,
                    Pregunta = request.Pregunta,
                    FechaProgramada = request.FechaProgramada,
                    FechaEntrega = request.FechaEntrega,
                    HoraProgramada = request.HoraProgramada,
                    HoraRealizada = request.HoraRealizada,
                    Cantidad = request.Cantidad,
                    Observaciones = request.Observaciones,
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,
                    MontoPenalizacion = montoPenalizacion
                };
            try 
            { 
                await _context.AddAsync(incidencia);
                await _context.SaveChangesAsync();

                if (request.DTIncidencia.Count() != 0)
                {
                    decimal totalDI = 0;
                    foreach (var dt in request.DTIncidencia)
                    {
                        var dtIncidencia = new DetalleIncidencia();
                        dtIncidencia.IncidenciaId = incidencia.Id;
                        dtIncidencia.IncidenciaId = dt;
                        dtIncidencia.MontoPenalizacion = GetPrecioUnitarioAgua(request, factura) * Convert.ToDecimal(0.01) * incidencia.Cantidad;
                        totalDI += dtIncidencia.MontoPenalizacion;

                        await _context.AddAsync(dtIncidencia);
                        await _context.SaveChangesAsync();
                    }
                    var Uincidencia = _context.Incidencias.Where(e => e.Id == incidencia.Id && !e.FechaEliminacion.HasValue).FirstOrDefault();
                    Uincidencia.MontoPenalizacion = totalDI;

                    await _context.SaveChangesAsync();
                }
                return incidencia;
            }
            catch (Exception ex)
            {
                string msg = ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException;
                return null;
            }
        }

        public decimal GetPenaDeductiva(IncidenciaCreateCommand incidencia, Factura factura)
        {
            decimal montoPenalizacion = 0;
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);

            var respuesta = _context.Respuestas.Single(r => r.Pregunta == incidencia.Pregunta && r.CedulaEvaluacionId == incidencia.CedulaEvaluacionId);

            var cuestionario = _context.CuestionarioMensual.Single(cm => cm.Consecutivo == incidencia.Pregunta &&
                                                                         cm.Anio == cedula.Anio &&
                                                                         cm.MesId == cedula.MesId &&
                                                                         cm.ContratoId == cedula.ContratoId);
            if (respuesta.Respuesta == cuestionario.ACLRS)
            {
                if (cuestionario.Formula.Contains("CTG * PP * NDA"))
                {
                    var diasAtraso = calculaDiasAtraso(incidencia.FechaProgramada, incidencia.FechaEntrega);
                                                    //Se multiplica el precio unitario por la cantidad de garrafones por la deductiva por los días naturales de atraso
                    montoPenalizacion = (GetPrecioUnitarioAgua(incidencia, factura) * incidencia.Cantidad) * cuestionario.Porcentaje * diasAtraso;
                }
                else if (cuestionario.Formula.Contains("CFM * PP * NDA"))
                {
                    var diasAtraso = calculaDiasAtraso(incidencia.FechaProgramada, incidencia.FechaEntrega);
                    montoPenalizacion = GetMontoFacturaSinIVA(incidencia, factura) * cuestionario.Porcentaje * diasAtraso;
                }
                else if (cuestionario.Formula.Contains("CFM * PD"))
                {
                    montoPenalizacion = GetMontoFacturaSinIVA(incidencia, factura) * cuestionario.Porcentaje;
                }
                else if(cuestionario.Formula.Contains("CFM * PP * NI"))
                {
                    montoPenalizacion = GetMontoFacturaSinIVA(incidencia, factura) * cuestionario.Porcentaje * incidencia.Cantidad;
                }
                else if (cuestionario.Formula.Contains("CFME * PP * NDA * E"))
                {
                    var diasAtraso = calculaDiasAtraso(incidencia.FechaProgramada, incidencia.FechaEntrega);
                    montoPenalizacion = (GetMontoFacturaSinIVA(incidencia, factura) * incidencia.Cantidad) * cuestionario.Porcentaje * diasAtraso;
                }
            }
            return montoPenalizacion;
        }

        public Factura GetFactura(IncidenciaCreateCommand incidencia)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);
            var facturas = new Factura();

            facturas = _context.Facturas.Single(f => f.RepositorioId == repositorio.Id &&
                                                    cedula.InmuebleId == f.InmuebleId &&
                                                    f.Tipo.Equals("Factura") && f.FechaEliminacion == null);
            
            return facturas;
        }

        public decimal GetMontoFacturaSinIVA(IncidenciaCreateCommand incidencia, Factura factura)
        {
            var cedula = _context.CedulaEvaluacion.Single(ce => ce.Id == incidencia.CedulaEvaluacionId);
            var repositorio = _context.Repositorios.Single(r => r.Anio == cedula.Anio && r.ContratoId == cedula.ContratoId
                                                                && r.MesId == cedula.MesId);

            var facturas = _context.Facturas.Where(f => f.RepositorioId == repositorio.Id &&
                                                            cedula.InmuebleId == f.InmuebleId && f.Tipo.Equals("Factura"));

            return factura.Subtotal;
        }

        public decimal GetPrecioUnitarioAgua(IncidenciaCreateCommand incidencia, Factura factura)
        {
            var conceptosFactura = _context.ConceptosFactura.Where(c => c.FacturaId == factura.Id).First();

            return conceptosFactura.PrecioUnitario;
        }

        public int calculaDiasAtraso (DateTime fechaProgramada, DateTime fechaEntrega)
        {
            int diasAtraso = 0;

            for (; true;)
            {
                fechaProgramada = fechaProgramada.AddDays(1);

                if (!EsInhabil(fechaProgramada))
                {
                    diasAtraso++;
                }

                if (fechaProgramada == fechaEntrega)
                {
                    break;
                }
            }
            return diasAtraso;
        }

        private bool EsInhabil(DateTime fecha)
        {
            List<DateTime> diasInhabiles = new List<DateTime>()
            {
                //DIAS INHABILES DEL 2025
                  new DateTime(2025, 01, 01),
                  new DateTime(2025, 02, 03),
                  new DateTime(2025, 03, 17),
                  new DateTime(2025, 04, 16),
                  new DateTime(2025, 04, 17),
                  new DateTime(2025, 04, 18),
                  new DateTime(2025, 05, 01),
                  new DateTime(2025, 05, 02),
                  new DateTime(2025, 05, 05),
                  new DateTime(2025, 07, 16),
                  new DateTime(2025, 07, 17),
                  new DateTime(2025, 07, 18),
                  new DateTime(2025, 07, 21),
                  new DateTime(2025, 07, 22),
                  new DateTime(2025, 07, 23),
                  new DateTime(2025, 07, 24),
                  new DateTime(2025, 07, 25),
                  new DateTime(2025, 07, 28),
                  new DateTime(2025, 07, 29),
                  new DateTime(2025, 07, 30),
                  new DateTime(2025, 07, 31),
                               new DateTime(2025, 09, 16),
                               new DateTime(2025, 11, 17),
                               new DateTime(2025, 12, 16),
                               new DateTime(2025, 12, 17),
                               new DateTime(2025, 12, 18),
                               new DateTime(2025, 12, 19),
                               new DateTime(2025, 12, 20),
                               new DateTime(2025, 12, 23),
                               new DateTime(2025, 12, 24),
                               new DateTime(2025, 12, 25),
                               new DateTime(2025, 12, 26),
                               new DateTime(2025, 12, 27),
                               new DateTime(2025, 12, 30),
                               new DateTime(2025, 12, 31)
            };
            if (diasInhabiles.Contains(fecha))
            {
                Debug.WriteLine("ENCONTRÉ UN DÍA INHABIL:" + fecha);
            }
            return diasInhabiles.Contains(fecha);
        }
    }
    
}

