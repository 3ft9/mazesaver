using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MazeSaver
{
    public partial class ScreenSaverForm : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        private bool previewMode = false;
        private Point mouseLocation;
        private Random rand = new Random();

        private int boxsize = Settings.BoxSize;
        private int restartdelay = Settings.RestartDelay;

        private bool running = false;
        private bool drawing = false;
        private int finished = 0;
        private int width = 0;
        private int height = 0;
        private bool initialised = false;
        private Cell current = null;

        private List<Cell> cells;
        private List<Cell> path;

        private Graphics graphics;

        public ScreenSaverForm(Rectangle Bounds)
        {
            InitializeComponent();
            this.Bounds = Bounds;
        }

        public ScreenSaverForm(IntPtr PreviewWndHandle)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            boxsize = 15;

            previewMode = true;
        }

        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            Cursor.Hide();
            TopMost = true;

            drawTimer.Interval = 20;
            drawTimer.Tick += new EventHandler(drawTimer_Tick);
            drawTimer.Start();
        }

        private void drawTimer_Tick(object sender, System.EventArgs e)
        {
            if (!drawing)
            {
                drawing = true;
                if (!initialised)
                {
                    width = (Bounds.Width - 20) / boxsize;
                    height = (Bounds.Height - 20) / boxsize;

                    int xOffset = (Bounds.Width - (width * boxsize)) / 2;
                    int yOffset = (Bounds.Height - (height * boxsize)) / 2;

                    cells = new List<Cell>(width * height);
                    path = new List<Cell>(width * height);

                    Pen gridPen = new Pen(Color.Black);
                    Pen wallPen = new Pen(Color.White);
                    Pen invisiblePen = new Pen(Color.DarkSlateGray);

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            cells.Add(new Cell(x, y, boxsize, gridPen, wallPen, invisiblePen, Brushes.Cyan, Brushes.DarkSlateGray, xOffset, yOffset));
                        }
                    }

                    graphics = CreateGraphics();
                }

                if (!running)
                {
                    if (GetTimestamp() - finished > restartdelay)
                    {
                        if (initialised)
                        {
                            List<int> indexes = new List<int>(cells.Count);
                            for (int i = 0; i < cells.Count; i++)
                            {
                                indexes.Add(i);
                            }
                            int idx = -1;
                            do
                            {
                                idx = rand.Next(indexes.Count);
                                cells[indexes[idx]].Reset();
                                cells[indexes[idx]].Draw(graphics);
                                indexes.RemoveAt(idx);
                            } while (indexes.Count > 0);
                        }

                        current = cells[rand.Next(cells.Count)];
                        current.Current = true;
                        current.Visited = true;
                        current.Draw(graphics);
                        running = true;
                        initialised = true;
                    }
                }
                else
                {
                    int neighbourCount = 0;
                    Cell[] neighbours = new Cell[4];
                    Cell top = GetCellAt(current.X, current.Y - 1);
                    Cell right = GetCellAt(current.X + 1, current.Y);
                    Cell bottom = GetCellAt(current.X, current.Y + 1);
                    Cell left = GetCellAt(current.X - 1, current.Y);

                    if (top != null && !top.Visited)
                    {
                        neighbours[neighbourCount] = top;
                        neighbourCount++;
                    }
                    if (right != null && !right.Visited)
                    {
                        neighbours[neighbourCount] = right;
                        neighbourCount++;
                    }
                    if (bottom != null && !bottom.Visited)
                    {
                        neighbours[neighbourCount] = bottom;
                        neighbourCount++;
                    }
                    if (left != null && !left.Visited)
                    {
                        neighbours[neighbourCount] = left;
                        neighbourCount++;
                    }

                    if (neighbourCount == 0)
                    {
                        if (path.Count > 0)
                        {
                            current.Current = false;
                            current.Draw(graphics);
                            int idx = path.Count - 1;
                            current = path[idx];
                            path.RemoveAt(idx);
                            current.Current = true;
                            current.Draw(graphics);
                        }
                        else
                        {
                            running = false;
                            finished = GetTimestamp();
                        }
                    }
                    else
                    {
                        // Choose a square to move to.
                        Cell chosen = neighbours[rand.Next(neighbourCount)];
                        // Remove walls.
                        if (current.X - chosen.X == -1)
                        {
                            current.WallRight = false;
                            chosen.WallLeft = false;
                        }
                        else if (current.X - chosen.X == 1)
                        {
                            current.WallLeft = false;
                            chosen.WallRight = false;
                        }
                        else if (current.Y - chosen.Y == -1)
                        {
                            current.WallBottom = false;
                            chosen.WallTop = false;
                        }
                        else if (current.Y - chosen.Y == 1)
                        {
                            current.WallTop = false;
                            chosen.WallBottom = false;
                        }
                        // Set the flags.
                        current.Current = false;
                        // Draw it.
                        current.Draw(graphics);
                        // Add it to the path.
                        path.Add(current);
                        // Switch the current.
                        current = chosen;
                        current.Current = true;
                        current.Visited = true;
                        // Draw it.
                        current.Draw(graphics);
                    }
                }
                drawing = false;
            }
        }

        private int GetTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        private int GetIndex(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                return -1;
            }
            return (y * width) + x;
        }

        private Cell GetCellAt(int x, int y)
        {
            int index = GetIndex(x, y);
            if (index < 0 || index >= cells.Count)
            {
                return null;
            }
            return cells[index];
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                if (!mouseLocation.IsEmpty)
                {
                    // Terminate if mouse is moved a significant distance
                    if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                        Math.Abs(mouseLocation.Y - e.Y) > 5)
                        Application.Exit();
                }

                // Update current mouse location
                mouseLocation = e.Location;
            }
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }

        private void ScreenSaverForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!previewMode)
            {
                Application.Exit();
            }
        }
    }
}
