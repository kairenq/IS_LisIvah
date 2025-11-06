using System;
using System.Drawing;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

namespace AdmissionSystem.Forms
{
    public partial class RegisterForm : Form
    {
        private TextBox txtLogin;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private TextBox txtFullName;
        private Button btnRegister;
        private Button btnCancel;
        private Label lblTitle;
        private Panel panelMain;
        private CheckBox chkShowPassword;

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(500, 650);
            this.Text = "Регистрация нового пользователя";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ColorTranslator.FromHtml("#f0f2f5");

            // Главная панель
            panelMain = new Panel
            {
                Size = new Size(400, 550),
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
                Text = "РЕГИСТРАЦИЯ",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#4caf50"),
                Size = new Size(360, 50),
                Location = new Point(20, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // ФИО
            Label lblFullName = new Label
            {
                Text = "ФИО:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 100),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtFullName = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 130),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Логин
            Label lblLogin = new Label
            {
                Text = "Логин:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 180),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtLogin = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 210),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Пароль
            Label lblPassword = new Label
            {
                Text = "Пароль:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 260),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 290),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Подтверждение пароля
            Label lblConfirmPassword = new Label
            {
                Text = "Подтверждение пароля:",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                Size = new Size(360, 25),
                Location = new Point(20, 340),
                ForeColor = ColorTranslator.FromHtml("#333333")
            };

            txtConfirmPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Size = new Size(360, 35),
                Location = new Point(20, 370),
                UseSystemPasswordChar = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Показать пароль
            chkShowPassword = new CheckBox
            {
                Text = "Показать пароль",
                Font = new Font("Segoe UI", 9),
                Size = new Size(200, 25),
                Location = new Point(20, 415),
                ForeColor = ColorTranslator.FromHtml("#666666")
            };
            chkShowPassword.CheckedChanged += (s, e) =>
            {
                txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
                txtConfirmPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
            };

            // Кнопка регистрации
            btnRegister = new Button
            {
                Text = "ЗАРЕГИСТРИРОВАТЬСЯ",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(360, 45),
                Location = new Point(20, 455),
                BackColor = ColorTranslator.FromHtml("#4caf50"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = ColorTranslator.FromHtml("#43a047");
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = ColorTranslator.FromHtml("#4caf50");

            // Кнопка отмены
            btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Size = new Size(360, 45),
                Location = new Point(20, 510),
                BackColor = ColorTranslator.FromHtml("#757575"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            btnCancel.MouseEnter += (s, e) => btnCancel.BackColor = ColorTranslator.FromHtml("#616161");
            btnCancel.MouseLeave += (s, e) => btnCancel.BackColor = ColorTranslator.FromHtml("#757575");

            // Добавление элементов на панель
            panelMain.Controls.Add(lblTitle);
            panelMain.Controls.Add(lblFullName);
            panelMain.Controls.Add(txtFullName);
            panelMain.Controls.Add(lblLogin);
            panelMain.Controls.Add(txtLogin);
            panelMain.Controls.Add(lblPassword);
            panelMain.Controls.Add(txtPassword);
            panelMain.Controls.Add(lblConfirmPassword);
            panelMain.Controls.Add(txtConfirmPassword);
            panelMain.Controls.Add(chkShowPassword);
            panelMain.Controls.Add(btnRegister);
            panelMain.Controls.Add(btnCancel);

            this.Controls.Add(panelMain);
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Пожалуйста, заполните все поля!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать минимум 3 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password.Length < 4)
            {
                MessageBox.Show("Пароль должен содержать минимум 4 символа!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User newUser = new User
            {
                Login = login,
                Password = password,
                FullName = fullName,
                Role = "User"
            };

            if (DatabaseHelper.RegisterUser(newUser))
            {
                MessageBox.Show("Регистрация прошла успешно!\nТеперь вы можете войти в систему.",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Пользователь с таким логином уже существует!",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
