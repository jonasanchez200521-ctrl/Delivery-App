namespace Nami.API.DTOs
{
    public record PromotionDto(
        int Id,
        string Code,
        decimal Discount,
        DateTime StartDate,
        DateTime EndDate,
        bool IsActive);

    public record CreatePromotionRequest(
        string Code,
        decimal Discount,
        DateTime StartDate,
        DateTime EndDate);
}
