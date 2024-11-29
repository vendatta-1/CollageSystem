namespace CollageSystem.Core.Models;
public class Address : BaseEntity
{
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string Country { get; set; }
    public int? PersonId { get; set; }
    public Person? Person { get; set; }
}