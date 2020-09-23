using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheBiscuitMachine.Logic.Common;
using TheBiscuitMachine.Logic.Models;

namespace TheBiscuitMachine.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiscuitMachineController : ControllerBase
    {
        private readonly IMachine _machine;

        public BiscuitMachineController(IMachine machine)
        {
            _machine = machine;
        }

        [HttpPost("on")]
        public async Task<IActionResult> TurnOn()
        {
            await _machine.TurnOn();
            return Ok();
        }

        [HttpPost("off")]
        public async Task<IActionResult> TurnOff()
        {
            await _machine.TurnOff();
            return Ok();
        }

        [HttpPost("pause")]
        public async Task<IActionResult> Pause()
        {
            await _machine.Pause();
            return Ok();
        }

        [HttpGet("state")]
        public IActionResult State()
        {
            var state = ((BiscuitMachine)_machine).State;
            return Ok(state);
        }
    }
}
