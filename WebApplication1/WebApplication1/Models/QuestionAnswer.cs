using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class QuestionAnswer
    {
        public Question question { get; set; }
        public List<TraLoi> _answers;
        public QuestionAnswer()
        {
            question = new Question();
            _answers = new List<TraLoi>();
        }
        public List<TraLoi> Answers { get => _answers; }

    }
}