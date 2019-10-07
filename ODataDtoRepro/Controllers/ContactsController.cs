using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ODataDtoRepro.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataDtoRepro.Controllers
{
  [ODataFormatting]
  [ODataRouting]
  [ApiExplorerSettings(IgnoreApi = true)]

  public class ContactsController : Controller
  {                                               
    IEnumerable<Contact> Contacts
    {
      get
      {
        yield return new Person { Id = 1, FirstName = "John", LastName = "Doe" };
        yield return new Company { Id = 2, CompanyName = "Microsoft" };
      }
    }


    [EnableQuery]
    public ActionResult<IQueryable<ContactDto>> Get() =>
      Ok(Contacts.Select(c => new ContactDto(c)));

    [EnableQuery]
    public ActionResult<Contact> Get(int key) =>
      Contacts.SingleOrDefault(c => c.Id == key);
  }
}
