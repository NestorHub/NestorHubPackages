using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace NestorHub.Netatmo.Security.Class
{
    public class Home
    {
        public string Id { get; }
        public string Name { get; }
        public IEnumerable<Camera> Cameras { get; }
        public IEnumerable<Event> Events { get; }

        public Home(string id, string name, IEnumerable<Camera> cameras, IEnumerable<Event> events)
        {
            Id = id;
            Name = name;
            Cameras = cameras;
            Events = events;
        }
    }

    public class Event
    {
        public string Id { get; }
        public string Type { get; }
        public TimeSpan Time { get; }
        public string CameraId { get; }
        public string DeviceId { get; }
        public string PersonId { get; }
        public string VideoId { get; }
        public string VideoStatus { get; }
        public bool IsArrival { get; }
        public string Message { get; }

        [JsonConstructor]
        public Event(string id, string type, Int64 time, string camera_id, string device_id, string person_id, string video_id, string video_status, bool is_arrival, string message)
        {
            Id = id;
            Type = type;
            Time = time > 0 ? new TimeSpan(time) : new TimeSpan();
            CameraId = camera_id;
            DeviceId = device_id;
            PersonId = person_id;
            VideoId = video_id;
            VideoStatus = video_status;
            IsArrival = is_arrival;
            Message = message;
        }
    }
}
