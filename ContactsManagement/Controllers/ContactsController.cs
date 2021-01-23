using ContactsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManagement.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactRepository _ContactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _ContactRepository = contactRepository;
        }

        public ActionResult List()
        {
            var model = _ContactRepository.GetAllContact();
            return View(model);
        }


        public ViewResult Show(int id)
        {

            Contact contact = _ContactRepository.GetContact(id);

            if (contact == null)
            {
                Response.StatusCode = 404;
                return View("ContactNotFound", id);
            }

            return View(contact);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact model)
        {
            if (ModelState.IsValid)
            {

                Contact newcontact = new Contact
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Address = model.Address
                };

                _ContactRepository.Add(newcontact);
                return RedirectToAction("Show", new { id = newcontact.Id });
            }

            return View();
        }

        [HttpGet]
        public ViewResult Update(int id)
        {
            Contact contact = _ContactRepository.GetContact(id);

            if (contact == null)
            {
                Response.StatusCode = 404;
                return View("ContactNotFound", id);
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult Update(Contact model)
        {
            if (ModelState.IsValid)
            {
                Contact contact = _ContactRepository.GetContact(model.Id);
                if (contact == null)
                {
                    Response.StatusCode = 404;
                    return View("ContactNotFound", model.Id);
                }
                contact.FirstName = model.FirstName;
                contact.LastName = model.LastName;
                contact.Email = model.Email;
                contact.Phone = model.Phone;
                contact.Address = model.Address;


                _ContactRepository.Update(contact);
                return RedirectToAction("List");
            }

            return View();
        }


        [HttpGet]
        public ViewResult Delete(int id)
        {
            Contact contact = _ContactRepository.GetContact(id);

            if (contact == null)
            {
                Response.StatusCode = 404;
                return View("ContactNotFound", id);
            }

            return View(contact);
        }

        [HttpPost]
        public IActionResult Delete(Contact model)
        {
            _ContactRepository.Delete(model.Id);
            return RedirectToAction("List");
        }

    }
}
