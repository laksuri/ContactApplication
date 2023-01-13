using ContactApplication.Models;
using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace ContactApplication.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index(string sortParam)
        {
            //Call Web API to retrieve the data
            IEnumerable<ContactModel> Contacts = new List<ContactModel>();
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://localhost:44305/api/");
                    var responseTask = client.GetAsync("getContacts");
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<ContactModel>>();
                        readTask.Wait();

                        Contacts = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..

                        Contacts = Enumerable.Empty<ContactModel>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "The API service is down");
                    return RedirectToAction("Error", "Home");
                }
                //Sorting Logic
                if (Contacts.Count() > 0)
                {
                    switch (sortParam)
                    {
                        case nameof(ContactModel.FirstName):
                            {
                                Contacts = Contacts.OrderBy(x => x.FirstName);
                                break;
                            }
                        case nameof(ContactModel.LastName):
                            {
                                Contacts = Contacts.OrderBy(x => x.LastName);
                                break;
                            }
                        case nameof(ContactModel.Email):
                            {
                                Contacts = Contacts.OrderBy(x => x.Email);
                                break;
                            }
                        case nameof(ContactModel.PhoneNumber):
                            {
                                Contacts = Contacts.OrderBy(x => x.PhoneNumber);
                                break;
                            }
                        case nameof(ContactModel.Address):
                            {
                                Contacts = Contacts.OrderBy(x => x.Address);
                                break;
                            }
                        case nameof(ContactModel.City):
                            {
                                Contacts = Contacts.OrderBy(x => x.City);
                                break;
                            }
                        case nameof(ContactModel.State):
                            {
                                Contacts = Contacts.OrderBy(x => x.State);
                                break;
                            }
                        case nameof(ContactModel.Country):
                            {
                                Contacts = Contacts.OrderBy(x => x.Country);
                                break;
                            }
                        case nameof(ContactModel.PostalCode):
                            {
                                Contacts = Contacts.OrderBy(x => x.PostalCode);
                                break;
                            }
                        default:
                            {
                                Contacts = Contacts.OrderByDescending(x => x.Id);
                                break;
                            }
                    }
                }
                return View(Contacts);
            }
        }
        public IActionResult EditContact(int id)
        {
            ContactModel contact = new ContactModel();
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44305/api/");
                    var responseTask = client.GetAsync(String.Format("getcontactbyid?id={0}", id));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ContactModel>();
                        readTask.Wait();

                        contact = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..


                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return RedirectToAction("Error", "Home");
            }
            return View(contact);
        }

        public IActionResult CreateNewContact()
        {
            return View();
        }
        public IActionResult Create(ContactModel contact)
        {
            try
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44305/api/");
                    var responseTask = client.PostAsJsonAsync<ContactModel>("addContact", contact);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else //web api sent error response 
                    {
                        //log response status here..


                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                    return View(contact);
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return RedirectToAction("Error", "Home");
            }

        }
        public IActionResult Edit(ContactModel contact)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44305/api/");
                    var responseTask = client.PostAsJsonAsync<ContactModel>("editcontacts", contact);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else //web api sent error response 
                    {
                        //log response status here..


                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                    return View(contact);
                }
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return RedirectToAction("Error", "Home");
            }
        }
        public IActionResult MapView(int id)
        {
            try
            {
                ContactModel contact = new ContactModel();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44305/api/");
                    var responseTask = client.GetAsync(String.Format("getcontactbyid?id={0}", id));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ContactModel>();
                        readTask.Wait();

                        contact = readTask.Result;
                    }
                    else //web api sent error response 
                    {
                        //log response status here..


                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                }
                //Integration with Google Location service
                using (var client = new HttpClient())
                {
                    try
                    {
                        string URI = string.Format("https://maps.googleapis.com/maps/api/geocode/xml?address={0}&key={1}", contact.City, "AIzaSyD8xOGUT_MLP_Q1f88A-5Jaw-FJa6P6TYU");
                        XmlDocument doc = new XmlDocument();
                        doc.Load(URI);

                        contact.Latitude = doc.SelectSingleNode("/GeocodeResponse/result/geometry/location/lat").InnerText;
                        contact.Longitude = doc.SelectSingleNode("/GeocodeResponse/result/geometry/location/lng").InnerText;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("Error", "Home");
                    }
                }

                return View(contact);
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                return RedirectToAction("Error", "Home");
            }
        }
    }

}