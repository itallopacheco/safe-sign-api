using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;

namespace Safe_Sign.DAL.DAO
{
    public class ProfileDAO : BaseDAO<Profile>
    {
        public void SwitchProfileActiveStatus(Profile profile)
        {
            profile.IsActive = !profile.IsActive;

            Update(profile);
        }
    }
}
