namespace IWantApp.Endpoints.Products;

public class ProductGet
{
    public static string Template => "/products/{id:guid}";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static IResult Action([FromRoute] Guid id, ApplicationDbContext context)
    {
        var product = context.Products.FirstOrDefault(p => p.Id == id);
        
        if(product == null)
            return Results.NotFound();

        if (!product.IsValid)
            return Results.ValidationProblem(product.Notifications.ConvertToProblemDetails());

        context.SaveChanges();
        
        return Results.Ok(product);
    }
}
