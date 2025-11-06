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
            this.Size = new Size(500, 600);
            this.Text = "Приемная комиссия - Вход";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#f0f2f5");

            // Главная панель
            panelMain = new Panel
            {
                Size = new Size(400, 500),
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
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1976d2"),
                Size = new Size(360, 50),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Подзаголовок
            Label lblSubtitle = new Label
            {
                Text = "Вход в систему",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = ColorTranslator.FromHtml("#666666"),
                Size = new Size(360, 30),
                Location = new Point(20, 85),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Логин
            lblLogin = new Label
            {
                Text = "Логин:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 140),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtLogin = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 170),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Пароль
            lblPassword = new Label
            {
                Text = "Пароль:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 220),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 250),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароль",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(20, 295),
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
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(360, 45),
                Location = new Point(20, 340),
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
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(360, 45),
                Location = new Point(20, 395),
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
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 455),
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
            string instructionPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Resources", "Инструкция_пользователя.docx");

            if (File.Exists(instructionPath))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = instructionPath,
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
