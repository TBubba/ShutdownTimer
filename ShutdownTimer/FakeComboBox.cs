using System.Drawing;
using System.Windows.Forms;

namespace ShutdownTimer
{
    class FakeComboBox : ComboBox
    {
        private PictureBox _fake;
        private Color _backColor;
        private Color _foreColor;

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                if (!this.DesignMode)
                    DisplayFake(value);
                base.Enabled = value;
            }
        }

        private void DisplayFake(bool enabled)
        {
            if (!enabled)
            {
                // Keep current colors
                _backColor = BackColor;
                _foreColor = ForeColor;

                // Show "grayed" colors
                BackColor = Color.FromKnownColor(KnownColor.InactiveCaptionText);
                ForeColor = Color.FromKnownColor(KnownColor.InactiveCaption);

                // Create fake combo box in front of the actual
                _fake = new PictureBox();
                _fake.Location = this.Location;
                _fake.Size = this.Size;
                var bmp = new Bitmap(_fake.Size.Width, _fake.Size.Height);
                this.DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
                _fake.Image = bmp;
                this.Parent.Controls.Add(_fake);
                _fake.BringToFront();
            }
            else
            {
                if (_fake == null) // Check if the combo box has been enabled before
                    return;

                // Switch colors (from grayed to normal)
                BackColor = _backColor;
                ForeColor = _foreColor;

                // Remove fake combo box
                this.Parent.Controls.Remove(_fake);
                _fake.Dispose();
                _fake = null;
            }
        }
    }
}
