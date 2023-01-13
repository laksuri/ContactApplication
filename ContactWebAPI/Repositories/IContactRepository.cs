using ContactWebAPI.Model;

namespace ContactWebAPI.Repositories
{
    public interface IContactRepository
    {
        Task<List<ContactModel>> GetContacts();
        Task<bool> EditContacts(ContactModel contact);
        Task<bool> AddContact (ContactModel contact);
        Task<ContactModel> GetContactById(int id);
        
    }
}
