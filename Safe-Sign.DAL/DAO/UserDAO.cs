using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;

namespace Safe_Sign.DAL.DAO
{
    public class UserDAO : BaseDAO<User>
    {
        public void SwitchUserEmailState(ulong id)
        {
            User? user = GetById(id);

            if (user is null) throw new NullReferenceException();

            else
            {
                user.EmailVerified = !user.EmailVerified;

                Update(user);
            }
        }

        public void SwitchUserActiveState(ulong id)
        {
            User? user = GetById(id);

            if (user is null) throw new NullReferenceException();

            else
            {
                user.IsActive = !user.IsActive;

                Update(user);
            }
        }
    }
}

