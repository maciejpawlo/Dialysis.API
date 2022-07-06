﻿using Dialysis.BE.Helpers;
using Dialysis.BLL.Examinations;
using Dialysis.BLL.Users;
using Dialysis.DAL.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Dialysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExaminationsController : ControllerBase
    {
        private readonly IExaminationRepository repository;

        public ExaminationsController(IExaminationRepository repository)
        {
            this.repository = repository;
        }

        [Authorize(Roles = Role.Doctor)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExaminations()
        {
            var result = await repository.GetAllExaminations();
            return Ok(result);
        }

        [Authorize(Roles = Role.Patient)]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExaminations([FromBody] ExaminationDTO examinationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await repository.AddExamination(examinationDTO);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExaminations(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await repository.DeleteExamination(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditExaminations(int id, [FromBody] ExaminationDTO examinationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await repository.EditExamination(id, examinationDTO);
            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExaminationByID(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await repository.GetExaminationById(id);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
