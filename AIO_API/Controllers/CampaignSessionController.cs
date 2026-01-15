using AIO_API.Entities;
using AIO_API.Interfaces;
using AIO_API.Models.CampaignSessionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AIO_API.Controllers
{
    [Route("api/campaign/{id}/session")]
    [ApiController]
    [Authorize]
    public class CampaignSessionController : ControllerBase
    {
        private ICampaignSessionService _sessionService;
        private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        public CampaignSessionController(ICampaignSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        public ActionResult<CampaignSessionDto> CreateSession([FromRoute] int id, CreateCampaignSessionDto dto)
        {
            var newSession = _sessionService.CreateCamapignSession(id, UserId,dto);
            return Ok(newSession);
        }

        [HttpGet]
        public ActionResult<IEnumerable<CampaignSessionDto>> GetAllCampaignSessions([FromRoute] int id)
        {
            var sessions =_sessionService.GetAllCampaignSessions(id, UserId);
            return Ok(sessions);
        }
        [HttpGet]
        [Route("{idSession}")]
        public ActionResult<CampaignSessionDto> GetCampaignSessionById([FromRoute] int id, [FromRoute] int idSession)
        {
            var session = _sessionService.GetCampaignSessionById(id, idSession, UserId);
            return Ok(session);
        }

        [HttpDelete]
        [Route("{idSession}")]
        public ActionResult DeleteAllSessions([FromRoute] int id, [FromRoute] int idSession)
        {
            _sessionService.DeleteCampaignSession(id, idSession, UserId);
            return NoContent();
        }
    }
}
