using System;
using System.ComponentModel;

namespace EventLogParser.DataModel;

public class EventLogItem
{
    public int Type { get; set; } = 0;
    public DateTime? TimeCreated { get; set; }
    public int EventID { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public string Computer { get; set; }
    public string TextColor
    {
        get => GetTextColor();
    }

    public string GetTextColor()
    {
        switch (Type)
        {
            case 2:
                return "PaleVioletRed";
            case 3:
                return "Yellow";
            default:
                return "White";
        }
    }

    public string GetTime
    {
        get => (TimeCreated == null) ? DateTime.MinValue.ToString() : TimeCreated.ToString();
    }
}
