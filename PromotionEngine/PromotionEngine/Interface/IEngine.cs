namespace PromotionEngine.Interface
{
    public interface IEngine
    {
        decimal CalculateCartTotalWithBestPromotion();
        decimal CalculateCartTotalWithMultiplePromotions();
    }
}
