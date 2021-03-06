﻿using System;
using System.Collections.Generic;

namespace Gwen.Control.Layout
{
	/// <summary>
	/// Arrange child controls into a row or a column.
	/// </summary>
	[Xml.XmlControl]
	public class StackLayout : ControlBase
	{
		private bool m_Horizontal;

		/// <summary>
		/// If set, arrange child controls into a row instead of a column.
		/// </summary>
		[Xml.XmlProperty]
		public bool Horizontal { get { return m_Horizontal; } set { if (m_Horizontal == value) return; m_Horizontal = value; Invalidate(); } }

		public StackLayout(ControlBase parent)
			: base(parent)
		{
		}

		protected override Size OnMeasure(Size availableSize)
		{

			availableSize -= Padding;

			int width = 0;
			int height = 0;

			if (m_Horizontal)
			{
				foreach (ControlBase child in this.Children)
				{
					if (child.IsCollapsed)
						continue;

					Size size = child.Measure(availableSize);
					availableSize.Width -= size.Width;
					if (size.Height > height)
						height = size.Height;
					width += size.Width;
				}
			}
			else
			{
				foreach (ControlBase child in this.Children)
				{
					if (child.IsCollapsed)
						continue;

					Size size = child.Measure(availableSize);
					availableSize.Height -= size.Height;
					if (size.Width > width)
						width = size.Width;
					height += size.Height;
				}
			}

			return new Size(width, height) + Padding;
		}

		protected override Size OnArrange(Size finalSize)
		{
			finalSize -= Padding;

			if (m_Horizontal)
			{

				int height = finalSize.Height;
				int x = Padding.Left;
				int stretchWidth = -1;
				
				void arrange()
				{
					int priorityCount = 0;
					
					foreach (ControlBase child in this.Children)
					{
						if (child.IsCollapsed)
							continue;

						var width = child.MeasuredSize.Width;
						if (child.PriorityControl && stretchWidth >= 0)
						{
							priorityCount++;
							if(priorityCount > 1) throw new Exception("Only one priority control can exist per layout.");
								width = stretchWidth;
						}							

						child.Arrange(new Rectangle(x, Padding.Top, width, height));
						x += child.MeasuredSize.Width;
					}

					x += Padding.Right;
				}
				arrange();

				if (x - Padding.Right < finalSize.Width)
				{
					stretchWidth = finalSize.Width - x;
					x = Padding.Left;				
					arrange();
				}

				return new Size(x, finalSize.Height + Padding.Top + Padding.Bottom);
			}
			else
			{
				int width = finalSize.Width;
				int y = Padding.Top;

				foreach (ControlBase child in this.Children)
				{
					if (child.IsCollapsed)
						continue;

					child.Arrange(new Rectangle(Padding.Left, y, width, child.MeasuredSize.Height));
					y += child.MeasuredSize.Height;
				}

				y += Padding.Bottom;

				return new Size(finalSize.Width + Padding.Left + Padding.Right, y);
			}
		}
	}
}
