using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DataModel.DTO.Communication
{
    public class Paging
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value equal or bigger than {0}")]
        public int Index { get; set; }

        [Range(10, int.MaxValue, ErrorMessage = "Please enter a value equal or bigger than {0}")]
        public int PageSize { get; set; }

        public Tuple<bool, string> ValidatePaging()
        {
            bool isValid = false;
            string error = string.Empty;

            if (this.Index < 0)
            {
                isValid = false;
                error = "Please enter an index equal or bigger than 0";
            }
            else if (this.PageSize < 10)
            {
                isValid = false;
                error = "Please enter a PageSize equal or bigger than 10";
            }
            else
            {
                isValid = true;
                error = string.Empty;
            }

            return new Tuple<bool, string>(isValid, error);
        }
    }
}