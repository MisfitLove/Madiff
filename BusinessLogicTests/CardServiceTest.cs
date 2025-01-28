using Madiff.Entities;

namespace BusinessLogicTests;

public class CardServiceTest
{
    private readonly CardService _cardService;

    public CardServiceTest()
    {
        _cardService = new CardService();

        var cards = new CardDetails[] { 
            new CardDetails("66", CardType.Credit, CardStatus.Blocked, true),
            new CardDetails("67", CardType.Credit, CardStatus.Ordered, false),
            new CardDetails("61", CardType.Credit, CardStatus.Blocked, true)
        }.ToDictionary(x => x.CardNumber, x => x);

        _cardService._userCards.Add("User6", cards);
    }

    [Fact]
    public async Task GetCardDetails_ExistingUser_ReturnsData()
    {
        //i'd arrange data here
        var userId = "User1";
        var cardId = "Card11";

        //act, i'd ask for arranged data
        var cardDetails = await _cardService.GetCardDetails(userId, cardId);

        //assert, would verify with arranged data, could use value based equality instead of manually checking all properties 1 by 1
        Assert.True(cardDetails.CardNumber == cardId);
    }

    [Fact]
    public async Task GetCardDetails_NonExistingUser_DoesNotReturnData()
    {
        //arrange
        var userId = "";
        var cardId = "";

        //act
        var cardDetails = await _cardService.GetCardDetails(userId, cardId);

        //assert
        Assert.Null(cardDetails);
    }

    [Fact]
    public async Task GetCardDetails_NullArguments_Throws()
    {
        var exception = await Record.ExceptionAsync(() => _cardService.GetCardDetails(null, null));

        Assert.NotNull(exception);
    }

    [Theory]
    [InlineData("avcas", "asda")]
    [InlineData("", "")]
    public async Task GetCardPermittedActions_UserDoesNotExist_DoesNotThrow(string userId, string cardId)
    {
        var exception = await Record.ExceptionAsync(() => _cardService.GetPermittedActions(userId, cardId));

        Assert.Null(exception);
    }

    [Fact]
    public async Task GetCardPermittedActions_ParametersNull_Throws()
    {
        var exception = await Record.ExceptionAsync(() => _cardService.GetPermittedActions(null, null));

        Assert.NotNull(exception);
    }


    [Theory]
    [InlineData(null, CardStatus.Active, null, "Action1")]
    [InlineData(null, CardStatus.Inactive, null, "Action2")]
    [InlineData(CardType.Debit, CardStatus.Active, null, "Action3")]
    [InlineData(CardType.Debit, CardStatus.Active, null, "Action4")]
    [InlineData(CardType.Credit, CardStatus.Active, null, "Action5")]
    [InlineData(null, CardStatus.Blocked, true, "Action6")]
    [InlineData(null, CardStatus.Ordered, false, "Action7")]
    [InlineData(CardType.Debit, CardStatus.Ordered, null, "Action8")]
    [InlineData(CardType.Debit, CardStatus.Active, null, "Action9")]
    [InlineData(null, CardStatus.Active, null, "Action10")]
    [InlineData(null, CardStatus.Active, null, "Action11")]
    [InlineData(CardType.Debit, CardStatus.Ordered, null, "Action12")]
    [InlineData(null, CardStatus.Ordered, null, "Action13")]
    public async Task GetCardPermittedActions(CardType? cardType, CardStatus? cardStatus, bool? pin, string expectedAction)
    {
        var flatKeyValues = _cardService._userCards.SelectMany(x => x.Value.Values, (x, cardDetails) => (x.Key, cardDetails));

        var (userId, cardDetails) = flatKeyValues.First(x => 
        cardType.HasValue ? x.cardDetails.CardType == cardType : true &&
        cardStatus.HasValue ? x.cardDetails.CardStatus == cardStatus : true &&
        (pin.HasValue ? x.cardDetails.IsPinSet == pin.Value : true));

        var permittedActions = await _cardService.GetPermittedActions(userId, cardDetails.CardNumber);

        Assert.Contains(permittedActions, x => x.Name == expectedAction);
    }
}
