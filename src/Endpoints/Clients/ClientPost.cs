using IWantApp.Domain.Users;

namespace IWantApp.Endpoints.Clients;

public class ClientPost
{
    public static string Template => "/clients";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    [AllowAnonymous]
    public static async Task<IResult> Action(ClientRequest clientRequest, CreateUser createUser)
    {
        var userClaims = new List<Claim>
        {
            new Claim("Cpf", clientRequest.Cpf),
            new Claim("Name", clientRequest.Name),
        };
        (IdentityResult identity, string userId) result =
            await createUser.Create(clientRequest.Email, clientRequest.Password, userClaims);

        return result.identity.Succeeded
            ? Results.Created($"{Template}/{result.userId}", result.userId)
            : Results.ValidationProblem(result.identity.Errors.ConvertToProblemDetails());
    }
}
