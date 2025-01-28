using Madiff.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Madiff.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ActionController : ControllerBase
    {
        private readonly CardService cardService;

        public ActionController(CardService cardService)
        {
            this.cardService = cardService;
        }

        [HttpGet("CardDetails")]
        [ActionName("GetCardDetails")]
        public async Task<CardDetails> GetCardDetails(string userId, string cardNumber)
        {
            var cardDetails = await cardService.GetCardDetails(userId, cardNumber);

            return cardDetails;
        }

        [HttpGet("PermittedActions")]
        [ActionName("GetPermittedActions")]
        public async Task<IReadOnlyCollection<CardAction>> GetCardActions(string userId, string cardNumber)
        {
            var permittedActions = await cardService.GetPermittedActions(userId, cardNumber);

            return permittedActions;
        }
    }
}
