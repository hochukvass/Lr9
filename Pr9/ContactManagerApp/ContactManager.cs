using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactManagerApp
{
    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public Contact(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }

        public override string ToString()
        {
            return $"{Name} - {Phone}";
        }
    }

    public class ContactManager
    {
        public List<Contact> Contacts { get; private set; } = new List<Contact>();

        public void AddContact(string name, string phone)
        {
            // Проверка безопасности: валидация ввода
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя не может быть пустым");

            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Телефон не может быть пустым");

            // Проверка на SQL-инъекции (базовая защита)
            if (name.Contains(";") || name.Contains("'") || name.Contains("--"))
                throw new ArgumentException("Недопустимые символы в имени");

            if (phone.Contains(";") || phone.Contains("'") || phone.Contains("--"))
                throw new ArgumentException("Недопустимые символы в телефоне");

            // Проверка на существующий контакт
            if (Contacts.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Контакт с таким именем уже существует");

            Contacts.Add(new Contact(name.Trim(), phone.Trim()));
        }

        public void RemoveContact(string name)
        {
            var contact = Contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (contact != null)
            {
                Contacts.Remove(contact);
            }
        }

        public Contact FindContact(string name)
        {
            return Contacts.FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Contact> GetAllContacts()
        {
            return new List<Contact>(Contacts);
        }
    }
}