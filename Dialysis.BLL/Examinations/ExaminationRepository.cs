using Dialysis.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialysis.BLL.Examinations
{
    public class ExaminationRepository : IExaminationRepository
    {
        public Task AddExamination(Examination examination)
        {
            throw new NotImplementedException();
        }

        public Task DeleteExamination(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Examination>> GetAllExaminations()
        {
            throw new NotImplementedException();
        }

        public Task<Examination> GetExaminationById(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExamination(int id)
        {
            throw new NotImplementedException();
        }
    }
}
