using Godot;

public partial class GodotLifeBar : Control
{
    [Export]
    private Label _label;

    [Export]
    private ProgressBar _bar;

    public void Init(int Maximum)
    {
        _bar.MaxValue = Maximum;
        SetValue(Maximum);
    }

    public void SetValue(int Value)
    {
        if (Value < 0)
        {
            SetValue(0);
        }
        else if (Value > _bar.MaxValue)
        {
            SetValue((int)_bar.MaxValue);
        }
        _bar.Value = Value;
        _label.Text = Value.ToString();
    }
}
