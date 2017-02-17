using Org.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.ViewModel
{
    public class StuSubGrade
    {
        public Student stu { get; set; }
        public List<Subject> sub { get; set; }
        public List<SubjectGrade> subGrade { get; set; }
    }
}
