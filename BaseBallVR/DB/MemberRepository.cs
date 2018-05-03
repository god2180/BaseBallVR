using BaseBallVR.Models;

namespace BaseBallVR.DB
{
    public interface IMemberRepository : IRepository<Member> { }

    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(Context context) 
            : base(context) { }
    }
}