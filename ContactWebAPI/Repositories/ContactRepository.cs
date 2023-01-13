using ContactWebAPI.Context;
using ContactWebAPI.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ContactWebAPI.Repositories
{
    public class ContactRepository:IContactRepository
    {
        ContactContext dbContext;
        public ContactRepository(ContactContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<ContactModel>> GetContacts()
        {
            if(dbContext!=null)
            {
                return await dbContext.Contacts.ToListAsync();
            }
            return null;
        }
        public async Task<bool> EditContacts(ContactModel contact)
        {
            if(dbContext!=null)
            {
                var entity = dbContext.Contacts.Where(x => x.Id == contact.Id).FirstOrDefault();
                if (entity != null)
                {
                    entity = contact;
                    dbContext.ChangeTracker.Clear();
                    dbContext.Contacts.Update(entity);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                
            }
            return false;
        }
        public async Task<bool> AddContact(ContactModel contact)
        {
            if (dbContext != null)
            {
                dbContext.Contacts.Add(contact);
                await dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<ContactModel> GetContactById(int id)
        {
            if(dbContext!=null)
            {
                var entity = dbContext.Contacts.Where(x => x.Id==id).FirstOrDefault();
                return entity;
            }
            return null;
        }
    }
}
