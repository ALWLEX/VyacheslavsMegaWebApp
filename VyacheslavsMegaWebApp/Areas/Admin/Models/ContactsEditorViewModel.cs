using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    [Area("Admin")]
    public class ContactsEditorViewModel
    {
        List<Contact> _contacts;

        public ContactsEditorViewModel(List<Contact> contacts) 
        { 
            _contacts = contacts;
        }

        public List<Contact> Contacts => _contacts;
    }
}
