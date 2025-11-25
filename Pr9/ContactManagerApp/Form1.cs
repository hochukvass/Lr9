using System;
using System.Drawing;
using System.Windows.Forms;

namespace ContactManagerApp
{
    public partial class Form1 : Form
    {
        private ContactManager _contactManager = new ContactManager();

        private TextBox txtName;
        private TextBox txtPhone;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnFind;
        private ListBox lstContacts;
        private Label lblName;
        private Label lblPhone;

        public Form1()
        {
            InitializeComponent();
            CreateControls();
            UpdateContactsList();
        }

        private void CreateControls()
        {
            this.Text = "Менеджер контактов";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Метка "Имя"
            lblName = new Label();
            lblName.Text = "Имя:";
            lblName.Location = new Point(20, 20);
            lblName.Size = new Size(50, 20);
            this.Controls.Add(lblName);

            // Поле ввода имени
            txtName = new TextBox();
            txtName.Location = new Point(80, 20);
            txtName.Size = new Size(150, 20);
            this.Controls.Add(txtName);

            // Метка "Телефон"
            lblPhone = new Label();
            lblPhone.Text = "Телефон:";
            lblPhone.Location = new Point(20, 50);
            lblPhone.Size = new Size(60, 20);
            this.Controls.Add(lblPhone);

            // Поле ввода телефона
            txtPhone = new TextBox();
            txtPhone.Location = new Point(80, 50);
            txtPhone.Size = new Size(150, 20);
            this.Controls.Add(txtPhone);

            // Кнопка "Добавить"
            btnAdd = new Button();
            btnAdd.Text = "Добавить";
            btnAdd.Location = new Point(250, 20);
            btnAdd.Size = new Size(80, 25);
            btnAdd.Click += btnAdd_Click;
            this.Controls.Add(btnAdd);

            // Кнопка "Удалить"
            btnRemove = new Button();
            btnRemove.Text = "Удалить";
            btnRemove.Location = new Point(250, 50);
            btnRemove.Size = new Size(80, 25);
            btnRemove.Click += btnRemove_Click;
            this.Controls.Add(btnRemove);

            // Кнопка "Найти"
            btnFind = new Button();
            btnFind.Text = "Найти";
            btnFind.Location = new Point(340, 20);
            btnFind.Size = new Size(80, 25);
            btnFind.Click += btnFind_Click;
            this.Controls.Add(btnFind);

            // Список контактов
            lstContacts = new ListBox();
            lstContacts.Location = new Point(20, 90);
            lstContacts.Size = new Size(440, 250);
            this.Controls.Add(lstContacts);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text;
                string phone = txtPhone.Text;

                _contactManager.AddContact(name, phone);

                txtName.Clear();
                txtPhone.Clear();

                UpdateContactsList();
                MessageBox.Show("Контакт успешно добавлен!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstContacts.SelectedItem != null)
            {
                string selectedContact = lstContacts.SelectedItem.ToString();
                string name = selectedContact.Split('-')[0].Trim();

                _contactManager.RemoveContact(name);
                UpdateContactsList();

                MessageBox.Show("Контакт удален!", "Успех",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Выберите контакт для удаления!", "Ошибка",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            if (!string.IsNullOrEmpty(name))
            {
                var contact = _contactManager.FindContact(name);
                if (contact != null)
                {
                    MessageBox.Show($"Найден контакт: {contact.Name} - {contact.Phone}",
                                  "Результат поиска",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Контакт не найден!", "Результат поиска",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void UpdateContactsList()
        {
            lstContacts.Items.Clear();
            foreach (var contact in _contactManager.GetAllContacts())
            {
                lstContacts.Items.Add(contact.ToString());
            }
        }
    }
}