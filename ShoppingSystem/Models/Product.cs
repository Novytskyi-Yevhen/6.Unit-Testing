using System;

namespace ShoppingSystem.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Product second)
                return this.Id == second.Id && this.Name == second.Name &&  this.Price == second.Price;
            return this.GetHashCode() == obj.GetHashCode();
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id,Name,Price);
        }
    }
}