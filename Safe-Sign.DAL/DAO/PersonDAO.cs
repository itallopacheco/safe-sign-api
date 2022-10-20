using Safe_Sign.DAL.Models;
using Safe_Sign.DAL.DAO.Base;

namespace Safe_Sign.DAL.DAO
{
    public class PersonDAO : BaseDAO<Person>
    {
        public Person GetByName(string name)
        {
            Person? person = _context.Set<Person>().FirstOrDefault(p => p.FullName.ToLower().Trim() == name.ToLower().Trim());

            if (person is null) throw new NullReferenceException();
            
            return person;
        }

        public Person GetByEmail(string email)
        {
            Person? person = _context.Set<Person>().FirstOrDefault(p => p.User.Email.ToLower().Trim() == email.ToLower().Trim());

            if (person is null) throw new NullReferenceException();

            return person;
        }

        public Person GetByCPF(string cpf)
        {
            Person? person = _context.Set<Person>().FirstOrDefault(p => p.CPF == cpf);

            if (person is null) throw new NullReferenceException();
            
            return person;
        }

        public Person GetByIdUser(ulong idUser)
        {
            Person? person = _context.Set<Person>().FirstOrDefault(p => p.IdUser == idUser);

            if (person is null) throw new NullReferenceException();

            else return person;
        }

    }
}

