using System;

namespace ShoppingSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Discount{ get; set; }
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Customer second)
                return this.Id == second.Id && this.FirstName == second.FirstName && this.LastName == second.LastName &&
                    this.Address == second.Address && this.Discount == second.Discount;
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Address, Discount);
        }
    }
}
