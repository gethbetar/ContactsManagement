using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManagement.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        public string  LastName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]
        public string  Phone { get; set; }
        public string Address { get; set; }
    }


    public interface IContactRepository
    {
        Contact GetContact(int Id);
        IEnumerable<Contact> GetAllContact();
        Contact Add(Contact contact);
        Contact Update(Contact contactChanges);
        Contact Delete(int id);
    }


    public class SQLContactRepository : IContactRepository
    {
        private readonly AppDbContext context;

        public SQLContactRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Contact Add(Contact contact)
        {
            context.Contacts.Add(contact);
            context.SaveChanges();
            return contact;
        }

        public Contact Delete(int id)
        {
            Contact Contact = context.Contacts.Find(id);
            if (Contact != null)
            {
                context.Contacts.Remove(Contact);
                context.SaveChanges();
            }
            return Contact;
        }

        public IEnumerable<Contact> GetAllContact()
        {
            return context.Contacts;
        }

        public Contact GetContact(int Id)
        {
            return context.Contacts.Find(Id);
        }

        public Contact Update(Contact contactChanges)
        {
            var Contact = context.Contacts.Attach(contactChanges);
            Contact.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return contactChanges;
        }
    }
}
