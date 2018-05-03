using BaseBallVR.Models;
using System.Collections.Generic;

namespace BaseBallVR.DB
{
    public interface IUserRepository : IRepository<User> { }

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Context context)
            : base(context)
        {}
        
        
    }
}