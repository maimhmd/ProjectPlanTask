using OneTrack.PM.Core.Specifications;
using OneTrack.PM.Entities.Models.DB;

namespace OneTrack.PM.Repositories.Specifications.Security
{
    public class UsersRefreshTokensSpecifications : BaseSpecification<SecUsersRefreshToken>
    {
        public UsersRefreshTokensSpecifications(int userId, string refreshToken)
            : base(R => R.UserId == userId && R.RefreshToken == refreshToken)
        {
            Includes.Add(C => C.User.Contact);
        }
    }
}