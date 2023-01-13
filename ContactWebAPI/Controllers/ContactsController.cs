using ContactWebAPI.Model;
using ContactWebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactWebAPI.Controllers
{
    
    [ApiController]
    public class ContactsController : ControllerBase
    {
        IContactRepository _contactRepository;
        public ContactsController(IContactRepository contactRepository )
        {
            _contactRepository = contactRepository;
        }
        [HttpGet]
        [Route("api/getcontacts")]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var contacts = await _contactRepository.GetContacts();
                if(contacts==null)
                {
                    return NotFound();
                }
                return Ok(contacts);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("api/editcontacts")]
        public async Task<IActionResult> EditContacts([FromBody] ContactModel contact)
        {
            try
            {
                bool result = await _contactRepository.EditContacts(contact);
                if(!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("api/addcontact")]
        public async Task<IActionResult> AddContact([FromBody] ContactModel contact)
        {
            try
            {
                bool result = await _contactRepository.AddContact(contact);
                if (!result)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/getcontactbyid")]
        public async Task<IActionResult> GetContactById(int id)
        {
            try
            {
                var result = await _contactRepository.GetContactById(id);
                if (result==null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
