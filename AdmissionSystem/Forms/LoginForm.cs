using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class LoginForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private LinkLabel linkInstruction;
        private Label lblTitle;
        private Label lblLogin;
        private Label lblPassword;
        private Panel panelMain;
        private CheckBox chkShowPassword;

        public LoginForm()
        {
            InitializeComponent();
            SetupCustomDesign();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(700, 700);
            this.Text = "Приемная комиссия - Вход";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#f0f2f5");

            // Главная панель
            panelMain = new Panel
            {
                Size = new Size(600, 600),
                Location = new Point(50, 50),
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            panelMain.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(ColorTranslator.FromHtml("#e0e0e0"), 1),
                    0, 0, panelMain.Width - 1, panelMain.Height - 1);
            };

            // Заголовок
            lblTitle = new Label
            {
                Text = "ПРИЕМНАЯ КОМИССИЯ",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1976d2"),
                Size = new Size(560, 60),
                Location = new Point(20, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Вход в систему",
                Font = new Font("Segoe UI", 14, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#666666"),
                Size = new Size(560, 35),
                Location = new Point(20, 110),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Логин
            lblLogin = new Label
            {
                Text = "Логин:",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(560, 30),
                Location = new Point(20, 180),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtLogin = new TextBox
            {
                Font = new Font("Segoe UI", 13),
                Size = new Size(560, 40),
                Location = new Point(20, 215),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Пароль
            lblPassword = new Label
            {
                Text = "Пароль:",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Size = new Size(560, 30),
                Location = new Point(20, 275),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 13),
                Size = new Size(560, 40),
                Location = new Point(20, 310),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароль",
                Font = new Font("Segoe UI", 10),
                Size = new Size(200, 30),
                Location = new Point(20, 365),
                ForeColor = ColorTranslator.FromHtml("#666666")
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка входа
            btnLogin = new Button
            {
                Text = "ВОЙТИ",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Size = new Size(560, 50),
                Location = new Point(20, 415),
                BackColor = ColorTranslator.FromHtml("#1976d2"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = ColorTranslator.FromHtml("#1565c0");
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = ColorTranslator.FromHtml("#1976d2");

            // Кнопка регистрации
            btnRegister = new Button
            {
                Text = "РЕГИСТРАЦИЯ",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                Size = new Size(560, 50),
                Location = new Point(20, 480),
                BackColor = ColorTranslator.FromHtml("#4caf50"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = ColorTranslator.FromHtml("#43a047");
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = ColorTranslator.FromHtml("#4caf50");

            // Ссылка на инструкцию
            linkInstruction = new LinkLabel
            {
                Text = "📖 Инструкция пользователя",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Size = new Size(560, 30),
                Location = new Point(20, 545),
                TextAlign = ContentAlignment.MiddleCenter,
                LinkColor = ColorTranslator.FromHtml("#1976d2"),
                Cursor = Cursors.Hand
            };
            linkInstruction.LinkClicked += LinkInstruction_LinkClicked;

            // Добавление элементов на панель
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(lblSubtitle);
            panelMain.Controls.Add(lblLogin);
            panelMain.Controls.Add(txtLogin);
            panelMain.Controls.Add(lblPassword);
            panelMain.Controls.Add(txtPassword);
            panelMain.Controls.Add(chkShowPassword);
            panelMain.Controls.Add(btnLogin);
            panelMain.Controls.Add(btnRegister);
            panelMain.Controls.Add(linkInstruction);

            this.Controls.Add(panelMain);

            // Enter для входа
            this.AcceptButton = btnLogin;
        }

        private void SetupCustomDesign()
        {
            // Дополнительные настройки дизайна
            this.DoubleBuffered = true;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User user = DatabaseHelper.GetUser(login, password);

            if (user != null)
            {
                this.Hide();

                if (user.Role == "Admin")
                {
                    AdminPanel adminPanel = new AdminPanel(user);
                    adminPanel.FormClosed += (s, args) => this.Close();
                    adminPanel.Show();
                }
                else
                {
                    UserPanel userPanel = new UserPanel(user);
                    userPanel.FormClosed += (s, args) => this.Close();
                    userPanel.Show();
                }
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void LinkInstruction_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Сначала ищем в корне проекта
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Инструкция_пользователя.docx");
            string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Инструкция_пользователя.docx");

            string instructionPath = File.Exists(rootPath) ? rootPath : resourcesPath;

            if (File.Exists(instructionPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = Path.GetFullPath(instructionPath),
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть инструкцию: {ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл инструкции не найден!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
