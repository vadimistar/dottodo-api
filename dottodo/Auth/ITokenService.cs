namespace dottodo.Auth
{
    public interface ITokenService
    {
        string CreateAccessToken(string userId, string username);

        TokenDto RefreshToken(string oldToken);

        RefreshTokenDto UpdateRefreshToken(string userId);
    }
}
