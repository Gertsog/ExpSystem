namespace ExpSystem
{
    public class Question
    {
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public double Answer { get; set; }

        public Question(string questionText)
        {
            QuestionText = questionText;
        }
    }
}
