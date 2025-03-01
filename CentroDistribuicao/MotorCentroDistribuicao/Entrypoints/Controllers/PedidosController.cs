using Microsoft.AspNetCore.Mvc;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.UseCases;
using Serilog.Context;

namespace MotorCentroDistribuicao.Entrypoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController(
        IProcessarPedidoUseCase processarUseCase,
        IGetPedidoUseCase consultarUseCase) : ControllerBase
    {
        private const string CORRELATION_ID = "correlation_id";

        [HttpGet("/{pedidoId}")]
        public IActionResult GetPedidoProcessado([FromRoute] Guid pedidoId)
        {
            AddCorrelationIdToLogContext(Request);

            var retorno = consultarUseCase.GetPedidoProcessado(pedidoId);

            if (retorno.NotFound)
                return NotFound();

            if (retorno.HasError)
                return BadRequest();

            return Ok(retorno);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarItens([FromBody] PedidoDto pedido)
        {
            AddCorrelationIdToLogContext(Request);

            var retorno = await processarUseCase.GetCentrosDistribuicao(pedido);

            if (retorno.NotFound)
                return NotFound();

            if (retorno.HasError)
                return BadRequest(retorno);

            return Ok(retorno);
        }

        private void AddCorrelationIdToLogContext(HttpRequest request)
        {
            var correlationId = 
                request.Headers[CORRELATION_ID].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(correlationId))
                correlationId = Guid.NewGuid().ToString();

            LogContext.PushProperty(CORRELATION_ID, correlationId);
        }

    }
}
