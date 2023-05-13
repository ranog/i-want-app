namespace IWantApp.Endpoints.Products;

public class ProductGetShowcase
{
    public static string Template => "/products/showcase";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(int? page, int? row, string? orderBy, ApplicationDbContext context)
    {
        page ??= 1;
        row ??= 10;
        if (string.IsNullOrEmpty(orderBy))
            orderBy = "name";

        var queryBase = context.Products
            .Include(p => p.Category)
            .Where(p => p.HasStock && p.Category.Active);

        queryBase = orderBy == "name" ? queryBase.OrderBy(p => p.Name) : queryBase.OrderBy(p => p.Price);

        var queryFilter = queryBase.Skip((page.Value - 1) * row.Value).Take(row.Value);

        var products = queryFilter.ToList();

        var response = products
            .Select(p => new ProductResponse(p.Name, p.Category.Name, p.Description, p.HasStock, p.Price, p.Active));
        return Results.Ok(response);
    }
}
