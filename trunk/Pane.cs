using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PDFBun {
    public partial class Pane : UserControl {
        public Pane() {
            InitializeComponent();
        }

        private void Pane_Load(object sender, EventArgs e) {
            DoubleBuffered = true;
        }

        public String fpSrc = String.Empty;
        public int iPage = 0;

        Bitmap ima = null;

        public Bitmap Image { get { return ima; } set { ima = value; Invalidate(); } }

        int cxThumb = 202;

        public int ThumbnailWidth { get { return cxThumb; } set { cxThumb = value; Invalidate(); } }

        public override Size GetPreferredSize(Size proposedSize) {
            return new Size(cxThumb + 6, cxThumb);
        }

        private void Pane_Paint(object sender, PaintEventArgs e) {
            Rectangle rc0 = ClientRectangle;
            int w = Math.Min(rc0.Width, rc0.Height);
            Rectangle rc1 = Rectangle.FromLTRB(rc0.Right - w, rc0.Bottom - w, rc0.Right, rc0.Bottom);
            if (ima == null) return;
            Graphics cv = e.Graphics;
            cv.DrawRectangle(new Pen(Color.FromArgb(50, ForeColor)), Rectangle.FromLTRB(rc1.Left, rc1.Top, rc1.Right - 1, rc1.Bottom - 1));
            {
                int xc = (rc1.X + rc1.Right) / 2;
                int yc = (rc1.Y + rc1.Bottom) / 2;
                cv.DrawImageUnscaled(ima, xc - ima.Width / 2, yc - ima.Height / 2);
            }
            if (killMe) {
                Rectangle rc3 = Rectangle.FromLTRB(rc1.X + 3, rc1.Y + 3, rc1.Right - 3, rc1.Bottom - 3);
                cv.DrawLine(penX, rc3.X, rc3.Y, rc3.Right, rc3.Bottom);
                cv.DrawLine(penX, rc3.Right, rc3.Y, rc3.X, rc3.Bottom);
            }
            if (splitHere) {
                cv.DrawLine(penI, rc0.X + 1, rc1.Y, rc0.X + 1, rc1.Bottom);
            }
        }

        Pen penX = new Pen(Color.Red, 2);
        Pen penI = new Pen(Color.Blue, 3);

        private void Pane_PaddingChanged(object sender, EventArgs e) {
            Invalidate();
        }

        public bool DeleteMe { get { return killMe; } }
        public bool SplitFirst { get { return splitHere; } }

        bool killMe = false;
        bool splitHere = false;
        int step = -1;

        private void Pane_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                step = 0;
            }
            else if (e.Button == MouseButtons.Right) {
                splitHere = !splitHere;
                Invalidate();
            }
        }

        private void Pane_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                killMe = !killMe;
                Invalidate();
            }
        }

        private void Pane_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                step = -1;
            }
        }

        private void Pane_MouseMove(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                if (step >= 0) {
                    step++;
                    if (step == 5) {
                        DoDragDrop(Name, DragDropEffects.Copy);
                    }
                }
            }
        }
    }
}
