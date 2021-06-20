using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpSystem.Entities
{
    public static class Phrases
    {
        public static string OpenFile => "Откройте файл или перетащите его на форму";
        public static string NoFileSelected => "Файл не выбран";
        public static string PressStart => "Для начала консультации нажмите кнопку \"Старт\"";
        public static string StartConsultation => "Начните консультацию";
        public static string ConsultationEnded => "Консультация окончена";
        public static string FileReadingError => "Ошибка при попытке считывания файла";
        public static string HeaderReadingError => "Ошибка при считывании заголовка";
        public static string QuestionsReadingError => "Ошибка при считывании вопросов";
        public static string HypothesesReadingError => "Ошибка при считывании гипотез";
        public static string HypothesesFillingError => "Ошибка при заполнении гипотез";
    }
}
