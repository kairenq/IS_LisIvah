using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AdmissionSystem.UI
{
    /// <summary>
    /// Современный UI helper с уникальной темной темой и градиентами
    /// </summary>
    public static class ModernUIHelper
    {
        // Темная цветовая схема
        public static readonly Color DarkBackground = ColorTranslator.FromHtml("#0a0e27");
        public static readonly Color CardBackground = ColorTranslator.FromHtml("#1a1d3a");
        public static readonly Color SidebarBackground = ColorTranslator.FromHtml("#16192e");

        // Акцентные цвета
        public static readonly Color PrimaryAccent = ColorTranslator.FromHtml("#6c5ce7");    // Фиолетовый
        public static readonly Color SecondaryAccent = ColorTranslator.FromHtml("#00cec9");  // Бирюзовый
        public static readonly Color SuccessColor = ColorTranslator.FromHtml("#00b894");     // Зеленый
        public static readonly Color DangerColor = ColorTranslator.FromHtml("#ff7675");      // Красный
        public static readonly Color WarningColor = ColorTranslator.FromHtml("#fdcb6e");     // Желтый

        // Текст
        public static readonly Color TextPrimary = ColorTranslator.FromHtml("#ffffff");
        public static readonly Color TextSecondary = ColorTranslator.FromHtml("#b2bec3");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#636e72");

        /// <summary>
        /// Создает стильную кнопку с градиентом
        /// </summary>
        public static Button CreateGradientButton(string text, Point location, Size size, Color startColor, Color endColor)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = TextPrimary,
                Cursor = Cursors.Hand,
                BackColor = startColor
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = endColor;

            // Рисуем градиент при отрисовке
            button.Paint += (s, e) =>
            {
                var btn = (Button)s;
                using (var brush = new LinearGradientBrush(
                    btn.ClientRectangle,
                    startColor,
                    endColor,
                    LinearGradientMode.Horizontal))
                {
                    e.Graphics.FillRectangle(brush, btn.ClientRectangle);
                }

                // Рисуем текст
                TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle,
                    btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };

            return button;
        }

        /// <summary>
        /// Создает стильное текстовое поле
        /// </summary>
        public static TextBox CreateModernTextBox(Point location, Size size, string placeholder = "")
        {
            var textBox = new TextBox
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = CardBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.None,
                Tag = placeholder
            };

            // Создаем панель-контейнер для границы
            var panel = new Panel
            {
                Location = location,
                Size = new Size(size.Width, size.Height + 10),
                BackColor = SidebarBackground,
                Padding = new Padding(2)
            };

            textBox.Location = new Point(10, 5);
            panel.Controls.Add(textBox);

            return textBox;
        }

        /// <summary>
        /// Создает современную метку
        /// </summary>
        public static Label CreateModernLabel(string text, Point location, int fontSize = 10,
            FontStyle style = FontStyle.Regular, Color? color = null)
        {
            return new Label
            {
                Text = text,
                Location = location,
                Font = new Font("Segoe UI", fontSize, style),
                ForeColor = color ?? TextSecondary,
                AutoSize = true,
                BackColor = Color.Transparent
            };
        }

        /// <summary>
        /// Создает панель-карточку
        /// </summary>
        public static Panel CreateCard(Point location, Size size)
        {
            var panel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = CardBackground,
                Padding = new Padding(20)
            };

            // Добавляем эффект тени через границу
            panel.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new Pen(Color.FromArgb(30, 255, 255, 255), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1);
                }
            };

            return panel;
        }

        /// <summary>
        /// Создает боковую панель навигации
        /// </summary>
        public static Panel CreateSidebar(Size size)
        {
            var sidebar = new Panel
            {
                Location = new Point(0, 0),
                Size = size,
                BackColor = SidebarBackground,
                Dock = DockStyle.Left
            };

            // Градиент на боковой панели
            sidebar.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(
                    sidebar.ClientRectangle,
                    ColorTranslator.FromHtml("#16192e"),
                    ColorTranslator.FromHtml("#0a0e27"),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, sidebar.ClientRectangle);
                }
            };

            return sidebar;
        }

        /// <summary>
        /// Создает кнопку для боковой панели
        /// </summary>
        public static Button CreateSidebarButton(string text, int yPosition, bool isActive = false)
        {
            var button = new Button
            {
                Text = "    " + text,
                Location = new Point(0, yPosition),
                Size = new Size(250, 55),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = isActive ? TextPrimary : TextSecondary,
                BackColor = isActive ? ColorTranslator.FromHtml("#6c5ce7") : Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2d3561");

            return button;
        }

        /// <summary>
        /// Стилизует DataGridView в темной теме
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = CardBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 45;

            // Стиль заголовков
            dgv.ColumnHeadersDefaultCellStyle.BackColor = SidebarBackground;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = SidebarBackground;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgv.ColumnHeadersHeight = 50;

            // Стиль ячеек
            dgv.DefaultCellStyle.BackColor = CardBackground;
            dgv.DefaultCellStyle.ForeColor = TextSecondary;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryAccent;
            dgv.DefaultCellStyle.SelectionForeColor = TextPrimary;
            dgv.DefaultCellStyle.Padding = new Padding(10, 5, 10, 5);

            dgv.GridColor = ColorTranslator.FromHtml("#2d3561");
        }

        /// <summary>
        /// Создает стильный NumericUpDown
        /// </summary>
        public static NumericUpDown CreateModernNumericUpDown(Point location, Size size,
            decimal min, decimal max, decimal value, int decimalPlaces = 0)
        {
            var numericUpDown = new NumericUpDown
            {
                Location = location,
                Size = size,
                Font = new Font("Segoe UI", 11),
                BackColor = CardBackground,
                ForeColor = TextPrimary,
                BorderStyle = BorderStyle.FixedSingle,
                Minimum = min,
                Maximum = max,
                Value = value,
                DecimalPlaces = decimalPlaces
            };

            return numericUpDown;
        }

        /// <summary>
        /// Создает разделитель
        /// </summary>
        public static Panel CreateDivider(Point location, int width)
        {
            return new Panel
            {
                Location = location,
                Size = new Size(width, 1),
                BackColor = ColorTranslator.FromHtml("#2d3561")
            };
        }
    }
}
