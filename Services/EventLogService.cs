using System;
using System.Collections.Generic;
using EventLogParser.DataModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Collections.ObjectModel;

namespace EventLogParser.Services;

public sealed class EventLogService
{

    EventLogService() 
    { 
        LogName = "Application";
        ComputerConnectionList = new List<ComputerConnection>();

    }
    private static EventLogService instance = null;
    public List<ComputerConnection> ComputerConnectionList;
    public ConnectionID? CurrentConnectionID;
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

    static void Connection_Handler(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {

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

        if(CurrentConnectionID != null)
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
                var log = record.LogName;
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
        }

        eventLogItems.Reverse();
        return eventLogItems;

    }
}
