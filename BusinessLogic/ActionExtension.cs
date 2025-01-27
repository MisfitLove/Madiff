namespace Madiff.Entities
{
    public static class ActionExtension
    {
        public static List<CardAction> GetPermittedActions(this CardDetails cardDetails) 
        {
            var allActions = CardAction.actions;
            
            return allActions.Where(x => x.actionAllowed(cardDetails)).ToList();
        }
    }
}
