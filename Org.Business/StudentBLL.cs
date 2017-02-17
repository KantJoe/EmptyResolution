using Org.Business.Base;
using Org.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Org.Business
{
    public class StudentBLL : BusinessBase<StudentBLL>
    {
        private StudentBLL() { }

        public Tuple<Student,List<Subject>,List<SubjectGrade>> GetsubGrade()
        {
            var _index = 0;
            var subList = new List<Subject>();
            var subGList = new List<SubjectGrade>();
            var stu = new Student() { ID="001",FirstName="chow",SecondName="xing",Class=1,Grade=1};
            foreach (var i in new string[]{"语文","数学","英语"})
            {
                subList.Add(new Subject() { ID = _index.ToString(), Name = i });
            }
            foreach (var i in new int[]{89,96,86})
            {
                subGList.Add(new SubjectGrade() { FK_SG_StudentID=stu.ID,FK_SG_SubjectID =subList[_index++].ID,Grade=i,ID=_index.ToString() });
            }
            return Tuple.Create<Student, List<Subject>, List<SubjectGrade>>(stu, subList, subGList);
        }
    }
}
