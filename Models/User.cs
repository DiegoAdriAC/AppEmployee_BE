using System.ComponentModel.DataAnnotations;

namespace AppEmployee.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Pasword { get; set; }
    }
}
