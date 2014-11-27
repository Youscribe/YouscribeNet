using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YouScribe.Rest.Models
{
    public class FieldValidationError
    {
        public string FieldName { get; set; }

        public string MessageName { get; set; }

        public string Message { get; set; }
    }

    public class ValidationError
    {
        public ValidationError()
        {
            this.Fields = new List<FieldValidationError>();
        }

        public List<FieldValidationError> Fields { get; set; }
    }
}
