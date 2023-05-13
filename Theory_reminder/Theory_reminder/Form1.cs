using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Theory_reminder
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///Лист загруженных вопросов-ответов
        /// </summary>
        List<string> CONSTANT_list_Answer_Question = new List<string>();


        /// <summary>
        ///Лист, который хранит неотвеченные вопрос-ответы (обновляется с каждым ответом)
        /// </summary>
        List<string> UPDATING_list_Question = new List<string>();  

        List<string> list_Success_answers = new List<string>();
        List<string> list_Failure_answers = new List<string>();

        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;

        }

 

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)// Кнопка выбрать папку
        {
            folderBrowserDialog1.SelectedPath = System.IO.Directory.GetCurrentDirectory();

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                //путь с текст файлами. Обычно это папка с названием главной темы
                string pathWithTopics = folderBrowserDialog1.SelectedPath;

                string pathToCurrent_txt_file = "";

                //массив путей(включая имена) текстовых файлов из нашей папки
                string[] sections_topics = Directory.GetFiles(pathWithTopics, "*.txt");

                if (sections_topics.Length == 0) MessageBox.Show("Темы в данном каталоге не обнаружены");

                string nameTextFile = "";

                //Читаем каждый текст файл в папке и добавляем в наши листы вопросов-ответов
                for (int i = 0; i < sections_topics.Length; i++)
                {
                    //получаем имя отдельного текст файла с расширением
                    nameTextFile = Path.GetFileName(sections_topics[i]).ToString();

                    //получаем путь отдельного текст файла
                    pathToCurrent_txt_file = sections_topics[i];

                    bool txtFileExists = File.Exists(pathToCurrent_txt_file);

                    //если файл существует 
                    if (txtFileExists)
                    {
                        //?
                        string[] allLinesTxtFile = File.ReadAllLines(pathToCurrent_txt_file);

                        //Получаем полный текст файла
                        string fullStringFromFile = File.ReadAllText(pathToCurrent_txt_file);

                        //Получаем отдельные вопрос-ответы, которые разделены в файле символом ~
                        string[] individual_questions = fullStringFromFile.Split('~');

                        string answer = "";
                        string question = "";


                        //Теперь для каждого вопрос-ответа
                        for (int t = 0; t < individual_questions.Length; t++)
                        {
                            //Запоминаем вопрос
                            question = individual_questions[t].Split('►')[0]; Console.WriteLine("question = " + question);

                            //Запоминаем ответ
                            answer = individual_questions[t].Split('►')[1];   Console.WriteLine("answer = " + answer);

                            //Добавляем вопрос-ответы в лист
                            if(!CONSTANT_list_Answer_Question.Contains(question + "►" + answer)) CONSTANT_list_Answer_Question.Add(question + "►" + answer);

                            //Добавляем вопрос-ответы в лист, из которого мы их будем удалять после ответов
                            if (!UPDATING_list_Question.Contains(question + "►" + answer)) UPDATING_list_Question.Add(question + "►" + answer);

                            
                            //listBox1.Items.Add(question);

                        }

                        //Показываем в правом верхнем окне сколько осталось ответить
                        textBox2.Text = "0/" + individual_questions.Length;

                        
                        int rnd = random.Next(0, CONSTANT_list_Answer_Question.Count);

                        //Выводим в верхнее окно вопрос на текущий ответ из оставшихся вопросов
                        textBox3.Text = UPDATING_list_Question.ElementAt(rnd).ToString().Split('►')[0];

                        //Выводим в среднее окно ответ на текущий вопрос из оставшихся ответов
                        textBox4.Text = UPDATING_list_Question.ElementAt(rnd).ToString().Split('►')[1];


                    }

                }

            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

       
        private void button2_Click(object sender, EventArgs e)//Кнопка Ответить на вопрос
        {
            //Если поле ВВОДА ответа не пустое
            if (textBox1.Text != null)
            {

                if(UPDATING_list_Question.Count != 0)
                {
                    //Если поле с правильным ответов содержит наш написанный текст
                    if (textBox4.Text.Contains(textBox1.Text))
                    {
                        //MessageBox.Show("Верно");
                        textBox1.ForeColor = Color.Green;
                        
                        //Добавляем в лист верно отвеченных вопросов
                        if (!list_Success_answers.Contains(textBox3.Text)) list_Success_answers.Add(textBox3.Text);

                        //Обновляем лист оставшихся вопрос-ответов
                        if (UPDATING_list_Question.Count != 0) UPDATING_list_Question.Remove(textBox3.Text + '►' + textBox4.Text);

                    }
                    else
                    {
                        //MessageBox.Show("Не верно");
                        textBox1.ForeColor = Color.Red;

                        //Добавляем в лист не верно отвеченных вопросов
                        if (!list_Failure_answers.Contains(textBox3.Text)) list_Failure_answers.Add(textBox3.Text);

                        //Обновляем лист оставшихся вопрос-ответов
                        if (UPDATING_list_Question.Count != 0) UPDATING_list_Question.Remove(textBox3.Text + '►' + textBox4.Text);

                    }

                }

            }
        }

        private void button5_Click(object sender, EventArgs e) //Кнопка следующий вопрос
        {
            //очищаем поле ввода
            textBox1.Text = "";

            //Если у нас есть неотвеченные вопросы
            if(UPDATING_list_Question.Count != 0)
            {
                int rnd = random.Next(0, UPDATING_list_Question.Count);
                
                //Выводим рандомный вопрос-ответ в окна
                textBox3.Text = UPDATING_list_Question.ElementAt(rnd).ToString().Split('►')[0];
                textBox4.Text = UPDATING_list_Question.ElementAt(rnd).ToString().Split('►')[1];

                textBox1.ForeColor = Color.Black;
            }
            else if (UPDATING_list_Question.Count == 0 && list_Success_answers.Count != 0 || list_Failure_answers.Count != 0)//?
            {
                //Тут проверяем что вопросов больше нет и верных\неверных тоже нет(по идее последние две проверки не нужны т.к они заполняются по мере ответов)
                MessageBox.Show("Все вопросы пройдены");


                string SuccessResult = "Верно отвечено на : " + '\r' + '\n';;
                for (int i = 0; i < list_Success_answers.Count; i++)
                {
                    SuccessResult += list_Success_answers[i] + '\r' + '\n';
                }

                //textBox4.Text = result;


                //string FailureResult = "Нужно поизучать :" + '\r' + '\n';

                SuccessResult += "Нужно поизучать :" + '\r' + '\n'; ;


                for (int i = 0; i < list_Failure_answers.Count; i++)
                {
                    SuccessResult += list_Failure_answers[i] + '\r' + '\n';
                }

                textBox4.Text = SuccessResult;


            }

        }

        private void button4_Click(object sender, EventArgs e) //Кнопка предыдущий вопрос
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
