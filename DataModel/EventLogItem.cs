using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
// using Microsoft.CodeAnalysis.Diagnostics;

namespace EventLogParser.DataModel;

public class EventLogItem : INotifyPropertyChanged
{
    private const int TruncatedLength = 100;
    private int _type;
    public int Type { get => _type; set => _type = value; }

    private DateTime? _timeCreated;
    public DateTime? TimeCreated { get => _timeCreated; set => _timeCreated = value; }

    private int _eventID;
    public int EventID { get => _eventID; set => _eventID = value; }

    private string _description;
    public string Description 
    { 
        get 
        {
            if(Expanded)
                return _description;
            else
                return DescriptionTruncated;
        }
        set 
        {
            _description = value;
            // NotifyPropertyChanged();
        }  
            
    }
    // public string Description { get => Expanded ? _description : DescriptionTruncated; set => _description = value; }

    // Returns a truncated version of the event description.
    public string DescriptionTruncated
    {
        get
        {
            if (_description == null) return null;

            // add elipses to the end if the text was truncated.
            if (_description.Length - 1 > TruncatedLength)
                return _description.Substring(0, Math.Min(_description.Length - 1, TruncatedLength)) + "...";
            else
                return _description;
        }
    }

    private string _source;
    public string Source { get => _source; set => _source = value; }
    private string _computer;
    public string Computer { get => _computer; set => _computer = value; }
    public string TextColor
    {
        get => GetTextColor();
    }

    public string GetTextColor()
    {
        switch (_type)
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

    private bool _expanded;

    public bool Expanded
    {
        get => _expanded;
        set 
        {
            _expanded = value;
            NotifyPropertyChanged(nameof(Description));
        } 
    }


    public EventLogItem()
    {
        _expanded = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // This method is called by the Set accessor of each property.  
    // The CallerMemberName attribute that is applied to the optional propertyName  
    // parameter causes the property name of the caller to be substituted as an argument.  
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }



}
