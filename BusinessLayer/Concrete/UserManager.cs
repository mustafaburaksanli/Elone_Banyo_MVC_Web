using DataAccessLayer.Concrete.Repositories;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class UserManager
    {
        GenericRepository<User> repo = new GenericRepository<User>();

        public List<User> GetAllBL()
        {
            return repo.List();
        }

        public void AddBL(User u)
        {
                repo.Insert(u);            
        }
        public void DeleteBL(User u)
        {
            repo.Delete(u);
        }
        public void UpdateeBL(User u)
        {
            repo.Update(u);
        }
    }
}
