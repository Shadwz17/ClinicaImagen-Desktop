using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ClinicaImagen
{
    internal static class UIStyles
    {
        private static Icon? _sharedIcon;

        public static readonly Color PrimaryColor = Color.FromArgb(17, 94, 140);
        public static readonly Color SecondaryColor = Color.FromArgb(6, 129, 156);
        public static readonly Color BackgroundColor = Color.FromArgb(245, 249, 252);
        public static readonly Color SurfaceColor = Color.White;
        public static readonly Color TextColor = Color.FromArgb(33, 37, 41);
        public static readonly Color MutedTextColor = Color.FromArgb(108, 117, 125);

        public static readonly Font HeadingFont = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font SubtitleFont = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font BodyFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);

        public static void ApplyFormStyles(Form form)
        {
            form.BackColor = BackgroundColor;
            form.ForeColor = TextColor;
            form.Font = BodyFont;
            form.AutoScaleMode = AutoScaleMode.Dpi;
            var icon = GetAppIcon();
            if (icon != null)
            {
                form.Icon = icon;
            }

            foreach (Control control in form.Controls)
            {
                ApplyControlStyles(control);
            }
        }

        private static Icon? GetAppIcon()
        {
            if (_sharedIcon != null)
            {
                return _sharedIcon;
            }

            _sharedIcon = (Icon)SystemIcons.Application.Clone();
            return _sharedIcon;
        }

        private static void ApplyControlStyles(Control control)
        {
            control.ForeColor = TextColor;
            control.Font = BodyFont;

            switch (control)
            {
                case Button button:
                    button.BackColor = PrimaryColor;
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderSize = 0;
                    button.ForeColor = Color.White;
                    button.Font = SubtitleFont;
                    button.Padding = new Padding(8, 4, 8, 4);
                    break;
                case Label label when label.Font.Size > 12:
                    label.Font = HeadingFont;
                    label.ForeColor = TextColor;
                    break;
                case Label label:
                    label.Font = BodyFont;
                    label.ForeColor = label.ForeColor == Color.Black ? TextColor : label.ForeColor;
                    break;
                case TextBox textBox:
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = SurfaceColor;
                    break;
                case DataGridView grid:
                    StyleGrid(grid);
                    break;
                case GroupBox groupBox:
                    groupBox.Font = SubtitleFont;
                    groupBox.ForeColor = TextColor;
                    groupBox.BackColor = SurfaceColor;
                    break;
                case Panel panel:
                    panel.BackColor = panel.Parent?.BackColor ?? BackgroundColor;
                    break;
            }

            foreach (Control child in control.Controls)
            {
                ApplyControlStyles(child);
            }
        }

        private static void StyleGrid(DataGridView grid)
        {
            grid.BackgroundColor = SurfaceColor;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = SubtitleFont;
            grid.DefaultCellStyle.Font = BodyFont;
            grid.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 243, 248);
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.RowHeadersVisible = false;
        }

        public static void ApplyEmptyState(DataGridView grid, string emptyMessage)
        {
            if (grid.Parent == null)
            {
                return;
            }

            var existing = grid.Parent.Controls.OfType<Label>()
                .FirstOrDefault(l => l.Tag != null && l.Tag.Equals(grid.Name + "_empty"));

            if (existing == null)
            {
                existing = new Label
                {
                    Tag = grid.Name + "_empty",
                    Text = emptyMessage,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = MutedTextColor,
                    Font = SubtitleFont,
                    Visible = false,
                    AccessibleName = "Mensaje de tabla vacía",
                    AccessibleDescription = emptyMessage
                };
                grid.Parent.Controls.Add(existing);
                existing.BringToFront();
            }

            void UpdateVisibility()
            {
                bool empty = grid.Rows.Count == 0;
                existing.Visible = empty;
                existing.Enabled = empty;
                grid.AccessibleDescription = empty ? emptyMessage : string.Empty;
            }

            grid.DataBindingComplete -= GridOnDataBindingComplete;
            grid.DataBindingComplete += GridOnDataBindingComplete;
            grid.DataError -= GridOnDataError;
            grid.DataError += GridOnDataError;
            UpdateVisibility();

            void GridOnDataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
            {
                UpdateVisibility();
            }

            void GridOnDataError(object? sender, DataGridViewDataErrorEventArgs e)
            {
                MessageBox.Show("No pudimos mostrar algunos datos. Revisa la conexión o vuelve a intentarlo.",
                    "Error de datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                grid.AccessibleDescription = "Error al cargar los datos";
            }
        }
    }
}
