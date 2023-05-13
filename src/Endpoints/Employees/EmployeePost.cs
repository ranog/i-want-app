namespace IWantApp.Endpoints.Employees;

public class EmployeePost
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [Authorize(Policy = "EmployeePolicy")]
    public static async Task<IResult> Action(
        EmployeeRequest employeeRequest,
        HttpContext http,
        CreateUser createUser)
    {
        var userId = http.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var userClaims = new List<Claim>
        {
            new Claim("EmployeeCode", employeeRequest.EmployeeCode),
            new Claim("Name", employeeRequest.Name),
            new Claim("CreatedBy", userId),
        };

        (IdentityResult identity, string userId) result =
            await createUser.Create(employeeRequest.Email, employeeRequest.Password, userClaims);

        return result.identity.Succeeded
            ? Results.Created($"{Template}/{result.userId}", result.userId)
            : Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
    }
}
