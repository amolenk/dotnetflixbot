using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1
{
    [Serializable]
    public class Episode
    {
        public string Title { get; set; }
        public Uri Link { get; set; }
    }
}