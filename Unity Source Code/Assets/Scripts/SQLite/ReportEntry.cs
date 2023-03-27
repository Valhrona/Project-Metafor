using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBank
{
    public class ReportEntry
    {

        public string _id;
        public String _type;
        public string _authorizationLevel;
        public string _name;
        public String _dateReported;

        public ReportEntry(string id, String type, string authorizationLevel, string name, string dateReported)
        {
            _id = id;
            _type = type;
            _authorizationLevel = authorizationLevel;
            _name = name;
            _dateReported = dateReported;
        }

        // Use current date if no reportdate (dateReported) has been provided as an argument
        public ReportEntry(string id, String type, string authorizationLevel, string name)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            _id = id;
            _type = type;
            _authorizationLevel = authorizationLevel;
            _name = name;
            _dateReported = dateTime.ToString("dd/MM/yyyy");
        }
    }
}
