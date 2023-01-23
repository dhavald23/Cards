using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly CardsDBContext cardsDBContext;
        public CardsController(CardsDBContext cardsDBContext) {
            this.cardsDBContext = cardsDBContext;
        }

        //GetAllCards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
           var cards = await cardsDBContext.Cards.ToArrayAsync();
            return Ok(cards);
        }

        //GetSingleCard
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid id)
        {
            var cards = await cardsDBContext.Cards.FirstOrDefaultAsync(X => X.Id == id);
            if(cards== null) {
                return NotFound("Card not found");
            }
            return Ok(cards);
        }

        //Add Card
        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();
            await cardsDBContext.Cards.AddAsync(card);
            await cardsDBContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
        }

        //Updating Card
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var existingcards = await cardsDBContext.Cards.FirstOrDefaultAsync(X => X.Id == id);
            if(existingcards!= null) {
                existingcards.CardholderName = card.CardholderName;
                existingcards.CardholderNumber = card.CardholderNumber;
                existingcards.ExpiryMonth = card.ExpiryMonth;
                existingcards.ExpiryYear = card.ExpiryYear;
                existingcards.CVC = card.CVC;
                await cardsDBContext.SaveChangesAsync();
                return Ok(existingcards);
            }
            else
            {
                return NotFound("Card not Found");
            }
        }

        //Delete Card
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var existingcards = await cardsDBContext.Cards.FirstOrDefaultAsync(X => X.Id == id);
            if (existingcards != null)
            {
                cardsDBContext.Remove(existingcards);
                await cardsDBContext.SaveChangesAsync();
                return Ok(existingcards);
            }
            else
            {
                return NotFound("Card not Found");
            }
        }
    }
}
