using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab03WebApiOrdenesCompras.Models;

[Table("Customer")]
[Index("LastName", "FirstName", Name = "IndexCustomerName")]
public partial class Customer
{
    [Key]
    public int Id { get; set; }

    [StringLength(40)]
    public string FirstName { get; set; } = null!;

    [StringLength(40)]
    public string LastName { get; set; } = null!;

    [StringLength(40)]
    public string? City { get; set; }

    [StringLength(40)]
    public string? Country { get; set; }

    [StringLength(20)]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "El número debe tener 10 dígitos dahhhhhhh.")]
    public string? Phone { get; set; }
    //Correo electrónico 
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string? Email { get; set; }
    //Fecha 

    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? FechaNacimiento { get; set; }

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();


}
