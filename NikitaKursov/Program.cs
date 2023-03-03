using System;
using FILEWORKING;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace nikita_kr_final
{
    class Programm
    {
        public static void client(string path)
        {
            TcpClient eclient = new TcpClient("95.165.6.147", 55555);
            NetworkStream writerStream = eclient.GetStream();

            // класс для [де]сериализации данных
            BinaryFormatter format = new BinaryFormatter();
            byte[] buf = new byte[1024];

            int count;

            // открытие файла
            FileStream fs = new FileStream(path, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            // размер файла
            long k = fs.Length;

            // передача реального размера
            format.Serialize(writerStream, k.ToString());

            // сериализация и передача файла по 1024 байта
            while ((count = br.Read(buf, 0, 1024)) > 0)
            {
                format.Serialize(writerStream, buf);
            }

            Console.WriteLine("Объект сериализован");
            Console.WriteLine("Файл отправлен!");

            fs.Close();
            writerStream.Close();
        }

        // вывод инфомации об авторе
        public static void info()
        {
            Console.WriteLine("—————————————————————————————————————");
            Console.WriteLine("|          МАИ Институт №12         |");
            Console.WriteLine("|         Группа Т12О-209Б-19       |");
            Console.WriteLine("|           Атрашков Никита         |");
            Console.WriteLine("|           Курсовая работа         |");
            Console.WriteLine("—————————————————————————————————————");
            Console.WriteLine();
        }

        // меню программы
        public static void menu()
        {
            Console.WriteLine("Что вы хотите сделать?");
            Console.WriteLine("1. Вывести данные на экран.");
            Console.WriteLine("2. Произвести очистку данных от некорректной и недостоверной информации.");
            Console.WriteLine("3. Добавление информации о новом студенте.");
            Console.WriteLine("4. Отсортировать список по алфавиту.");
            Console.WriteLine("5. Редактирование информации о существующем студенте.");
            Console.WriteLine("6. Удаление информации о существующем студенте.");
            Console.WriteLine("7. Отправить данные.");
            Console.WriteLine("8. Выход.");
            Console.WriteLine();
        }


        public static void Main(string[] args)
        {
            info();
            int m;
            work_w_file obj = new work_w_file("C:/Users/MSI/source/repos/cours1.txt");
            obj.reading();

            do
            {
                menu();
                bool Tr;
                do
                {
                    Console.Write("Введите номер пункта: ");
                    Tr = Int32.TryParse(Console.ReadLine(), out m);
                    if (Tr != true || m < 1 || m > 8)
                        Console.WriteLine("Неправильное значение, оно должно быть в пределах диапозона 1...8");
                    if (obj._null == true)
                    {
                        do
                        {
                            Console.WriteLine("Файл пуст. Обработка невозможна. Закройте программу и повторите попытку или заполните таблицу, выбрав соответствующий пункт меню!");
                            menu();
                            Console.Write("Введите номер пункта: ");
                            Tr = Int32.TryParse(Console.ReadLine(), out m);
                            if (Tr != true || m < 1 || m > 8)
                                Console.WriteLine("Неправильное значение, оно должно быть в пределах диапозона 1...8");
                        } while (obj._null != false && m != 3);
                    }

                } while (Tr != true || m < 1 || m > 8);

                if (m == 1) { obj.show(); }

                else if (m == 2)
                {
                    obj.check_true_info();
                }
                else if (m == 3)
                {
                    if (obj._null == true)
                    {
                        obj.add_header();
                        obj = new work_w_file("/Users/gulya/Desktop/cours1.txt");
                        obj.reading();
                    }

                    string[] str = new string[obj.count_of_t];
                    Console.WriteLine("Введите Фамилию студента:");
                    str[0] = Console.ReadLine();

                    Console.WriteLine("Введите Имя студента:");
                    str[1] = Console.ReadLine();

                    Console.WriteLine("Введите Отчество студента:");
                    str[2] = Console.ReadLine();

                    Console.WriteLine("Введите баллы студента по Русскому языку:");
                    str[3] = Console.ReadLine();

                    Console.WriteLine("Введите баллы студента по Математике:");
                    str[4] = Console.ReadLine();

                    Console.WriteLine("Введите баллы студента по Информатике:");
                    str[5] = Console.ReadLine();

                    obj.add(str);
                }
                else if (m == 4)
                {
                    obj.sort_by_alf();
                    obj.show();
                }
                else if (m == 5)
                {
                    Console.WriteLine("Введите фамилию студента, информацию о котором требуется редактировать:");
                    string str = Console.ReadLine();
                    obj.edit_list(str);

                }
                else if (m == 6)
                {
                    Console.WriteLine("Введите фамилию студента, информацию о котором требуется удалить:");
                    string str = Console.ReadLine();
                    obj.del(str);
                }
                else if (m == 7)
                {
                    obj.write();
                    client(obj.path);
                }

            } while (m != 8);

            obj.write();
            Environment.Exit(0);
        }
    }
}
