using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class AdminPanel : Form
    {
        private User currentUser;
        private TabControl tabControl;
        private DataGridView dgvUsers;
        private DataGridView dgvSpecialties;
        private DataGridView dgvApplications;
        private Label lblWelcome;

        public AdminPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 850);
            this.Text = "Панель администратора";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#f0f2f5");

            // Заголовок
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = ColorTranslator.FromHtml("#1976d2")
            };

            lblWelcome = new Label
            {
                Text = $"Добро пожаловать, {currentUser.FullName}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblRole = new Label
            {
                Text = "Администратор",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.White,
                Location = new Point(20, 45),
                AutoSize = true
            };

            Button btnLogout = new Button
            {
                Text = "ВЫХОД",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(120, 40),
                Location = new Point(1250, 20),
                BackColor = ColorTranslator.FromHtml("#f44336"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => { this.Close(); };

            headerPanel.Controls.Add(lblWelcome);
            headerPanel.Controls.Add(lblRole);
            headerPanel.Controls.Add(btnLogout);

            // TabControl
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Вкладка "Заявления"
            TabPage tabApplications = new TabPage("📋 Заявления");
            InitializeApplicationsTab(tabApplications);

            // Вкладка "Специальности"
            TabPage tabSpecialties = new TabPage("🎓 Специальности");
            InitializeSpecialtiesTab(tabSpecialties);

            // Вкладка "Пользователи"
            TabPage tabUsers = new TabPage("👥 Пользователи");
            InitializeUsersTab(tabUsers);

            tabControl.TabPages.Add(tabApplications);
            tabControl.TabPages.Add(tabSpecialties);
            tabControl.TabPages.Add(tabUsers);

            this.Controls.Add(tabControl);
            this.Controls.Add(headerPanel);
        }

        private void InitializeApplicationsTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            Panel toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            Button btnRefresh = CreateStyledButton("🔄 Обновить", 20, 10, ColorTranslator.FromHtml("#2196f3"));
            btnRefresh.Click += (s, e) => LoadApplications();

            Button btnApprove = CreateStyledButton("✓ Одобрить", 160, 10, ColorTranslator.FromHtml("#4caf50"));
            btnApprove.Click += (s, e) => ChangeApplicationStatus("Одобрено");

            Button btnReject = CreateStyledButton("✗ Отклонить", 300, 10, ColorTranslator.FromHtml("#f44336"));
            btnReject.Click += (s, e) => ChangeApplicationStatus("Отклонено");

            Button btnDelete = CreateStyledButton("🗑 Удалить", 440, 10, ColorTranslator.FromHtml("#757575"));
            btnDelete.Click += (s, e) => DeleteApplication();

            toolPanel.Controls.Add(btnRefresh);
            toolPanel.Controls.Add(btnApprove);
            toolPanel.Controls.Add(btnReject);
            toolPanel.Controls.Add(btnDelete);

            dgvApplications = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };

            tab.Controls.Add(dgvApplications);
            tab.Controls.Add(toolPanel);
        }

        private void InitializeSpecialtiesTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            Panel toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            Button btnRefresh = CreateStyledButton("🔄 Обновить", 20, 10, ColorTranslator.FromHtml("#2196f3"));
            btnRefresh.Click += (s, e) => LoadSpecialties();

            Button btnAdd = CreateStyledButton("➕ Добавить", 160, 10, ColorTranslator.FromHtml("#4caf50"));
            btnAdd.Click += (s, e) => AddSpecialty();

            Button btnEdit = CreateStyledButton("✏ Изменить", 300, 10, ColorTranslator.FromHtml("#ff9800"));
            btnEdit.Click += (s, e) => EditSpecialty();

            Button btnDelete = CreateStyledButton("🗑 Удалить", 440, 10, ColorTranslator.FromHtml("#f44336"));
            btnDelete.Click += (s, e) => DeleteSpecialty();

            toolPanel.Controls.Add(btnRefresh);
            toolPanel.Controls.Add(btnAdd);
            toolPanel.Controls.Add(btnEdit);
            toolPanel.Controls.Add(btnDelete);

            dgvSpecialties = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };

            tab.Controls.Add(dgvSpecialties);
            tab.Controls.Add(toolPanel);
        }

        private void InitializeUsersTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            Panel toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            Button btnRefresh = CreateStyledButton("🔄 Обновить", 20, 10, ColorTranslator.FromHtml("#2196f3"));
            btnRefresh.Click += (s, e) => LoadUsers();

            Button btnDelete = CreateStyledButton("🗑 Удалить", 160, 10, ColorTranslator.FromHtml("#f44336"));
            btnDelete.Click += (s, e) => DeleteUser();

            toolPanel.Controls.Add(btnRefresh);
            toolPanel.Controls.Add(btnDelete);

            dgvUsers = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9)
            };

            tab.Controls.Add(dgvUsers);
            tab.Controls.Add(toolPanel);
        }

        private Button CreateStyledButton(string text, int x, int y, Color color)
        {
            Button btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(x, y),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadData()
        {
            LoadApplications();
            LoadSpecialties();
            LoadUsers();
        }

        private void LoadApplications()
        {
            var applications = DatabaseHelper.GetAllApplications();
            var displayData = applications.Select(a => new
            {
                Id = a.Id,
                ФИО = $"{a.LastName} {a.FirstName} {a.MiddleName}",
                Специальность = DatabaseHelper.GetSpecialtyById(a.SpecialtyId)?.Name ?? "Неизвестно",
                Телефон = a.Phone,
                Email = a.Email,
                Баллы = a.ExamScore,
                Статус = a.Status,
                Дата = DateTime.Parse(a.SubmissionDate.ToString()).ToString("dd.MM.yyyy")
            }).ToList();

            dgvApplications.DataSource = displayData;
            if (dgvApplications.Columns.Contains("Id"))
                dgvApplications.Columns["Id"].Visible = false;
        }

        private void LoadSpecialties()
        {
            var specialties = DatabaseHelper.GetAllSpecialties();
            var displayData = specialties.Select(s => new
            {
                Id = s.Id,
                Название = s.Name,
                Код = s.Code,
                Мест = s.PlacesCount,
                МинБалл = s.MinScore,
                Описание = s.Description
            }).ToList();

            dgvSpecialties.DataSource = displayData;
            if (dgvSpecialties.Columns.Contains("Id"))
                dgvSpecialties.Columns["Id"].Visible = false;
        }

        private void LoadUsers()
        {
            var users = DatabaseHelper.GetAllUsers();
            var displayData = users.Select(u => new
            {
                Id = u.Id,
                Логин = u.Login,
                ФИО = u.FullName,
                Роль = u.Role,
                Дата_регистрации = DateTime.Parse(u.RegistrationDate.ToString()).ToString("dd.MM.yyyy")
            }).ToList();

            dgvUsers.DataSource = displayData;
            if (dgvUsers.Columns.Contains("Id"))
                dgvUsers.Columns["Id"].Visible = false;
        }

        private void ChangeApplicationStatus(string status)
        {
            if (dgvApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявление!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvApplications.SelectedRows[0].Cells["Id"].Value;
            var applications = DatabaseHelper.GetAllApplications();
            var application = applications.FirstOrDefault(a => a.Id == id);

            if (application != null)
            {
                application.Status = status;
                DatabaseHelper.UpdateApplication(application);
                LoadApplications();
                MessageBox.Show($"Статус заявления изменен на '{status}'", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteApplication()
        {
            if (dgvApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявление!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить это заявление?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = (int)dgvApplications.SelectedRows[0].Cells["Id"].Value;
                DatabaseHelper.DeleteApplication(id);
                LoadApplications();
                MessageBox.Show("Заявление удалено!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void AddSpecialty()
        {
            SpecialtyEditForm form = new SpecialtyEditForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSpecialties();
            }
        }

        private void EditSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvSpecialties.SelectedRows[0].Cells["Id"].Value;
            var specialty = DatabaseHelper.GetAllSpecialties().FirstOrDefault(s => s.Id == id);

            if (specialty != null)
            {
                SpecialtyEditForm form = new SpecialtyEditForm(specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadSpecialties();
                }
            }
        }

        private void DeleteSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить эту специальность?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = (int)dgvSpecialties.SelectedRows[0].Cells["Id"].Value;
                DatabaseHelper.DeleteSpecialty(id);
                LoadSpecialties();
                MessageBox.Show("Специальность удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = (int)dgvUsers.SelectedRows[0].Cells["Id"].Value;

            if (id == currentUser.Id)
            {
                MessageBox.Show("Вы не можете удалить сами себя!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DatabaseHelper.DeleteUser(id);
                LoadUsers();
                MessageBox.Show("Пользователь удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
