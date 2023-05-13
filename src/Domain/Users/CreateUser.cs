namespace IWantApp.Domain.Users;

public class CreateUser
{
    private readonly UserManager<IdentityUser> _userManager;

    public CreateUser(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(IdentityResult, string)> Create(string email, string password, List<Claim> claims)
    {
        var newUser = new IdentityUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(newUser, password);

        return result.Succeeded
            ? (await _userManager.AddClaimsAsync(newUser, claims), newUser.Id)
            : (result, String.Empty);
    }
}
