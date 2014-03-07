using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcTest.Models
{
    public class EditModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "请上传图片！")]
        public string Image { get; set; }
    }
}