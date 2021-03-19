using System;

namespace ShoppingSystem.Models
{
    public class Supermarket
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Supermarket second)
                return this.Id == second.Id && this.Name == second.Name && this.Address == second.Address;
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Address);
        }

    }
}