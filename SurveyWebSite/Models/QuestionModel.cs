using Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SurveyWebSite.Models
{
    public class QuestionModel
    {
        [DataType(DataType.MultilineText)]
        [StringLength(100, ErrorMessageResourceName = "TheQuestionIsToLong", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Required(ErrorMessageResourceName = "QuestionIsEmptyMessage", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public string NewText { get; set; }
        [Range(1, 10000, ErrorMessageResourceName = "TheOrderNumberLong", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Required(ErrorMessageResourceName = "TheOrderIsEmpty", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public int Order { get; set; }
        public int Id { get; set; }
        public TypeOfQuestion TypeOfQuestion { get; set; }
        public int IdForType { get; set; }
        [Required(ErrorMessageResourceName = "EmptyStartValue", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Range(1, 100, ErrorMessageResourceName = "StartValueGreaterThanOneHundredMessage", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public int StartValue { get; set; }
        [Required(ErrorMessageResourceName = "EmptyEndValue", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Range(1, 100, ErrorMessageResourceName = "EndValueGreaterThanOneHundredMessage", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public int EndValue { get; set; }
        [Required(ErrorMessageResourceName = "EmptyStartCaption", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [StringLength(10, MinimumLength = 1, ErrorMessageResourceName = "StartCaptionIsToLong", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public string StartCaption { get; set; }
        [Required(ErrorMessageResourceName = "EmptyEndCaption", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [StringLength(10, MinimumLength = 1, ErrorMessageResourceName = "EndCaptionIsToLong", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public string EndCaption { get; set; }
        [Required(ErrorMessageResourceName = "EmptyNumberOfSmile", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Range(2, 5, ErrorMessageResourceName = "NumberOfSmileBetweenFiveAndTow", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public int NumberOfSmiles { get; set; }
        [Required(ErrorMessageResourceName = "EmptyNumberOfStar", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        [Range(1, 10, ErrorMessageResourceName = "NumberOfStrasBetweenTenAndOne", ErrorMessageResourceType = typeof(Question.Resources.Messages))]
        public int NumberOfStars { get; set; }
    }
}