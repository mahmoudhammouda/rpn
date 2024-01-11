using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rpnw.Domain.Model.Rpn;
using Rpnw.Domain.Services;
using Rpnw.Presentation.Rest.Services.Dto.Operator;
using Swashbuckle.AspNetCore.Annotations;


namespace Rpnw.Presentation.Rest.Services.Services
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/operators")]
    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorService _operationService;

        public OperatorController(
            IOperatorService operationService)
        {
            _operationService = operationService ?? throw new ArgumentNullException(nameof(operationService));
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<OperatorDto>))]
        public IActionResult GetAllOperators()
        {
            try
            {
                var operators = _operationService.GetAllOperators()
                              .Select(op => new OperatorDto
                              {
                                  Id = op.Id,
                                  Name = op.Name,
                                  Description = op.Description
                              });
                return Ok(operators);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(OperatorDto))]
        public IActionResult GetOperator(int id)
        {
            try
            {
                var op = _operationService.GetOperatorById(id);
                var operatorDto = new OperatorDto()
                              {
                                  Id = op.Id,
                                  Name = op.Name,
                                  Description = op.Description
                              };

                return Ok(operatorDto);
            }
            catch (OperatorNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

    }
}
