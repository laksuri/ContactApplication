
using Microsoft.EntityFrameworkCore;
using ContactWebAPI.Model;

namespace ContactWebAPI.Context
{
    public class ContactContext:DbContext
    {
        #region properties
        public DbSet<ContactModel> Contacts { get; set; }
        #endregion
        public ContactContext(DbContextOptions options)
            : base(options)
        {

        }
        
    }
}
