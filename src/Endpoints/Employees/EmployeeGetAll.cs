namespace IWantApp.Endpoints.Employees;

[Authorize(Policy = "EmployeePolicy")]
public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "Employee005Policy")]
    public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
    {
        if (page == null || rows == null)
            return Results.BadRequest($"Value of page = '{page}' or rows = '{rows}' is empty.");
        var result = await query.Execute(page.Value, rows.Value);
        return Results.Ok(result);
    }
}
