namespace dottodo.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public UserEntity Save(UserEntity user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        public UserEntity? GetByUsername(string username)
        {
            return _db.Users.FirstOrDefault(user => user.UserName == username);
        }

        public UserEntity? GetByRefreshToken(string refreshToken)
        {
            return _db.Users.FirstOrDefault(user => user.RefreshToken == refreshToken);
        }

        public UserEntity? GetById(string userId)
        {
            return _db.Users.Find(userId);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}
