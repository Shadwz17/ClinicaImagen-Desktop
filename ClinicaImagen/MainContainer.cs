using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClinicaImagen
{
    public class MainContainer : Form
    {
        private readonly Panel _contentPanel;
        private Form? _currentView;
        public static MainContainer? Current { get; private set; }

        public MainContainer()
        {
            Text = "Clinica Imagen";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1024, 640);
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = UIStyles.BackgroundColor,
                Padding = new Padding(16)
            };

            Controls.Add(_contentPanel);
            Current = this;
            UIStyles.ApplyFormStyles(this);
        }

        public void ShowView(Form view)
        {
            if (_currentView != null)
            {
                _currentView.Hide();
                _currentView.Dispose();
            }

            _currentView = view;
            view.TopLevel = false;
            view.FormBorderStyle = FormBorderStyle.None;
            view.Dock = DockStyle.Fill;
            view.StartPosition = FormStartPosition.CenterParent;
            _contentPanel.Controls.Clear();
            _contentPanel.Controls.Add(view);
            UIStyles.ApplyFormStyles(view);
            view.Show();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_currentView == null)
            {
                ShowView(new FormLogin());
            }
        }
    }
}
