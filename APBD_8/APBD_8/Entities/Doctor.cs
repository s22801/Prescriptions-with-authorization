using APBD_8.Entities;
using System.Collections.Generic;


public class Doctor
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    
    public virtual ICollection<Prescription> Prescriptions { get; set; }

    public Doctor()
    {
        Prescriptions = new HashSet<Prescription>();
    }
}