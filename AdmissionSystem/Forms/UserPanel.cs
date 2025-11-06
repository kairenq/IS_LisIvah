using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class UserPanel : Form
    {
        private User currentUser;
        private TabControl tabControl;
        private DataGridView dgvSpecialties;
        private DataGridView dgvMyApplications;

        public UserPanel(User user)
        {
            currentUser = user;
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(1400, 850);
            this.Text = "Личный кабинет абитуриента";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorTranslator.FromHtml("#f0f2f5");

            // Заголовок
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = ColorTranslator.FromHtml("#4caf50")
            };

            Label lblWelcome = new Label
            {
                Text = $"Добро пожаловать, {currentUser.FullName}",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblRole = new Label
            {
                Text = "Абитуриент",
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

            // Вкладка "Специальности"
            TabPage tabSpecialties = new TabPage("🎓 Доступные специальности");
            InitializeSpecialtiesTab(tabSpecialties);

            // Вкладка "Мои заявления"
            TabPage tabMyApplications = new TabPage("📋 Мои заявления");
            InitializeMyApplicationsTab(tabMyApplications);

            tabControl.TabPages.Add(tabSpecialties);
            tabControl.TabPages.Add(tabMyApplications);

            this.Controls.Add(tabControl);
            this.Controls.Add(headerPanel);
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

            Button btnRefresh = new Button
            {
                Text = "🔄 Обновить",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(20, 10),
                BackColor = ColorTranslator.FromHtml("#2196f3"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadSpecialties();

            Button btnApply = new Button
            {
                Text = "➕ Подать заявление",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(160, 40),
                Location = new Point(160, 10),
                BackColor = ColorTranslator.FromHtml("#4caf50"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) => ApplyToSpecialty();

            toolPanel.Controls.Add(btnRefresh);
            toolPanel.Controls.Add(btnApply);

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

        private void InitializeMyApplicationsTab(TabPage tab)
        {
            tab.BackColor = Color.White;

            Panel toolPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            Button btnRefresh = new Button
            {
                Text = "🔄 Обновить",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(20, 10),
                BackColor = ColorTranslator.FromHtml("#2196f3"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadMyApplications();

            Button btnDelete = new Button
            {
                Text = "🗑 Удалить",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(130, 40),
                Location = new Point(160, 10),
                BackColor = ColorTranslator.FromHtml("#f44336"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteMyApplication();

            toolPanel.Controls.Add(btnRefresh);
            toolPanel.Controls.Add(btnDelete);

            dgvMyApplications = new DataGridView
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

            tab.Controls.Add(dgvMyApplications);
            tab.Controls.Add(toolPanel);
        }

        private void LoadData()
        {
            LoadSpecialties();
            LoadMyApplications();
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

        private void LoadMyApplications()
        {
            var applications = DatabaseHelper.GetUserApplications(currentUser.Id);
            var displayData = applications.Select(a => new
            {
                Id = a.Id,
                Специальность = DatabaseHelper.GetSpecialtyById(a.SpecialtyId)?.Name ?? "Неизвестно",
                ФИО = $"{a.LastName} {a.FirstName} {a.MiddleName}",
                Баллы = a.ExamScore,
                Статус = a.Status,
                Дата = DateTime.Parse(a.SubmissionDate.ToString()).ToString("dd.MM.yyyy")
            }).ToList();

            dgvMyApplications.DataSource = displayData;
            if (dgvMyApplications.Columns.Contains("Id"))
                dgvMyApplications.Columns["Id"].Visible = false;
        }

        private void ApplyToSpecialty()
        {
            if (dgvSpecialties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите специальность!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int specialtyId = (int)dgvSpecialties.SelectedRows[0].Cells["Id"].Value;
            var specialty = DatabaseHelper.GetAllSpecialties().FirstOrDefault(s => s.Id == specialtyId);

            if (specialty != null)
            {
                ApplicationForm form = new ApplicationForm(currentUser, specialty);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadMyApplications();
                }
            }
        }

        private void DeleteMyApplication()
        {
            if (dgvMyApplications.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите заявление!", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Вы уверены, что хотите удалить заявление?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int id = (int)dgvMyApplications.SelectedRows[0].Cells["Id"].Value;
                DatabaseHelper.DeleteApplication(id);
                LoadMyApplications();
                MessageBox.Show("Заявление удалено!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
