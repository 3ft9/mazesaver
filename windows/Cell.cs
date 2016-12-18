using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeSaver
{
    class Cell
    {
        private int m_scale;
        private int m_x;
        private int m_y;
        private int m_xOffset;
        private int m_yOffset;

        private Pen m_gridPen;
        private Pen m_wallPen;
        private Pen m_invisiblePen;
        private Brush m_currentBrush;
        private Brush m_visitedBrush;

        private bool m_wall_top;
        private bool m_wall_right;
        private bool m_wall_bottom;
        private bool m_wall_left;

        private bool m_current;
        private bool m_path;
        private bool m_visited;

        public Cell(int x, int y, int scale, Pen gridPen, Pen wallPen, Pen invisiblePen, Brush currentBrush, Brush visitedBrush, int xOffset, int yOffset)
        {
            m_x = x;
            m_y = y;
            m_scale = scale;
            m_gridPen = gridPen;
            m_wallPen = wallPen;
            m_invisiblePen = invisiblePen;
            m_currentBrush = currentBrush;
            m_visitedBrush = visitedBrush;
            m_xOffset = xOffset;
            m_yOffset = yOffset;
            Reset();
        }

        public int X { get { return m_x; } }
        public int Y { get { return m_y; } }

        public bool WallTop
        {
            get { return m_wall_top; }
            set { m_wall_top = value; }
        }

        public bool WallRight
        {
            get { return m_wall_right; }
            set { m_wall_right = value; }
        }

        public bool WallBottom
        {
            get { return m_wall_bottom; }
            set { m_wall_bottom = value; }
        }

        public bool WallLeft
        {
            get { return m_wall_left; }
            set { m_wall_left = value; }
        }

        public bool Current
        {
            get { return m_current; }
            set { m_current = value; }
        }

        public bool Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        public bool Visited
        {
            get { return m_visited; }
            set { m_visited = value; }
        }

        public void Reset()
        {
            m_wall_top = true;
            m_wall_right = true;
            m_wall_bottom = true;
            m_wall_left = true;
            m_current = false;
            m_visited = false;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(m_visited ? (m_current ? m_currentBrush : m_visitedBrush) : Brushes.Black, new Rectangle(m_xOffset + (m_x * m_scale) + 1, m_yOffset + (m_y * m_scale) + 1, m_scale - 1, m_scale - 1));

            DrawLine(g,
                m_xOffset + (m_x * m_scale),
                m_yOffset + (m_y * m_scale),
                m_xOffset + (m_x * m_scale) + m_scale,
                m_yOffset + (m_y * m_scale),
                m_wall_top);

            DrawLine(g,
                m_xOffset + (m_x * m_scale) + m_scale,
                m_yOffset + (m_y * m_scale),
                m_xOffset + (m_x * m_scale) + m_scale,
                m_yOffset + (m_y * m_scale) + m_scale,
                m_wall_right);

            DrawLine(g,
                m_xOffset + (m_x * m_scale),
                m_yOffset + (m_y * m_scale) + m_scale,
                m_xOffset + (m_x * m_scale) + m_scale,
                m_yOffset + (m_y * m_scale) + m_scale,
                m_wall_bottom);

            DrawLine(g,
                m_xOffset + (m_x * m_scale),
                m_yOffset + (m_y * m_scale),
                m_xOffset + (m_x * m_scale),
                m_yOffset + (m_y * m_scale) + m_scale,
                m_wall_left);
        }

        public void DrawLine(Graphics g, int x1, int y1, int x2, int y2, bool visible)
        {
            g.DrawLine((visible ? (m_visited ? m_wallPen : m_gridPen) : m_invisiblePen), x1, y1, x2, y2);
        }
    }
}
