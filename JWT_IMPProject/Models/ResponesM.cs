using NPOI.SS.Formula.Functions;
using System.Linq;

namespace JWT_IMPProject.Models
{
    public class ResponesM<T>
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
        public int StatusCode { get; set; }
        public T Data {get;set;}
    }
}
