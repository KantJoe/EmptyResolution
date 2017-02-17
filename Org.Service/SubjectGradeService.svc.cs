using Org.Business;
using Org.Common.BasicClass;
using Org.Entity;
using Org.Service.BasicSetting;
using Org.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Org.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“SubjectGrade”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 SubjectGrade.svc 或 SubjectGrade.svc.cs，然后开始调试。
    [ServiceErrorHandling]
    public class SubjectGradeService : ISubjectGradeService
    {
        StudentBLL studentBll = StudentBLL.GetInstance();
        /// <summary>
        /// 获取学生成绩
        /// </summary>
        /// <returns></returns>
        public TransResponse<StuSubGrade> GetSudentGrades()
        {
            var response = new TransResponse<StuSubGrade>();
            var sg= studentBll.GetsubGrade();
            response.Result = new StuSubGrade() { stu = sg.Item1, sub = sg.Item2, subGrade = sg.Item3 };
            return response;
        }

    }
}
