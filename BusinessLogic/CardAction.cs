namespace Madiff.Entities
{
    public class CardAction
    {
        public string Name { get; }

        public Func<CardDetails, bool> actionAllowed;

        public CardAction(string name, List<Predicate<CardDetails>> rules)
        {
            Name = name;
            this.actionAllowed = cardDetails => rules.All(x => x(cardDetails));
        }

        public CardAction(string name, params Predicate<CardDetails>[] rules)
        {
            Name = name;
            this.actionAllowed = cardDetails => rules.All(x => x(cardDetails));
        }

        public static Predicate<CardDetails> EveryCardTypePredicate =>
            cardDetails => new[] { CardType.Debit, CardType.Prepaid, CardType.Credit }.Any(x => x == cardDetails.CardType);
        public static Predicate<CardDetails> EveryCardStatusPredicate =>
           cardDetails => new[] { CardStatus.Ordered, CardStatus.Inactive,
               CardStatus.Active, CardStatus.Restricted, CardStatus.Blocked,
               CardStatus.Expired, CardStatus.Closed }.Any(x => x == cardDetails.CardStatus);


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
            new CardAction("Action6", (cardDetails) =>
            {
                var cardType = cardDetails.CardType;
                var cardStatus = cardDetails.CardStatus;

                return cardType == CardType.Prepaid || cardType == CardType.Debit || cardType == CardType.Credit
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Ordered)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Inactive)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Active)
                    && (cardDetails.IsPinSet && cardStatus == CardStatus.Blocked);
            }),
            new CardAction("Action7", new List<Predicate<CardDetails    >> {
                cardDetails => new []{CardType.Debit, CardType.Prepaid, CardType.Credit}.Any(x => x == cardDetails.CardType),
                cardDetails => !cardDetails.IsPinSet && new [] {CardStatus.Ordered, CardStatus.Inactive }.Any(x => x == cardDetails.CardStatus),
                cardDetails => cardDetails.IsPinSet && CardStatus.Blocked == cardDetails.CardStatus
            }),
            new CardAction("Action8",
                EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed }.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action9", EveryCardTypePredicate, EveryCardStatusPredicate),
            new CardAction("Action10", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Restricted, CardStatus.Blocked, CardStatus.Expired, CardStatus.Closed }.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action11", EveryCardTypePredicate,
                cardDetails => CardStatus.Inactive == cardDetails.CardStatus || CardStatus.Active == cardDetails.CardStatus),
            new CardAction("Action12", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(status => status == cardDetails.CardStatus)),
            new CardAction("Action13", EveryCardTypePredicate,
                cardDetails => new [] { CardStatus.Ordered, CardStatus.Inactive, CardStatus.Active }.Any(status => status == cardDetails.CardStatus))
        ];
    }
}
