using Microsoft.AspNetCore.Http;
using Shared.Application;
using Shared.Application.BaseCommands;
using Shared.Application.Validations;
using Shared.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Site.Application.Contract.BanerService.Command
{
    public class EditBanerCommandModel:Image_ImageAlt
    {
        public int Id { get; set; }
        [Display(Name = "لینک مقصد")]
        [Required(ErrorMessage = ValidationMessages.RequiredMessage)]
        [MaxLength(900, ErrorMessage = ValidationMessages.MaxLengthMessage)]
        public string Url { get; set; }
        public BanerState State{ get; set; }
        public string ImageName { get; set; }


    }
}
