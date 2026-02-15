using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Views1.Models
{
    public class Subject
    {
        public int? subjectCode{get; set;}

        public String? subjectName{get; set; }

        [Range(0,100,ErrorMessage ="Maximum marks shall be between 0 and 100.")]
        public int? subjectMaxMarks{get; set;}

        [Range(0,100,ErrorMessage ="Obtained marks shall be between 0 and 100.")]
        public int? subjectMarksObtained{get; set;}
    }
}
