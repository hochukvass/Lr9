using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContactManagerApp;
using System;

namespace ContactManagerApp.Tests
{
    [TestClass]
    public class ContactManagerTests
    {
        [TestMethod]
        public void AddContact_ValidData_ShouldIncreaseCount()
        {
            // Arrange
            var manager = new ContactManager();

            // Act
            manager.AddContact("Иван", "123-45-67");

            // Assert
            Assert.AreEqual(1, manager.Contacts.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddContact_EmptyName_ShouldThrowException()
        {
            var manager = new ContactManager();
            manager.AddContact("", "123-45-67");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddContact_SqlInjectionInName_ShouldThrowException()
        {
            var manager = new ContactManager();
            manager.AddContact("Robert'; DROP TABLE Students;--", "123-45-67");
        }

        [TestMethod]
        public void RemoveContact_ExistingContact_ShouldDecreaseCount()
        {
            // Arrange
            var manager = new ContactManager();
            manager.AddContact("Сергей", "555-55-55");

            // Act
            manager.RemoveContact("Сергей");

            // Assert
            Assert.AreEqual(0, manager.Contacts.Count);
        }

        [TestMethod]
        public void FindContact_ExistingContact_ShouldReturnContact()
        {
            // Arrange
            var manager = new ContactManager();
            manager.AddContact("Дмитрий", "333-33-33");

            // Act
            var contact = manager.FindContact("Дмитрий");

            // Assert
            Assert.IsNotNull(contact);
            Assert.AreEqual("333-33-33", contact.Phone);
        }
    }
}