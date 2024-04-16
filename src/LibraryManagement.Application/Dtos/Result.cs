using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Dtos
{
    public class Result
    {
        public Result(bool isSucceed, string message)
        {
            IsSucceed = isSucceed;
            Message = message;
        }

        public Result(string message)
        {
            IsSucceed = true;
            Message = message;
        }

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
    }
}
