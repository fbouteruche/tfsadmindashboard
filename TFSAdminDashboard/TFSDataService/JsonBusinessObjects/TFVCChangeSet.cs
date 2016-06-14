using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TFVCChangeSetRootobject
    {
        public int count { get; set; }
        public TFVCChangeSet[] value { get; set; }
    }

    public class TFVCChangeSet
    {
        public int changesetId { get; set; }
        public string url { get; set; }
        public Author author { get; set; }
        public Checkedinby checkedInBy { get; set; }
        public DateTime createdDate { get; set; }
        public string comment { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Checkedinby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

}
