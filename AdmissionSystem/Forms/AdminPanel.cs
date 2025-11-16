using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;
using AdmissionSystem.UI;

namespace AdmissionSystem.Forms
{
    public partial class AdminPanel : Form
    {
        private User currentUser;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel applicationsPanel;
        private Panel specialtiesPanel;
        private Panel usersPanel;
        private DataGridView dgvUsers;
        private DataGridView dgvSpecialties;
        private DataGridView dgvApplications;
        private Button btnApplicationsNav;
        private Button btnSpecialtiesNav;
        private Button btnUsersNav;
        private Label lblPageTitle;

        public AdminPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowApplicationsPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1500, 900);
            this.Text = "Панель администратора";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Боковая панель навигации
            sidebarPanel = ModernUIHelper.CreateSidebar(new Size(280, 900));

            // Логотип и приветствие
            Label lblLogo = new Label
            {
                Text = "⚙️",
                Font = new Font("Segoe UI", 48),
                ForeColor = ModernUIHelper.PrimaryAccent,
                Size = new Size(280, 80),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblWelcome = new Label
            {
                Text = "ПАНЕЛЬ\nАДМИНИСТРАТОРА",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(280, 70),
                Location = new Point(0, 120),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblUserName = new Label
            {
                Text = currentUser.FullName,
                Font = new Font("Segoe UI", 11),
                ForeColor = ModernUIHelper.TextSecondary,
                Size = new Size(260, 40),
                Location = new Point(10, 190),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Panel divider = ModernUIHelper.CreateDivider(new Point(20, 240), 240);

            // Кнопки навигации
            btnApplicationsNav = ModernUIHelper.CreateSidebarButton("📋  Заявления", 270, true);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

            btnSpecialtiesNav = ModernUIHelper.CreateSidebarButton("🎓  Специальности", 340);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnUsersNav = ModernUIHelper.CreateSidebarButton("👥  Пользователи", 410);
            btnUsersNav.Click += (s, e) => ShowUsersPanel();

            // Кнопка выхода
            Button btnLogout = new Button
            {
                Text = "🚪  Выход",
                Location = new Point(0, 800),
                Size = new Size(280, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12),
                ForeColor = ModernUIHelper.DangerColor,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2d3561");
            btnLogout.Click += (s, e) =>
            {
                this.Close();
            };

            sidebarPanel.Controls.Add(lblLogo);
            sidebarPanel.Controls.Add(lblWelcome);
            sidebarPanel.Controls.Add(lblUserName);
            sidebarPanel.Controls.Add(divider);
            sidebarPanel.Controls.Add(btnApplicationsNav);
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnUsersNav);
            sidebarPanel.Controls.Add(btnLogout);

            // Панель контента
            contentPanel = new Panel
            {
                Location = new Point(280, 0),
                Size = new Size(1220, 900),
                BackColor = ModernUIHelper.CardBackground
            };

            // Заголовок страницы
            lblPageTitle = new Label
            {
                Text = "УПРАВЛЕНИЕ ЗАЯВЛЕНИЯМИ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1200, 60),
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateApplicationsPanel();
            CreateSpecialtiesPanel();
            CreateUsersPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = true
            };

            // DataGridView для заявлений
            dgvApplications = new DataGridView
            {
                Location = new Point(0, 70),
                Size = new Size(1160, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvApplications);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent
            };

            Button btnApprove = ModernUIHelper.CreateGradientButton(
                "✓  ОДОБРИТЬ",
                new Point(0, 10),
                new Size(220, 50),
                ModernUIHelper.SuccessColor,
                ColorTranslator.FromHtml("#00a67d")
            );
            btnApprove.Click += (s, e) => ChangeApplicationStatus("Одобрено");

            Button btnReject = ModernUIHelper.CreateGradientButton(
                "✗  ОТКЛОНИТЬ",
                new Point(240, 10),
                new Size(220, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnReject.Click += (s, e) => ChangeApplicationStatus("Отклонено");

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "🗑  УДАЛИТЬ",
                new Point(480, 10),
                new Size(220, 50),
                ColorTranslator.FromHtml("#636e72"),
                ColorTranslator.FromHtml("#535c62")
            );
            btnDelete.Click += (s, e) => DeleteApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "🔄  ОБНОВИТЬ",
                new Point(720, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefresh.Click += (s, e) => LoadApplications();

            buttonPanel.Controls.Add(btnApprove);
            buttonPanel.Controls.Add(btnReject);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            applicationsPanel.Controls.Add(dgvApplications);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для специальностей
            dgvSpecialties = new DataGridView
            {
                Location = new Point(0, 70),
                Size = new Size(1160, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvSpecialties);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent
            };

            Button btnAdd = ModernUIHelper.CreateGradientButton(
                "➕  ДОБАВИТЬ",
                new Point(0, 10),
                new Size(220, 50),
                ModernUIHelper.SuccessColor,
                ColorTranslator.FromHtml("#00a67d")
            );
            btnAdd.Click += (s, e) => AddSpecialty();

            Button btnEdit = ModernUIHelper.CreateGradientButton(
                "✏  ИЗМЕНИТЬ",
                new Point(240, 10),
                new Size(220, 50),
                ModernUIHelper.WarningColor,
                ColorTranslator.FromHtml("#f4c05e")
            );
            btnEdit.Click += (s, e) => EditSpecialty();

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "🗑  УДАЛИТЬ",
                new Point(480, 10),
                new Size(220, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnDelete.Click += (s, e) => DeleteSpecialty();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "🔄  ОБНОВИТЬ",
                new Point(720, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnAdd);
            buttonPanel.Controls.Add(btnEdit);
            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateUsersPanel()
        {
            usersPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = false
            };

            // DataGridView для пользователей
            dgvUsers = new DataGridView
            {
                Location = new Point(0, 70),
                Size = new Size(1160, 550),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ModernUIHelper.StyleDataGridView(dgvUsers);

            // Панель с кнопками
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 640),
                Size = new Size(1160, 80),
                BackColor = Color.Transparent
            };

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "🗑  УДАЛИТЬ",
                new Point(0, 10),
                new Size(220, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnDelete.Click += (s, e) => DeleteUser();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "🔄  ОБНОВИТЬ",
                new Point(240, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefresh.Click += (s, e) => LoadUsers();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            usersPanel.Controls.Add(dgvUsers);
            usersPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(usersPanel);
        }

        private void ShowApplicationsPanel()
        {
            applicationsPanel.Visible = true;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = false;

            btnApplicationsNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextPrimary;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ЗАЯВЛЕНИЯМИ";
        }

        private void ShowSpecialtiesPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = true;
            usersPanel.Visible = false;

            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;
            btnSpecialtiesNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextPrimary;
            btnUsersNav.BackColor = Color.Transparent;
            btnUsersNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ СПЕЦИАЛЬНОСТЯМИ";
        }

        private void ShowUsersPanel()
        {
            applicationsPanel.Visible = false;
            specialtiesPanel.Visible = false;
            usersPanel.Visible = true;

            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;
            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnUsersNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnUsersNav.ForeColor = ModernUIHelper.TextPrimary;

            lblPageTitle.Text = "УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ";
        }

        private void LoadData()
        {
            try
            {
                if (dgvApplications != null)
                    LoadApplications();
                else
                    MessageBox.Show("ERROR: dgvApplications is null!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (dgvSpecialties != null)
                    LoadSpecialties();
                else
                    MessageBox.Show("ERROR: dgvSpecialties is null!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (dgvUsers != null)
                    LoadUsers();
                else
                    MessageBox.Show("ERROR: dgvUsers is null!", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных:\n\n{ex.Message}\n\nStack Trace:\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadApplications()
        {
            List<Models.Application> applications = DatabaseHelper.GetAllApplications();
            dgvApplications.DataSource = null;
            dgvApplications.DataSource = applications;

            if (dgvApplications.Columns.Count > 0 && dgvApplications.Columns["Id"] != null)
            {
                dgvApplications.Columns["Id"].HeaderText = "ID";
                dgvApplications.Columns["Id"].Width = 60;
                if (dgvApplications.Columns["UserId"] != null)
                    dgvApplications.Columns["UserId"].Visible = false;
                if (dgvApplications.Columns["SpecialtyId"] != null)
                    dgvApplications.Columns["SpecialtyId"].Visible = false;
                if (dgvApplications.Columns["FullName"] != null)
                    dgvApplications.Columns["FullName"].HeaderText = "ФИО";
                if (dgvApplications.Columns["BirthDate"] != null)
                    dgvApplications.Columns["BirthDate"].HeaderText = "Дата рождения";
                if (dgvApplications.Columns["PassportSeries"] != null)
                    dgvApplications.Columns["PassportSeries"].HeaderText = "Серия паспорта";
                if (dgvApplications.Columns["PassportNumber"] != null)
                    dgvApplications.Columns["PassportNumber"].HeaderText = "Номер паспорта";
                if (dgvApplications.Columns["Address"] != null)
                    dgvApplications.Columns["Address"].HeaderText = "Адрес";
                if (dgvApplications.Columns["Phone"] != null)
                    dgvApplications.Columns["Phone"].HeaderText = "Телефон";
                if (dgvApplications.Columns["Email"] != null)
                    dgvApplications.Columns["Email"].HeaderText = "Email";
                if (dgvApplications.Columns["ExamScore"] != null)
                    dgvApplications.Columns["ExamScore"].HeaderText = "Средний балл";
                if (dgvApplications.Columns["Status"] != null)
                    dgvApplications.Columns["Status"].HeaderText = "Статус";
                if (dgvApplications.Columns["SpecialtyName"] != null)
                    dgvApplications.Columns["SpecialtyName"].HeaderText = "Специальность";
                if (dgvApplications.Columns["SubmittedAt"] != null)
                    dgvApplications.Columns["SubmittedAt"].HeaderText = "Дата подачи";
            }
        }

        private void LoadSpecialties()
        {
            List<Specialty> specialties = DatabaseHelper.GetAllSpecialties();
            dgvSpecialties.DataSource = null;
            dgvSpecialties.DataSource = specialties;

            if (dgvSpecialties.Columns.Count > 0 && dgvSpecialties.Columns["Id"] != null)
            {
                dgvSpecialties.Columns["Id"].HeaderText = "ID";
                dgvSpecialties.Columns["Id"].Width = 60;
                if (dgvSpecialties.Columns["Name"] != null)
                    dgvSpecialties.Columns["Name"].HeaderText = "Название";
                if (dgvSpecialties.Columns["Code"] != null)
                    dgvSpecialties.Columns["Code"].HeaderText = "Код";
                if (dgvSpecialties.Columns["PlacesCount"] != null)
                    dgvSpecialties.Columns["PlacesCount"].HeaderText = "Мест";
                if (dgvSpecialties.Columns["MinScore"] != null)
                    dgvSpecialties.Columns["MinScore"].HeaderText = "Мин. балл";
                if (dgvSpecialties.Columns["Description"] != null)
                    dgvSpecialties.Columns["Description"].HeaderText = "Описание";
            }
        }

        private void LoadUsers()
        {
            List<User> users = DatabaseHelper.GetAllUsers();
            dgvUsers.DataSource = null;
            dgvUsers.DataSource = users;

            if (dgvUsers.Columns.Count > 0 && dgvUsers.Columns["Id"] != null)
            {
                dgvUsers.Columns["Id"].HeaderText = "ID";
                dgvUsers.Columns["Id"].Width = 60;
                if (dgvUsers.Columns["Login"] != null)
                    dgvUsers.Columns["Login"].HeaderText = "Логин";
                if (dgvUsers.Columns["Password"] != null)
                    dgvUsers.Columns["Password"].Visible = false;
                if (dgvUsers.Columns["FullName"] != null)
                    dgvUsers.Columns["FullName"].HeaderText = "ФИО";
                if (dgvUsers.Columns["Role"] != null)
                    dgvUsers.Columns["Role"].HeaderText = "Роль";
            }
        }

        private void ChangeApplicationStatus(string status)
        {
            if (dgvApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявление!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (Models.Application)dgvApplications.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Изменить статус заявления на '{status}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.UpdateApplicationStatus(application.Id, status);
                LoadApplications();
                MessageBox.Show("Статус успешно изменен!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteApplication()
        {
            if (dgvApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявление для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var application = (Models.Application)dgvApplications.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show("Удалить выбранное заявление?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.DeleteApplication(application.Id);
                LoadApplications();
                MessageBox.Show("Заявление успешно удалено!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Выберите специальность для редактирования!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;
            SpecialtyEditForm form = new SpecialtyEditForm(specialty);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSpecialties();
            }
        }

        private void DeleteSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;

            if (MessageBox.Show($"Удалить специальность '{specialty.Name}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.DeleteSpecialty(specialty.Id);
                LoadSpecialties();
                MessageBox.Show("Специальность успешно удалена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteUser()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите пользователя для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = (User)dgvUsers.SelectedRows[0].DataBoundItem;

            if (user.Role == "Admin")
            {
                MessageBox.Show("Невозможно удалить администратора!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show($"Удалить пользователя '{user.FullName}'?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DatabaseHelper.DeleteUser(user.Id);
                LoadUsers();
                MessageBox.Show("Пользователь успешно удален!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
