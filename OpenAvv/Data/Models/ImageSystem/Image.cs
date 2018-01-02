using System;

namespace OpenAvv.Data.Models.ImageSystem
{
    public class Image
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string AltText { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime DateCreated { get; set; }
    }
}