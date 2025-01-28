namespace Madiff.Entities
{
    public class CardAction
    {
        public string Name { get; }

        public Func<CardDetails, bool> actionAllowed;

        public CardAction(string name, params Predicate<CardDetails>[] rules)
        {
            Name = name;
            this.actionAllowed = cardDetails => rules.All(x => x(cardDetails));
        }

        // for it to always be true would need to use Enum.GetValues, but this is just helper to not type as much
        public static Predicate<CardDetails> EveryCardTypePredicate =>
            cardDetails => new[] { CardType.Debit, CardType.Prepaid, CardType.Credit }.Any(x => x == cardDetails.CardType);

        // for it to always be true would need to use Enum.GetValues, but this is just helper to not type as much
        public static Predicate<CardDetails> EveryCardStatusPredicate =>
           cardDetails => new[] { CardStatus.Ordered, CardStatus.Inactive,
               CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked,
               CardStatus.Expired, CardStatus.Closed }.Any(x => x == cardDetails.CardStatus);

        // for action rules could also try going for some fluent builder approach to make it more readable at glance (if it's important), or use fluent validation if acceptable
        // also maybe try using expressions if performance is important?
        // i'm checking for conditions which will always be true just to "document" rules
        public static readonly IReadOnlyCollection<CardAction> Actions =
        [
            new CardAction("Action1", (cardDetails) =>
                {
                var cardType = cardDetails.CardType;
                var cardStatus = cardDetails.CardStatus;

                return cardType == CardType.Prepaid || cardType == CardType.Debit || cardType == CardType.Credit
                    && cardStatus == CardStatus.Active;
                }),
            new CardAction("Action2",
                cardDetails => new []{CardType.Debit, CardType.Prepaid, CardType.Credit}.Any(x => x == cardDetails.CardType),
                cardDetails => cardDetails.CardStatus == CardStatus.Inactive),
            new CardAction("Action3",
                EveryCardTypePredicate,
                EveryCardStatusPredicate
                ),
            new CardAction("Action4",
                EveryCardTypePredicate,
                EveryCardStatusPredicate),
            new CardAction("Action5",
                cardDetails => cardDetails.CardType == CardType.Credit),
            new CardAction("Action6", cardDetails =>
            {
                var cardType = cardDetails.CardType;
                var cardStatus = cardDetails.CardStatus;

                return cardType == CardType.Prepaid || cardType == CardType.Debit || cardType == CardType.Credit
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Ordered)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Inactive)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Active)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Blocked);
            }),
            new CardAction("Action7", 
                cardDetails => new []{CardType.Debit, CardType.Prepaid, CardType.Credit}.Any(x => x == cardDetails.CardType),
                cardDetails => !cardDetails.IsPinSet && new [] {CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(x => x == cardDetails.CardStatus) ||
                                cardDetails.IsPinSet && CardStatus.Blocked == cardDetails.CardStatus),
            new CardAction("Action8",
                EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active, CardStatus.Blocked}.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action9", EveryCardTypePredicate, EveryCardStatusPredicate),
            new CardAction("Action10", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action11", EveryCardTypePredicate,
                cardDetails => CardStatus.Inactive == cardDetails.CardStatus || CardStatus.Active == cardDetails.CardStatus),
            new CardAction("Action12", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action13", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(status => status == cardDetails.CardStatus))
        ];
    }
}
