using Dialysis.DAL;
using Dialysis.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Dialysis.DAL.DTOs;

namespace Dialysis.BLL.Examinations
{
    public class ExaminationRepository : IExaminationRepository
    {
        private readonly DialysisContext context;
        private readonly IMapper mapper;

        public ExaminationRepository(DialysisContext context, 
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> AddExamination(ExaminationDTO examination)
        {
            var patient = await context.Patients.FindAsync(examination.PatientID);

            if (patient == null)
                return false;

            var newExamination = mapper.Map<Examination>(examination);
            newExamination.CreatedAt = DateTime.Now;
            await context.Examinations.AddAsync(newExamination);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteExamination(int id)
        {
            var examination = await context.Examinations
                .FindAsync(id);

            if (examination == null)
                return false;

            context.Examinations
                .Remove(examination);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ExaminationDTO>> GetAllExaminations()
        {
            var examinations = await context.Examinations
                .AsNoTracking()
                .ToListAsync();

            var result = mapper.Map<IEnumerable<ExaminationDTO>>(examinations);

            return result;
        }

        public async Task<ExaminationDTO> GetExaminationById(int id)
        {
            var examination = await context.Examinations
                .AsNoTracking()
                .Where(e => e.ExaminationID == id)
                .FirstOrDefaultAsync();

            var result = mapper.Map<ExaminationDTO>(examination);

            return result;
        }

        public async Task<bool> EditExamination(int id, ExaminationDTO examination)
        {
            var oldExamination = await context.Examinations
                .Where(e => e.ExaminationID == id)
                .FirstOrDefaultAsync();

            if (oldExamination == null)
                return false;

            mapper.Map(examination, oldExamination);

            return await context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ExaminationDTO>> GetExaminationsByPatientId(int id)
        {
            var examinations = await context.Examinations
                .AsNoTracking()
                .Where(e => e.PatientID == id)
                .ToListAsync();

            var result = mapper.Map<IEnumerable<ExaminationDTO>>(examinations);

            return result;
        }
    }
}
