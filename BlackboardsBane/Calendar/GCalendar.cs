using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackboardsBane
{
    public class GCalendar
    {
        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "Google Calendar API .NET Quickstart";
        static string timeZone = "America/Chicago";

        private CalendarService serv;

        public async Task<bool> Init()
        {
            UserCredential credential;

            if (!File.Exists("credentials.json"))
            {
                return false;
            }

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None,
                    new FileDataStore(credPath, true));
            }

            serv = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return true;
        }

        public string AddOrFindCalendar(string name, string desc)
        {
            var listreq = serv.CalendarList.List();
            CalendarList list = listreq.Execute();
            int indexOfCal = list.Items.ToList().FindIndex(i => i.Summary == name);
            
            if (indexOfCal != -1) //calendar already added
                return list.Items[indexOfCal].Id;

            Calendar cal = new Calendar
            {
                Summary = name,
                Description = desc,
                TimeZone = timeZone //yes hardcoded timezone deal with it
            };

            var calreq = serv.Calendars.Insert(cal);
            Calendar newCal = calreq.Execute();
            return newCal.Id;
        }

        public void AddItemToCalendar(string title, string desc, DateTime startdt, DateTime enddt, string calId = "primary")
        {
            Event ev = new Event
            {
                Summary = title,
                Description = desc
            };

            EventDateTime start = new EventDateTime
            {
                DateTime = startdt,
                TimeZone = timeZone
            };
            ev.Start = start;

            EventDateTime end = new EventDateTime
            {
                DateTime = enddt,
                TimeZone = timeZone
            };
            ev.End = end;

            var insreq = serv.Events.Insert(ev, calId);
            insreq.Execute();
        }
    }
}
