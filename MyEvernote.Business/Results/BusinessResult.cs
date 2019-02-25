using MyEvernote.Entities.Messages;
using System.Collections.Generic;

namespace MyEvernote.Business.Results
{
    public class BusinessResult<T> where T : class
    {
        public BusinessResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public void AddError(ErrorMessageCode code, string message)
        {

            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
        }

        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }
    }
}