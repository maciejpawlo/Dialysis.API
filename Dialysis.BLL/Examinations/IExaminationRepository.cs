using Dialysis.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Examinations
{
    public interface IExaminationRepository
    {
        Task<IEnumerable<Examination>> GetAllExaminations();
        Task<Examination> GetExaminationById(int id);
        Task AddExamination(Examination examination);
        Task DeleteExamination(int id);
        Task UpdateExamination(int id);
    }
}
