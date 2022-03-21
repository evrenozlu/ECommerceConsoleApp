using API.DTOs.Campaign.CreateCampaign;
using API.DTOs.Campaign.GetCampaign;
using API.DTOs.Campaign.UpdateCampaign;
using API.Services.Campaigns;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly CampaignService _service;

        public CampaignController(CampaignService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCampaign([FromQuery] GetCampaignRequest request)
        {
            var campaign = await _service.GetCampaign(request);
            return Ok(campaign);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignRequest request)
        {
            var campaign = await _service.CreateCampaign(request);
            return Ok(campaign);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateActiveCampaign([FromBody] UpdateActiveCampaignRequest request)
        {
            var campaign = await _service.UpdateActiveCampaign(request);
            return Ok(campaign);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCampaigns()
        {
            var campaign = await _service.DeleteCampaigns();
            return Ok(campaign);
        }
    }
}
