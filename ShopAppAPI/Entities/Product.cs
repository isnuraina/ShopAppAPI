using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAppAPI.Entities
{
    public class Product:BaseEntity 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsDelete { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
