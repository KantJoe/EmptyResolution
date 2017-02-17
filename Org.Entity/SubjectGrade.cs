using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Entity
{
    public class SubjectGrade
    {
        public string ID { get; set; }

        public string FK_SG_SubjectID { get; set; }

        public decimal Grade { get; set; }

        public string FK_SG_StudentID{get;set;}
    }
}
