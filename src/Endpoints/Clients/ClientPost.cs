namespace IWantApp.Endpoints.Clients;

public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(
        ClientRequest clientRequest,
        HttpContext http,
        UserManager<IdentityUser> userManager)
    {
        var newUser = new IdentityUser { UserName = clientRequest.Email, Email = clientRequest.Email };
        var result = await userManager.CreateAsync(newUser, clientRequest.Password);

        if (!result.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        var userClaims = new List<Claim>
        {
            new Claim("Cpf", clientRequest.Cpf),
            new Claim("Name", clientRequest.Name),
        };

        var claimResult = await userManager.AddClaimsAsync(newUser, userClaims);

        return claimResult.Succeeded
            ? Results.Created($"{Template}/{newUser.Id}", newUser.Id)
            : Results.BadRequest(result.Errors.First());
    }
}
