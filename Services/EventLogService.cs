using System;
using System.Collections.Generic;
using EventLogParser.DataModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading;
using System.Security;
using System.Diagnostics;
using System.ComponentModel;

namespace EventLogParser.Services;

public sealed class EventLogService : INotifyPropertyChanged
{

    EventLogService() 
    { 
        LogName = "Application";
        ComputerConnectionList = new ObservableCollection<ComputerConnection>();
        ComputerConnectionList.Add(new ComputerConnection("","", "Localhost"));

    }
    private static EventLogService instance = null;
    public ObservableCollection<ComputerConnection> ComputerConnectionList;
    private ConnectionID? _currentConnectionID;
    public ConnectionID? CurrentConnectionID
    {
        get => _currentConnectionID;
        set
        {
            _currentConnectionID = value;
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    public string LogName { get; set; }
    public static EventLogService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventLogService();
            }
            return instance;
        }
    }


    // Attempts to create the specified connection and read the first event. 
    // Returns a tuple with a boolean indicating whether the connection succeeded, and the exception that ocurred if it
    // failed.
    public Tuple<bool, string?> TestConnection(ComputerConnection connection)
    {
        string date = DateTime.Now
                    .AddDays(-1)
                    .ToUniversalTime()
                    .ToString("yyyy-MM-ddTHH:mm:ss.fffffff00K", CultureInfo.InvariantCulture);
        string query = string.Format($"*[System[TimeCreated[@SystemTime>\"{date}\"]]]");

        EventLogQuery eventLogQuery = new EventLogQuery("Application", PathType.LogName, query);
        eventLogQuery.Session = new EventLogSession(connection.Hostname, null, connection.Username, connection.Password, SessionAuthentication.Default);

        bool successful = true;
        string message = null;


        try
        {
            EventLogReader logReader = new EventLogReader(eventLogQuery);
            logReader.ReadEvent();

        }
        catch (EventLogNotFoundException e)
        {
            Console.WriteLine("An exception has occurred while reading the event logs. {0}", e.Message);
            successful = false;
            message = e.Message;
        }catch(UnauthorizedAccessException e)
        {
            Console.WriteLine("User is not authorized to access the {0} log.", LogName);
            successful = false;
            message = e.Message;
        }catch(Exception e)
        {
            Console.WriteLine("An exception has occurred while reading the event logs. {0}", e.Message);
            successful = false;
            message = e.Message;
        }
        return new Tuple<bool, string?>(successful, message);
    }

    public IEnumerable<EventLogItem> ReadEventLogs()
    {

        string date = DateTime.Now
                    .AddDays(-5)
                    .ToUniversalTime()
                    .ToString("yyyy-MM-ddTHH:mm:ss.fffffff00K", CultureInfo.InvariantCulture);

        string query = string.Format($"*[System[TimeCreated[@SystemTime>\"{date}\"]]]");
        // string logname = "Application";

        EventLogQuery eventLogQuery = new EventLogQuery(LogName, PathType.LogName, query);

        //check if the currently selected conenction is the local connection. If it is a remote connection, create a session.
        ConnectionID localConnectionID = ComputerConnectionList.First(comp => comp.Hostname.Equals("Localhost")).ID;
        if(CurrentConnectionID != null && (CurrentConnectionID != localConnectionID) )
        {
            ComputerConnection connection = (ComputerConnection)ComputerConnectionList.Single(c => c.ID == CurrentConnectionID);
            eventLogQuery.Session = new EventLogSession(connection.Hostname, null, connection.Username, connection.Password, SessionAuthentication.Default);
        }

        List<EventLogItem> eventLogItems = [];

        try
        {
            EventLogReader logReader = new EventLogReader(eventLogQuery);
            int i = 0;

            for (EventRecord record = logReader.ReadEvent(); record != null; record = logReader.ReadEvent())
            {
                var time = record.TimeCreated;
                var id = record.Id;
                var log = record.ProviderName;
                var level = (record.Level == null) ? 0 : (int)record.Level;
                var mname = record.MachineName;
                var description = record.FormatDescription();

                // Console.WriteLine($@"{time}, {id}, {log}, {level}, {mname}");
                EventLogItem item = new EventLogItem
                {
                    Type = level,
                    TimeCreated = time,
                    EventID = id,
                    Description = description,
                    Source = log,
                    Computer = mname

                };
                
                eventLogItems.Add(item);
            }

        }
        catch (EventLogNotFoundException e)
        {
            Console.WriteLine("An exception has occurred while reading the event logs. {0}", e.Message);
        }catch(UnauthorizedAccessException e)
        {
            Console.WriteLine("User is not authorized to access the {0} log.", LogName);
        }catch(Exception e)
        {
            Console.WriteLine("An exception has occurred while reading the event logs. {0}", e.Message);
            // TODO: show dialog with the error.
        }

        eventLogItems.Reverse();
        return eventLogItems;

    }
}
