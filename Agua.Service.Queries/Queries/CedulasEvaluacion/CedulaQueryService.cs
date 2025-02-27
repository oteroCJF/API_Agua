﻿using Agua.Domain.DCuestionario;
using Agua.Persistence.Database;
using Agua.Service.Queries.DTOs.CedulaEvaluacion;
using Agua.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agua.Service.Queries.Queries.CedulasEvaluacion
{
    public interface ICedulasQueryService
    {
        Task<List<CedulaEvaluacionDto>> GetAllCedulasAsync();
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio);
        Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato);
        Task<CedulaEvaluacionDto> GetCedulaById(int cedula);
    }

    public class CedulasQueryService : ICedulasQueryService
    {
        private readonly ApplicationDbContext _context;

        public CedulasQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CedulaEvaluacionDto>> GetAllCedulasAsync()
        {
            var collection = await _context.CedulaEvaluacion.OrderByDescending(x => x.Id).ToListAsync();

            return collection.MapTo<List<CedulaEvaluacionDto>>();
        }

        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            try
            {
                int page = 1;
                var totalCedulas = await _context.CedulaEvaluacion.Where(x => x.Anio == anio && !x.FechaEliminacion.HasValue).CountAsync();

                var cedulas = await _context.CedulaEvaluacion
                       .Where(x => x.Anio == anio && !x.FechaEliminacion.HasValue)
                       .OrderBy(x => x.MesId)
                       .GetPagedAsync(page, totalCedulas);

                return cedulas.MapTo<DataCollection<CedulaEvaluacionDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato)
        {
            try
            {
                int page = 1;
                var totalCedulas = await _context.CedulaEvaluacion.Where(x => x.Anio == anio && x.MesId == mes && x.ContratoId == contrato 
                                                                            && !x.FechaEliminacion.HasValue).CountAsync();

                var cedulas = await _context.CedulaEvaluacion
                       .Where(x => x.Anio == anio && x.MesId == mes && x.ContratoId == contrato && !x.FechaEliminacion.HasValue)
                       .OrderBy(x => x.MesId)
                       .GetPagedAsync(page, totalCedulas);

                return cedulas.MapTo<DataCollection<CedulaEvaluacionDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<CedulaEvaluacionDto> GetCedulaById(int cedula)
        {
            return (await _context.CedulaEvaluacion.SingleOrDefaultAsync(x => x.Id == cedula)).MapTo<CedulaEvaluacionDto>();
        }
    }
}
