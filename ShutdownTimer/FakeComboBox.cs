using System.Drawing;
using System.Windows.Forms;

namespace ShutdownTimer
{
    class FakeComboBox : ComboBox
    {
        private PictureBox _fake;

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
                this.Parent.Controls.Remove(_fake);
                _fake.Dispose();
                _fake = null;
            }
        }
    }
}
