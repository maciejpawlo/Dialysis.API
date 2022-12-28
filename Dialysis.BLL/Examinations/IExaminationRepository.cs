using Dialysis.DAL.DTOs;
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
        Task<IEnumerable<ExaminationDTO>> GetAllExaminations();
        Task<ExaminationDTO> GetExaminationById(int id);
        Task<bool> AddExamination(ExaminationDTO examination);
        Task<bool> DeleteExamination(int id);
        Task<bool> EditExamination(int id, ExaminationDTO examination);
        Task<IEnumerable<ExaminationDTO>> GetExaminationsByPatientId(int id);
    }
}
