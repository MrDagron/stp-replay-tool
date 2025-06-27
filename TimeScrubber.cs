using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeAByte.BizHawk.StpTool;

public sealed class TimeScrubber : Control
{
    public TimeScrubber()
    {
        DoubleBuffered = true;
    }
    private readonly Color _watchedZoneColor = Color.Aqua;
    private readonly Color _unwatchedZoneColor = Color.Gray;
    public bool IsMouseDown = false;
    public (int X, int Y) ControlMousePosition = new(0,0);
    private int _totalCount = 0;
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);
        ControlMousePosition = (e.X, e.Y);
        IsMouseDown = true;
        Invalidate();
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        base.OnMouseUp(e);
        ControlMousePosition = (e.X, e.Y);
        IsMouseDown = false;
        Invalidate();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (IsMouseDown)
        {
            ControlMousePosition = (e.X, e.Y);
            Invalidate();
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (_totalCount == 0) return;
        using var watchedZoneBrush = new SolidBrush(_watchedZoneColor);
        using var unwatchedZoneBrush = new SolidBrush(_unwatchedZoneColor);
        e.Graphics.FillRectangle(unwatchedZoneBrush, ClientRectangle);
        var fillWidth = Math.Max(0, Math.Min(ControlMousePosition.X, Width));
        e.Graphics.FillRectangle(watchedZoneBrush, 0, 0, fillWidth, Height);
        using var borderPen = new Pen(Color.Black);
        e.Graphics.DrawRectangle(borderPen, 0, 0, this.Width - 1, this.Height - 1);
    }

    public int GetIndex(int total)
    {
        _totalCount = total;
        var scale = (double)Width / total;
        var index = (int)(ControlMousePosition.X / scale);
        return Math.Max(0, Math.Min(index, total - 1));
    }

    public void UpdatePosition(int index, int total)
    {
        _totalCount = total;
        var scale = (double)Width / total;
        var x = index * scale;
        ControlMousePosition.X = (int)x;
        Invalidate();
    }
}
