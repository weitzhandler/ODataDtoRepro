using System;
using System.ComponentModel.DataAnnotations;

namespace ODataDtoRepro.Models
{
  public abstract class ContactBase
  {
    public int Id { get; set; }
    public abstract string DisplayName { get; set; }
    public abstract ContactType ContactType { get; set; }
  }

  public abstract class Contact : ContactBase
  {
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
    public override string DisplayName
    {
      get => $"{FirstName} {LastName}".Trim();
      set { }
    }

    public override ContactType ContactType
    {
      get => ContactType.Person;
      set { }
    }
  }

  public class Company : Contact
  {
    public string CompanyName { get; set; }

    public override string DisplayName
    {
      get => CompanyName;
      set { }
    }

    public override ContactType ContactType
    {
      get => ContactType.Company;
      set { }
    }
  }

  public class ContactDto : ContactBase
  {
    public ContactDto(Contact contact)
    {
      Id = contact.Id;
      DisplayName = contact.DisplayName;
      ContactType = contact.ContactType;
    }

    public override string DisplayName { get; set; }

    public override ContactType ContactType { get; set; }
  }

  public enum ContactType
  {
    Person,
    Company
  }


}
