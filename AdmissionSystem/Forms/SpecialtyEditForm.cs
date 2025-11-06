using System;
using System.Drawing;
using System.Windows.Forms;
using AdmissionSystem.Database;
using AdmissionSystem.Models;

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
            this.Size = new Size(500, 550);
            this.Text = isEditMode ? "Редактирование специальности" : "Добавление специальности";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.White;

            Label lblTitle = new Label
            {
                Text = isEditMode ? "РЕДАКТИРОВАНИЕ СПЕЦИАЛЬНОСТИ" : "НОВАЯ СПЕЦИАЛЬНОСТЬ",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#1976d2"),
                Size = new Size(450, 40),
                Location = new Point(25, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Название
            Label lblName = new Label
            {
                Text = "Название специальности:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 80),
                Size = new Size(450, 20)
            };

            txtName = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 105),
                Size = new Size(450, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Код
            Label lblCode = new Label
            {
                Text = "Код специальности:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 150),
                Size = new Size(450, 20)
            };

            txtCode = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 175),
                Size = new Size(450, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Количество мест
            Label lblPlaces = new Label
            {
                Text = "Количество мест:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 220),
                Size = new Size(450, 20)
            };

            numPlaces = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 245),
                Size = new Size(450, 30),
                Minimum = 1,
                Maximum = 500,
                Value = 25
            };

            // Минимальный балл
            Label lblMinScore = new Label
            {
                Text = "Минимальный балл:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 290),
                Size = new Size(450, 20)
            };

            numMinScore = new NumericUpDown
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 315),
                Size = new Size(450, 30),
                Minimum = 0,
                Maximum = 300,
                Value = 150
            };

            // Описание
            Label lblDescription = new Label
            {
                Text = "Описание:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 360),
                Size = new Size(450, 20)
            };

            txtDescription = new TextBox
            {
                Font = new Font("Segoe UI", 10),
                Location = new Point(25, 385),
                Size = new Size(450, 60),
                Multiline = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Кнопка сохранения
            btnSave = new Button
            {
                Text = "СОХРАНИТЬ",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(25, 465),
                BackColor = ColorTranslator.FromHtml("#4caf50"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Кнопка отмены
            btnCancel = new Button
            {
                Text = "ОТМЕНА",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(255, 465),
                BackColor = ColorTranslator.FromHtml("#757575"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblCode);
            this.Controls.Add(txtCode);
            this.Controls.Add(lblPlaces);
            this.Controls.Add(numPlaces);
            this.Controls.Add(lblMinScore);
            this.Controls.Add(numMinScore);
            this.Controls.Add(lblDescription);
            this.Controls.Add(txtDescription);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadSpecialtyData()
        {
            txtName.Text = specialty.Name;
            txtCode.Text = specialty.Code;
            numPlaces.Value = specialty.PlacesCount;
            numMinScore.Value = specialty.MinScore;
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
                    specialty.MinScore = (int)numMinScore.Value;
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
                        MinScore = (int)numMinScore.Value,
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
