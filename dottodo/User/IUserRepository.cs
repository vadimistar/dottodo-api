namespace dottodo.User
{
    public interface IUserRepository
    {
        UserEntity Save(UserEntity user);

        UserEntity? GetByUsername(string username);

        UserEntity? GetByRefreshToken(string refreshToken);

        UserEntity? GetById(string userId);

        void SaveChanges();
    }
}
