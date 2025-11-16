using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;
using AdmissionSystem.UI;

namespace AdmissionSystem.Forms
{
    public partial class SpecialtyEditForm : Form
    {
        private Specialty specialty;
        private bool isEditMode;

        private TextBox txtName;
        private TextBox txtCode;
        private NumericUpDown numPlaces;
        private NumericUpDown numMinScore;
        private TextBox txtDescription;
        private Button btnSave;
        private Button btnCancel;

        public SpecialtyEditForm(Specialty existingSpecialty = null)
        {
            specialty = existingSpecialty;
            isEditMode = specialty != null;
            InitializeComponent();

            if (isEditMode)
            {
                LoadSpecialtyData();
            }
        }

        private void InitializeComponent()
        {
            this.Size = new Size(650, 700);
            this.Text = isEditMode ? "Редактирование специальности" : "Добавление специальности";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = ModernUIHelper.DarkBackground;
            this.DoubleBuffered = true;

            // Фоновый градиент
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    this.ClientRectangle,
                    ModernUIHelper.DarkBackground,
                    ColorTranslator.FromHtml("#16192e"),
                    90F))
                {
                    e.Graphics.FillRectangle(brush, this.ClientRectangle);
                }
            };

            // Главная карточка
            Panel cardPanel = ModernUIHelper.CreateCard(new Point(75, 50), new Size(500, 600));

            Label lblTitle = new Label
            {
                Text = isEditMode ? "РЕДАКТИРОВАТЬ СПЕЦИАЛЬНОСТЬ" : "НОВАЯ СПЕЦИАЛЬНОСТЬ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ModernUIHelper.TextPrimary,
                Size = new Size(460, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            // Название
            Label lblName = ModernUIHelper.CreateModernLabel(
                "НАЗВАНИЕ СПЕЦИАЛЬНОСТИ", new Point(40, 80), 9, FontStyle.Bold, ModernUIHelper.TextMuted);

            Panel panelNameBox = new Panel
            {
                Location = new Point(40, 105),
                Size = new Size(420, 40),
                BackColor = ModernUIHelper.SidebarBackground
            };
            txtName = new TextBox
            {
                Location = new Point(10, 9),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelNameBox.Controls.Add(txtName);

            // Код
            Label lblCode = ModernUIHelper.CreateModernLabel(
                "КОД СПЕЦИАЛЬНОСТИ", new Point(40, 165), 9, FontStyle.Bold, ModernUIHelper.TextMuted);

            Panel panelCodeBox = new Panel
            {
                Location = new Point(40, 190),
                Size = new Size(420, 40),
                BackColor = ModernUIHelper.SidebarBackground
            };
            txtCode = new TextBox
            {
                Location = new Point(10, 9),
                Size = new Size(400, 30),
                Font = new Font("Segoe UI", 10),
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None
            };
            panelCodeBox.Controls.Add(txtCode);

            // Количество мест и минимальный балл в одной строке
            Label lblPlaces = ModernUIHelper.CreateModernLabel(
                "КОЛИЧЕСТВО МЕСТ", new Point(40, 250), 9, FontStyle.Bold, ModernUIHelper.TextMuted);

            numPlaces = ModernUIHelper.CreateModernNumericUpDown(
                new Point(40, 275), new Size(190, 40), 1, 500, 25);

            Label lblMinScore = ModernUIHelper.CreateModernLabel(
                "МИНИМАЛЬНЫЙ БАЛЛ", new Point(270, 250), 9, FontStyle.Bold, ModernUIHelper.TextMuted);

            numMinScore = ModernUIHelper.CreateModernNumericUpDown(
                new Point(270, 275), new Size(190, 40), 2.0M, 5.0M, 4.0M, 2);

            // Описание
            Label lblDescription = ModernUIHelper.CreateModernLabel(
                "ОПИСАНИЕ", new Point(40, 335), 9, FontStyle.Bold, ModernUIHelper.TextMuted);

            Panel panelDescBox = new Panel
            {
                Location = new Point(40, 360),
                Size = new Size(420, 100),
                BackColor = ModernUIHelper.SidebarBackground
            };
            txtDescription = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(400, 80),
                Font = new Font("Segoe UI", 10),
                BackColor = ModernUIHelper.SidebarBackground,
                ForeColor = ModernUIHelper.TextPrimary,
                BorderStyle = BorderStyle.None,
                Multiline = true
            };
            panelDescBox.Controls.Add(txtDescription);

            // Кнопки
            btnSave = ModernUIHelper.CreateGradientButton(
                "СОХРАНИТЬ",
                new Point(40, 490),
                new Size(190, 45),
                ModernUIHelper.SuccessColor,
                ColorTranslator.FromHtml("#00a67d")
            );
            btnSave.Click += BtnSave_Click;

            btnCancel = ModernUIHelper.CreateGradientButton(
                "ОТМЕНА",
                new Point(270, 490),
                new Size(190, 45),
                ModernUIHelper.DangerColor,
                ColorTranslator.FromHtml("#e66565")
            );
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblName);
            cardPanel.Controls.Add(panelNameBox);
            cardPanel.Controls.Add(lblCode);
            cardPanel.Controls.Add(panelCodeBox);
            cardPanel.Controls.Add(lblPlaces);
            cardPanel.Controls.Add(numPlaces);
            cardPanel.Controls.Add(lblMinScore);
            cardPanel.Controls.Add(numMinScore);
            cardPanel.Controls.Add(lblDescription);
            cardPanel.Controls.Add(panelDescBox);
            cardPanel.Controls.Add(btnSave);
            cardPanel.Controls.Add(btnCancel);

            this.Controls.Add(cardPanel);
        }

        private void LoadSpecialtyData()
        {
            txtName.Text = specialty.Name;
            txtCode.Text = specialty.Code;
            numPlaces.Value = specialty.PlacesCount;
            numMinScore.Value = (decimal)specialty.MinScore;
            txtDescription.Text = specialty.Description;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название специальности!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCode.Text))
            {
                MessageBox.Show("Введите код специальности!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (isEditMode)
                {
                    specialty.Name = txtName.Text.Trim();
                    specialty.Code = txtCode.Text.Trim();
                    specialty.PlacesCount = (int)numPlaces.Value;
                    specialty.MinScore = (double)numMinScore.Value;
                    specialty.Description = txtDescription.Text.Trim();

                    DatabaseHelper.UpdateSpecialty(specialty);
                    MessageBox.Show("Специальность успешно обновлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Specialty newSpecialty = new Specialty
                    {
                        Name = txtName.Text.Trim(),
                        Code = txtCode.Text.Trim(),
                        PlacesCount = (int)numPlaces.Value,
                        MinScore = (double)numMinScore.Value,
                        Description = txtDescription.Text.Trim()
                    };

                    DatabaseHelper.AddSpecialty(newSpecialty);
                    MessageBox.Show("Специальность успешно добавлена!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
