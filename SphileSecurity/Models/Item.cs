
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SphileSecurity.Models
{

    public class Item
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemCode { get; set; }
        [Required]
        [ForeignKey("Department")]
        [Display(Name = "Category")]
        public int Department_ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MinLength(3)]
        [MaxLength(80)]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [MinLength(3)]
        [MaxLength(255)]
        public string Description { get; set; }
        [Display(Name = "Quantity in Stock")]
        public int QuantityInStock { get; set; }
        //[Required]
        [Display(Name = "Picture")]
        //[DataType(DataType.Upload)]
        public byte[] Picture { get; set; }
        [Required]
        [Display(Name = "Proposed Price")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
     
        public string ImgPath { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<CartItem> Cart_Items { get; set; }
        //Supplier Who supplies the product
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<ProductOrder> FoodOrders { get; set; }
        public Item()
        {
            ImgPath = "~/Content/Images/Vehicle.png";
        }
       
    }

}