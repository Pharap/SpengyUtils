// Simple wrapper around IMyTextPanel to give it functionality
// similar to .Net's System.Console and System.IO.TextWriter.

public class Terminal  
{
    private readonly IMyTextPanel panel;
    
    public Terminal(IMyTextPanel panel)
    {
        if(panel == null)
        {
            throw new ArgumentNullException("panel");
        }
        this.panel = panel;
    }

    public float FontSize
    {
        get { return panel.GetValueFloat("FontSize"); }
        set { panel.SetValueFloat("FontSize", value); }
    }

    public Color FontColour
    {
        get { return panel.GetValueColor("FontColor"); }
        set { panel.SetValueColor("FontColor", value); }
    }

    public Color BackgroundColour
    {
        get { return panel.GetValueColor("BackgroundColour"); }
        set { panel.SetValueColor("BackgroundColour", value); }
    }

    public void Show()
    {
        panel.ShowPublicTextOnScreen();
    }

    public void Clear()
    {
        panel.WritePublicText("");
    }

    public void Write(string text)
    {
        panel.WritePublicText(text, true);
    }

    public void Write(object value)
    {
        this.Write(value.ToString());
    }

    public void Write(string format, object value)
    {
        this.Write(string.Format(format, value));
    }

    public void Write(string format, params object[] args)
    {
        this.Write(string.Format(format, args));
    }
    
    public void WriteLine()  
    {  
        panel.WritePublicText("\n", true);  
    }

    public void WriteLine(string text)
    {
        panel.WritePublicText(text + "\n", true);
    }

    public void WriteLine(object value)
    {
        this.WriteLine(value.ToString());
    }

    public void WriteLine(string format, object value)
    {
        this.WriteLine(string.Format(format, value));
    }

    public void WriteLine(string format, params object[] args)
    {
        this.WriteLine(string.Format(format, args));
    }
}
