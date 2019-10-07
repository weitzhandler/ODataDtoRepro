using System.ComponentModel.DataAnnotations;

namespace ODataDtoRepro.Models
{
  public interface IContact
  {
    int Id { get; }
    string DisplayName { get; }
  }

  public abstract class Contact : IContact
  {
    public int Id { get; set; }

    public abstract string DisplayName { get; }

    // Another gazillion properties
  }

  public class Person : Contact
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }

#if !SERVER
    string _DisplayName { get; set; }
#endif
    /// <summary>
    /// Auto generated property, setter is no-op, and is required for EF infrastructure.
    /// </summary>
    [Required]
    [StringLength(129, MinimumLength = 2)]
    public override string DisplayName => $"{FirstName} {LastName}".Trim();

  }

  public class Company : Contact
  {
    public string CompanyName { get; set; }

    public override string DisplayName => CompanyName;
  }

  public class ContactDto : IContact
  {
    public ContactDto(Contact contact)
    {
      Id = contact.Id;
      DisplayName = contact.DisplayName;
    }

    public int Id { get; set; }

    public string DisplayName { get; set; }
  }


}
