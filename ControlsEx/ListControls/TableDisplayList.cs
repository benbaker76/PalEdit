using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ControlsEx.ListControls
{
	/// <summary>
	/// abstract base class for VTableDisplayList and HTableList
	/// </summary>
	public abstract class TableDisplayList : DisplayList
	{
		#region variables
		protected int m_lines = 1;
		protected Size m_fieldSize = new Size(48, 48);
		//
		protected Rectangle m_cacheBounds;
		protected int m_cacheIndex = int.MinValue,
			m_cacheCol;
		#endregion
		#region structs
		/// <summary>
		/// struct for holding a line and a column index
		/// </summary>
		protected struct Cell
		{
			public Cell(int row, int column)
			{
				this.Row = row;
				this.Column = column;
			}
			public int Row;
			public int Column;
		}
		#endregion
		#region inheritable
		/// <summary>
		/// override this member to implement calculating of
		/// line and column index from a specified position
		/// </summary>
		protected abstract Cell GetCellAtPosition(int x, int y);
		/// <summary>
		/// override this member to implement calculating of
		/// line and column index from a specified index
		/// </summary>
		protected abstract Cell GetCellAtIndex(int index);
		#endregion
		#region properties
		/// <summary>
		/// specifies the size of an element
		/// </summary>
		[Description("specifies the size of an element"),
		DefaultValue(typeof(Size), "48;48")]
		public Size FieldSize
		{
			get { return m_fieldSize; }
			set
			{
				//limit to maximal/minimal bounds
				value = new Size(
					Math.Max(10, Math.Min(1000, value.Width)),
					Math.Max(8, Math.Min(1000, value.Height)));
				if (value == m_fieldSize) return;
				m_fieldSize = value;
				//refresh the control
				Reload();
			}
		}
		#endregion
	}
	/// <summary>
	/// VTableList aligns DisplayElements as vertical table
	/// </summary>
	[ToolboxItem(true)]
	public class VTableDisplayList : TableDisplayList
	{
		#region helpers
		/// <summary>
		/// adjusts the scrollbars to content
		/// </summary>
		protected override Size GetTotalSize(Size clientsize, int count)
		{
			m_cacheIndex = int.MinValue;
			//calculate maximum number of lines which fit the control
			m_lines = Math.Max(1, (clientsize.Width - 2) / (base.m_fieldSize.Width + 1));
			//calculate row count
			int rem, rows = Math.DivRem(count, m_lines, out rem);
			if (rem != 0) rows++;
			//if hscroll is visible, recalculate
			if (rows * (base.m_fieldSize.Height + 1) > clientsize.Height)
			{
				m_lines = Math.Max(1, (clientsize.Width - 2 - SystemInformation.VerticalScrollBarWidth) /
					(m_fieldSize.Width + 1));
				//calculate row count
				rows = Math.DivRem(count, m_lines, out rem);
				if (rem != 0) rows++;
			}
			//update scrollbars
			return new Size(0,
				1 + rows * (base.m_fieldSize.Height + 1));
		}
		/// <summary>
		/// gets index of the item at the specified position.
		/// collection boundaries are checked
		/// </summary>
		protected override int GetIndexAt(int x, int y)
		{
			Cell c = this.GetCellAtPosition(x, y);
			return c.Row * m_lines + c.Column;
		}
		/// <summary>
		/// gets the bounds of the item at the specified index.
		/// collection boundaries are not checked
		/// </summary>
		protected override Rectangle GetBoundsAt(int index)
		{
			//optimization for drawing
			if (index == m_cacheIndex + 1)
			{
				m_cacheIndex = index;
				m_cacheBounds.X += base.m_fieldSize.Width + 1;
				if (m_cacheCol >= m_lines)
				{
					m_cacheCol = 0;
					m_cacheBounds.X = 1;
					m_cacheBounds.Y += base.m_fieldSize.Height + 1;
				}
			}
			m_cacheIndex = index;
			Cell c = this.GetCellAtIndex(index);
			m_cacheCol = c.Column;
			return m_cacheBounds = GetBoundsAtCell(c);
		}

		protected override void GetDrawingInterval(Rectangle clip, out int start, out int stop)
		{
			Cell startcell = GetCellAtPosition(clip.X, clip.Y);
			stop = GetIndexAt(clip.Right, clip.Bottom);
			start = startcell.Row * m_lines + startcell.Column;
		}
		#region cell functions
		/// <summary>
		/// gets the bounds of the item at the specified
		/// line and column index.
		/// collection boundaries are not checked
		/// </summary>
		private Rectangle GetBoundsAtCell(Cell c)
		{
			return new Rectangle(
				 1 + c.Column * (base.m_fieldSize.Width + 1),
				 1 + c.Row * (base.m_fieldSize.Height + 1),
				base.m_fieldSize.Width,
				base.m_fieldSize.Height);
		}
		/// <summary>
		/// calculates a line and a column index from the
		/// specified position.
		/// collection boundaries are not checked
		/// </summary>
		protected override Cell GetCellAtPosition(int x, int y)
		{
			return new Cell(
				Math.Max(0, y / (base.m_fieldSize.Height + 1)),//row
				Math.Max(0, Math.Min(m_lines - 1, x / (base.m_fieldSize.Width + 1))));//column
		}
		/// <summary>
		/// calculates a line and a column index from the
		/// specified index.
		/// collection boundaries are not checked
		/// </summary>
		protected override Cell GetCellAtIndex(int index)
		{
			int col, row = Math.DivRem(index, m_lines, out col);
			return new Cell(
				Math.Max(0, row),
				Math.Max(0, Math.Min(m_lines - 1, col)));
		}
		#endregion
		#endregion
	}
}
