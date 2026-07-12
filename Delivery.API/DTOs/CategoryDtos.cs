namespace Delivery.API.DTOs
{
    public record CategoryDto(int Id, string Name, string Description);

    public record CreateCategoryRequest(string Name, string Description);
}
