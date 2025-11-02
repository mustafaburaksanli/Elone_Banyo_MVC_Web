using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class UserRepository : IUserDal
    {
        Context c = new Context();
        DbSet<User> _object;
        public void Delete(User p)
        {
            throw new NotImplementedException();
        }

        public void Insert(User p)
        {
            throw new NotImplementedException();
        }

        public List<User> List()
        {
            throw new NotImplementedException();
        }

        public List<User> List(Expression<Func<User, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public void Update(User p)
        {
            throw new NotImplementedException();
        }
    }
}
