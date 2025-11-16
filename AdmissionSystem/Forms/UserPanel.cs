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
    public partial class UserPanel : Form
    {
        private User currentUser;
        private Panel sidebarPanel;
        private Panel contentPanel;
        private Panel specialtiesPanel;
        private Panel applicationsPanel;
        private DataGridView dgvSpecialties;
        private DataGridView dgvApplications;
        private Button btnSpecialtiesNav;
        private Button btnApplicationsNav;
        private Label lblPageTitle;

        public UserPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
            ShowSpecialtiesPanel();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1500, 900);
            this.Text = "Панель пользователя";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Боковая панель навигации
            sidebarPanel = ModernUIHelper.CreateSidebar(new Size(280, 900));

            // Логотип и приветствие
            Label lblLogo = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI", 48),
                ForeColor = ModernUIHelper.SecondaryAccent,
                Size = new Size(280, 80),
                Location = new Point(0, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label lblWelcome = new Label
            {
                Text = "ЛИЧНЫЙ\nКАБИНЕТ",
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
            btnSpecialtiesNav = ModernUIHelper.CreateSidebarButton("🎓  Специальности", 270, true);
            btnSpecialtiesNav.Click += (s, e) => ShowSpecialtiesPanel();

            btnApplicationsNav = ModernUIHelper.CreateSidebarButton("📋  Мои заявления", 340);
            btnApplicationsNav.Click += (s, e) => ShowApplicationsPanel();

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
            sidebarPanel.Controls.Add(btnSpecialtiesNav);
            sidebarPanel.Controls.Add(btnApplicationsNav);
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
                Text = "ДОСТУПНЫЕ СПЕЦИАЛЬНОСТИ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(1200, 60),
                Location = new Point(40, 30),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(lblPageTitle);

            // Создаем панели для разных разделов
            CreateSpecialtiesPanel();
            CreateApplicationsPanel();

            this.Controls.Add(sidebarPanel);
            this.Controls.Add(contentPanel);
        }

        private void CreateSpecialtiesPanel()
        {
            specialtiesPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = true
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

            Button btnSubmit = ModernUIHelper.CreateGradientButton(
                "➕  ПОДАТЬ ЗАЯВЛЕНИЕ",
                new Point(0, 10),
                new Size(280, 50),
                ModernUIHelper.PrimaryAccent,
                ColorTranslator.FromHtml("#5f4dd4")
            );
            btnSubmit.Click += (s, e) => SubmitApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "🔄  ОБНОВИТЬ",
                new Point(300, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefresh.Click += (s, e) => LoadSpecialties();

            buttonPanel.Controls.Add(btnSubmit);
            buttonPanel.Controls.Add(btnRefresh);

            specialtiesPanel.Controls.Add(dgvSpecialties);
            specialtiesPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(specialtiesPanel);
        }

        private void CreateApplicationsPanel()
        {
            applicationsPanel = new Panel
            {
                Location = new Point(40, 110),
                Size = new Size(1160, 750),
                BackColor = Color.Transparent,
                Visible = false
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

            Button btnDelete = ModernUIHelper.CreateGradientButton(
                "🗑  УДАЛИТЬ",
                new Point(0, 10),
                new Size(220, 50),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnDelete.Click += (s, e) => DeleteApplication();

            Button btnRefresh = ModernUIHelper.CreateGradientButton(
                "🔄  ОБНОВИТЬ",
                new Point(240, 10),
                new Size(220, 50),
                ModernUIHelper.SecondaryAccent,
                ColorTranslator.FromHtml("#00b5ad")
            );
            btnRefresh.Click += (s, e) => LoadApplications();

            buttonPanel.Controls.Add(btnDelete);
            buttonPanel.Controls.Add(btnRefresh);

            applicationsPanel.Controls.Add(dgvApplications);
            applicationsPanel.Controls.Add(buttonPanel);

            contentPanel.Controls.Add(applicationsPanel);
        }

        private void ShowSpecialtiesPanel()
        {
            specialtiesPanel.Visible = true;
            applicationsPanel.Visible = false;

            btnSpecialtiesNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextPrimary;
            btnApplicationsNav.BackColor = Color.Transparent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextSecondary;

            lblPageTitle.Text = "ДОСТУПНЫЕ СПЕЦИАЛЬНОСТИ";
        }

        private void ShowApplicationsPanel()
        {
            specialtiesPanel.Visible = false;
            applicationsPanel.Visible = true;

            btnSpecialtiesNav.BackColor = Color.Transparent;
            btnSpecialtiesNav.ForeColor = ModernUIHelper.TextSecondary;
            btnApplicationsNav.BackColor = ModernUIHelper.PrimaryAccent;
            btnApplicationsNav.ForeColor = ModernUIHelper.TextPrimary;

            lblPageTitle.Text = "МОИ ЗАЯВЛЕНИЯ";
        }

        private void LoadData()
        {
            LoadSpecialties();
            LoadApplications();
        }

        private void LoadSpecialties()
        {
            List<Specialty> specialties = DatabaseHelper.GetAllSpecialties();
            dgvSpecialties.DataSource = null;
            dgvSpecialties.DataSource = specialties;

            if (dgvSpecialties.Columns.Count > 0)
            {
                dgvSpecialties.Columns["Id"].HeaderText = "ID";
                dgvSpecialties.Columns["Id"].Width = 60;
                dgvSpecialties.Columns["Name"].HeaderText = "Название";
                dgvSpecialties.Columns["Code"].HeaderText = "Код";
                dgvSpecialties.Columns["PlacesCount"].HeaderText = "Мест";
                dgvSpecialties.Columns["MinScore"].HeaderText = "Мин. балл";
                dgvSpecialties.Columns["Description"].HeaderText = "Описание";
            }
        }

        private void LoadApplications()
        {
            List<Models.Application> applications = DatabaseHelper.GetUserApplications(currentUser.Id);
            dgvApplications.DataSource = null;
            dgvApplications.DataSource = applications;

            if (dgvApplications.Columns.Count > 0)
            {
                dgvApplications.Columns["Id"].HeaderText = "ID";
                dgvApplications.Columns["Id"].Width = 60;
                dgvApplications.Columns["UserId"].Visible = false;
                dgvApplications.Columns["SpecialtyId"].Visible = false;
                dgvApplications.Columns["FullName"].HeaderText = "ФИО";
                dgvApplications.Columns["BirthDate"].HeaderText = "Дата рождения";
                dgvApplications.Columns["PassportSeries"].HeaderText = "Серия";
                dgvApplications.Columns["PassportNumber"].HeaderText = "Номер";
                dgvApplications.Columns["Address"].HeaderText = "Адрес";
                dgvApplications.Columns["Phone"].HeaderText = "Телефон";
                dgvApplications.Columns["Email"].HeaderText = "Email";
                dgvApplications.Columns["ExamScore"].HeaderText = "Средний балл";
                dgvApplications.Columns["Status"].HeaderText = "Статус";
                dgvApplications.Columns["SpecialtyName"].HeaderText = "Специальность";
                dgvApplications.Columns["SubmittedAt"].HeaderText = "Дата подачи";
            }
        }

        private void SubmitApplication()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var specialty = (Specialty)dgvSpecialties.SelectedRows[0].DataBoundItem;
            ApplicationForm form = new ApplicationForm(currentUser, specialty);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadApplications();
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
    }
}
