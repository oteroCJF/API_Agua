﻿using Agua.Service.Queries.DTOs.EntregablesContratacion;
using Agua.Service.Queries.Queries.EntregablesContratacion;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Agua.Api.Controllers.EntregablesContratacion.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/agua/entregablesContratacion")]
    public class EntregableContratacionController : ControllerBase
    {
        private readonly IEntregableContratacionQueryService _entregables;
        private readonly IHostingEnvironment _environment;

        public EntregableContratacionController(IEntregableContratacionQueryService entregables, IHostingEnvironment environment)
        {
            _entregables = entregables;
            _environment = environment;
        }

        [Route("getEntregablesByContrato/{contrato}")]
        [HttpGet]
        public async Task<List<EntregableContratacionDto>> GetEntregablesByContrato(int contrato)
        {
            var entregables = await _entregables.GetEntregableContratacionByContrato(contrato);

            return entregables;
        }

        [Route("getEntregablesByContratoConvenio/{contrato}/{convenio}")]
        [HttpGet]
        public async Task<List<EntregableContratacionDto>> GetEntregablesByContratoConvenio(int contrato, int convenio)
        {
            var entregables = await _entregables.GetEntregableContratacionByContratoConvenio(contrato, convenio);

            return entregables;
        }

        [Route("getEntregableById/{entregable}")]
        [HttpGet]
        public async Task<EntregableContratacionDto> GetEntregableById(int entregable)
        {
            var entregables = await _entregables.GetEntregableById(entregable);

            return entregables;
        }

        [Route("visualizarEntregableCont/{contrato}/{tipoEntregable}/{archivo}")]
        [HttpGet]
        public string VisualizarEntregableCont(string contrato, string tipoEntregable, string archivo)
        {
            string folderName = "";
            string webRootPath = _environment.ContentRootPath;
            folderName = Directory.GetCurrentDirectory() + "\\Entregables Contratos\\" + contrato + "\\" + tipoEntregable;

            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, archivo);

            if (System.IO.File.Exists(pathArchivo))
            {
                return pathArchivo;
            }

            return "";
        }

        [Route("visualizarEntregableConv/{contrato}/{convenio}/{tipoEntregable}/{archivo}")]
        [HttpGet]
        public string VisualizarEntregableConv(string contrato, string convenio, string tipoEntregable, string archivo)
        {
            string folderName = "";
            string webRootPath = _environment.ContentRootPath;
            folderName = Directory.GetCurrentDirectory() + "\\Entregables Contratos\\" + contrato + "\\" + convenio + "\\" + tipoEntregable;

            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, archivo);

            if (System.IO.File.Exists(pathArchivo))
            {
                return pathArchivo;
            }

            return "";
        }
    }
}
