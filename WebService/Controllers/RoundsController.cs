using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using ServerLogic;
using ServerLogic.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebService.Controllers
{
    public class RoundsController : Controller
    {
        [HttpPost]
        public IActionResult StartNewRound([FromBody] StartNewRoundRequest request)
        {
            var response = RoundManager.Instance.StartNewRound(request);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult NotifyHeroHoles([FromBody] NotifyHeroHolesRequest request)
        {
            var response = RoundManager.Instance.NotifyHeroHoles(request);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NotifyFlops([FromBody] NotifyFlopsRequest request)
        {
            var response = RoundManager.Instance.NotifyFlops(request);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NotifyTurn([FromBody] NotifyTurnRequest request)
        {
            var response = RoundManager.Instance.NotifyTurn(request);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NotifyRiver([FromBody] NotifyRiverRequest request)
        {
            var response = RoundManager.Instance.NotifyRiver(request);
            return Ok(response);
        }
        [HttpPost]
        public IActionResult NotifyDecision([FromBody] NotifyDecisionRequest request)
        {
            var response = RoundManager.Instance.NotifyDecision(request);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetDecision(Guid roundId)
        {
            var response = RoundManager.Instance.GetDecision(roundId);
            return Ok(response);
        }
    }
}