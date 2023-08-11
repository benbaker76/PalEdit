using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PalEdit
{
    public class RectangleNode
    {
        public bool Used = false;
        public RectangleNode Left = null;
        public RectangleNode Right = null;
        public Rectangle Rect = Rectangle.Empty;

        public RectangleNode(Rectangle rect)
        {
            Rect = rect;
        }
    }

    public class RectanglePacker
    {
        private Size m_usedSize;
        private RectangleNode m_root = null;

        public RectanglePacker(int width, int height)
        {
            m_root = new RectangleNode(new Rectangle(0, 0, width, height));
            m_usedSize = Size.Empty;
        }

        private bool RecursiveFindPoint(RectangleNode node, Size size, ref Point point)
        {
            if (node.Left != null)
            {
                RecursiveFindPoint(node.Left, size, ref point);

                return point != Point.Empty ? true : RecursiveFindPoint(node.Right, size, ref point);
            }
            else
            {
                if (node.Used || size.Width > node.Rect.Width || size.Height > node.Rect.Height)
                    return false;

                if (size.Width == node.Rect.Width && size.Height == node.Rect.Height)
                {
                    node.Used = true;
                    point.X = node.Rect.X;
                    point.Y = node.Rect.Y;

                    return true;
                }

                node.Left = new RectangleNode(node.Rect);
                node.Right = new RectangleNode(node.Rect);

                if (node.Rect.Width - size.Width > node.Rect.Height - size.Height)
                {
                    node.Left.Rect.Width = size.Width;
                    node.Right.Rect.X = node.Rect.X + size.Width;
                    node.Right.Rect.Width = node.Rect.Width - size.Width;
                }
                else
                {
                    node.Left.Rect.Height = size.Height;
                    node.Right.Rect.Y = node.Rect.Y + size.Height;
                    node.Right.Rect.Height = node.Rect.Height - size.Height;
                }

                return RecursiveFindPoint(node.Left, size, ref point);
            }
        }
        public bool FindPoint(Size size, ref Point point)
        {
            if (RecursiveFindPoint(m_root, size, ref point))
            {
                if (m_usedSize.Width < point.X + size.Width)
                    m_usedSize.Width = point.X + size.Width;
                if (m_usedSize.Height < point.Y + size.Height)
                    m_usedSize.Height = point.Y + size.Height;

                return true;
            }

            return false;
        }

        public Size UsedSize
        {
            get { return m_usedSize; }
        }
    }
}