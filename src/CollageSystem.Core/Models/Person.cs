using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollageSystem.Core.Models
{
    public abstract class Person : BaseEntity, IEquatable<Person>
    {
        public int Age { get; set; }

        [MaxLength(13)] public string PhoneNumber { get; set; } = string.Empty;

        public Address? Address { get; set; }

        [ForeignKey(nameof(Address))] public int? AddressId { get; set; }

        public string Email { get; set; }

        public bool Equals(Person? other)
        {
            if (other == null) return false;
            if (other == this) return true;
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            var mulValue = 7;
            return new Object().GetHashCode() * mulValue + 2;
        }
    }
}