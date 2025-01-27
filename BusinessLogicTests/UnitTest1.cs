using Madiff.Entities;

namespace BusinessLogicTests;

public class UnitTest1
{
    private readonly CardService _cardService = new CardService();

    [Fact]
    public async Task Test1()
    {
        var details = await _cardService.GetCardDetails("User1", "Card11");

        Assert.NotNull(details);
    }

    [Theory]
    [InlineData("User1", "Card11")]
    [InlineData("", "")]
    [InlineData(null, null)]
    public async Task TestGetCardDetails(string userId, string cardId)
    {
        var details = await _cardService.GetCardDetails(userId, cardId);

        Assert.NotNull(details);
    }
}
