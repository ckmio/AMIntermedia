using System;

namespace AMIntermediaCore
{
    public class AxeFileEvent
    {
        public string Type {get; set; }
        public string FileName {get; set;}

        public DateTime CreationDate {get; set; }

        public String FromEntity {get; set; }

        public static AxeFileEvent FromFile(string filePath)
        {
            return null;
        }
    }
}