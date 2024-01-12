using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rpnw.Domain.Model;
using Rpnw.Domain.Model.Rpn;
using Rpnw.Presentation.Rest.Services.Dto.Operand;
using Rpnw.Presentation.Rest.Services.Dto.Operator;
using Rpnw.Presentation.Rest.Services.Dto.Stack;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpnw.Presentation.Rest.Services.Services
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stacks")]

    [SwaggerResponse(StatusCodes.Status400BadRequest)]
    [SwaggerResponse(StatusCodes.Status409Conflict)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    [SwaggerResponse(StatusCodes.Status503ServiceUnavailable)]
    public class StackController : ControllerBase
    {
        private readonly IStackService _stackService;

        public StackController(IStackService stackService)
        {
            _stackService = stackService ?? throw new ArgumentNullException(nameof(stackService));
        }

        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StackDto))]
        public IActionResult CreateStack()
        {
            try
            {
                var stackId = _stackService.CreateStack();
                var stackDto = new StackDto
                {
                    Id = stackId,
                    Elements = new List<StackElementDto>()
                };

                return CreatedAtAction(nameof(GetStack), new { stackId = stackId }, stackDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpDelete("{stackId}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public IActionResult DeleteStack(Guid stackId)
        {
            try
            {
                _stackService.DeleteStack(stackId);
                return NoContent();
            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
           
        }

        [HttpDelete("{stackId}/clear")]
        public IActionResult ClearStack(Guid stackId)
        {
            try
            {
                _stackService.ClearStack(stackId);
                return NoContent();
            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }

           
        }

        [HttpGet("{stackId}")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StackDto))]
        public IActionResult GetStack(Guid stackId)
        {
            try
            {
                var elements = _stackService.GetStackElements(stackId);
                var stackDto = new StackDto
                {
                    Id = stackId,
                    Elements = ConvertToElementDtos(elements)
                };
                return Ok(stackDto);
            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }

            
        }

        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<StackDto>))]
        public IActionResult GetAllStacks()
        {
            var stacks = _stackService.GetAllStacks();
            var stackDtos = stacks.Select(stackId => new StackDto
            {
                Id = stackId,
                Elements = ConvertToElementDtos(_stackService.GetStackElements(stackId))
            });
            return Ok(stackDtos);
        }

        [HttpPost("{stackId}/operands")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(StackDto))]
        public IActionResult AddOperand(Guid stackId, [FromBody] OperandDto operandDto)
        {
            try
            {
                _stackService.AddOperand(stackId, operandDto.Value);

                var elements = _stackService.GetStackElements(stackId);
                var stackDto = new StackDto
                {
                    Id = stackId,
                    Elements = ConvertToElementDtos(elements)
                };
                return CreatedAtAction(nameof(GetStack), new { stackId = stackId }, stackDto);

            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }

        }

        [HttpPost("{stackId}/operators")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(StackDto))]
        public IActionResult AddOperator(Guid stackId, [FromBody] AddOperatorDto addOperatorDto)
        {
            try
            {
                _stackService.AddOperator(stackId, addOperatorDto.OperatorId);

                var elements = _stackService.GetStackElements(stackId);
                var stackDto = new StackDto
                {
                    Id = stackId,
                    Elements = ConvertToElementDtos(elements)
                };
                return CreatedAtAction(nameof(GetStack), new { stackId = stackId }, stackDto);

            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(DivideByZeroException ex) 
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        [HttpPost("{stackId}/operators/undo")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StackDto))]
        public IActionResult UndoOperator(Guid stackId)
        {
            try
            {
                _stackService.UndoOperator(stackId);

                var elements = _stackService.GetStackElements(stackId);
                var stackDto = new StackDto
                {
                    Id = stackId,
                    Elements = ConvertToElementDtos(elements)
                };
                return CreatedAtAction(nameof(GetStack), new { stackId = stackId }, stackDto);
            }
            catch (StackNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Une erreur interne est survenue.");
            }
        }

        // TODO:Ceci a mettre dans un automapper
        private IEnumerable<StackElementDto> ConvertToElementDtos(IEnumerable<IRpnElement> elements)
        {
            foreach (var element in elements)
            {
                if (element is IRpnOperand operand)
                {
                    yield return new StackElementDto { Value = operand.Value.ToString() };
                }
                else if (element is IRpnOperator operatorElement)
                {
                    string operatorName = operatorElement.GetName().ToString();
                    yield return new StackElementDto { Value = operatorName };
                }

            }
        }


    }
}
