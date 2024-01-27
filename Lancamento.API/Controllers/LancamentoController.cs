using Lancamento.API.Application.Interfaces;
using Lancamento.API.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace Lancamento.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LancamentoController : ControllerBase
    {
        private readonly ILactoService _lactoService;

        public LancamentoController(ILactoService lactoService)
        {
            _lactoService = lactoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lactoService.GetAllLancamentos());
        }

        [HttpGet]
        [Route("consolidate/{data}")]
        public async Task<IActionResult> GetConsolidateByDate([FromRoute] DateTime data)
        {
            return Ok(await _lactoService.GetConsolidado(data));
        }

        [HttpPost]
        [Route("reprocess/{data}")]
        public async Task<IActionResult> ReprocessByDate([FromRoute] DateTime data)
        {
            return Ok(await _lactoService.Reprocess(data));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _lactoService.GetById(id));
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LactoAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _lactoService.Add(model);
                await _lactoService.Reprocess(model.Data);
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return StatusCode(StatusCodes.Status201Created, model);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LactoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await _lactoService.Update(model);
                await _lactoService.Reprocess(model.Data);
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return Ok(model);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                DateTime date = (await _lactoService.GetById(id)).Data.Date;

                await _lactoService.Delete(id);
                await _lactoService.Reprocess(date);
            }
            catch (Exception ex)
            {
                return NotFound((ex.InnerException != null) ? ex.InnerException.Message : ex.Message);
            }

            return Ok();
        }
    }
}
