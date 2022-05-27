using System;
using System.Collections.Generic;
using System.Linq;

namespace Futurama
{
    class Program
    {
        static void MindSwitcher(ref int[][] replaced, ref List<int> particiants, int p1, int p2)
        {
            if (p1 == p2)
            {
                Console.WriteLine("Два одинаковых участника!");
                return;
            }
            if (replaced[p1][p2] != -1 || replaced[p2][p1] != -1)
            {
                Console.WriteLine("Данная пара участников уже обменивалась разумом!");
                return;
            }
            int temp = particiants[p1];
            particiants[p1] = particiants[p2];
            particiants[p2] = temp;
            replaced[p1][p2] = p2;
            replaced[p2][p1] = p1;
            Console.WriteLine($"Произошел обмен между {p1} и {p2}");
        }

        static List<List<int>> getCycles(List<int> bodies, List<int> minds)
        {
            List<List<int>> cycles = new List<List<int>>(); // Список циклов

            for(int i = 0; i < bodies.Count(); i++)
            {
                if (bodies[i] == -1) // Уже использовался
                    continue;
                var current = bodies[i];
                var next = minds[i];

                List<int> cycle = new List<int>(); // Новый цикл
                while (true)
                {
                    cycle.Add(current); // Добавление в текущий цикл
                    bodies[current] = -1; // Помечаем использованый 
                    current = next; 
                    if (bodies[next] != -1)
                        next = minds[current];
                    else
                        break;
                }
                cycles.Add(cycle); // Добавление текущего цикла в список циклов
            }
            return cycles;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите кол-во участников, совершивших обмен разумами: ");
            int n = int.Parse(Console.ReadLine());
            Console.WriteLine("\nНомера участников начинаются с 0!\n");

            //Добавление участников
            List<int> participants = new List<int>(n);
            for (int i = 0; i < n; i++)
            {
                participants.Add(i);
            }

            //Матрица обменов разумами
            int[][] replaced = new int[participants.Count() + 2][];
            for(int i = 0; i < replaced.Length; i++)
            {
                replaced[i] = new int[participants.Count() + 2];
                for (int j = 0; j < replaced[i].Length; j++)
                    replaced[i][j] = -1;
            }
            Console.WriteLine("Для продожения нажмите Enter!\n");

            //Обмен разумов
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            while (keyInfo.Key != ConsoleKey.Spacebar)
            { 
                Console.WriteLine("Введите очередную пару обменов разумами: ");
                int t1, t2;
                bool b1 = int.TryParse(Console.ReadLine(), out t1);
                bool b2 = int.TryParse(Console.ReadLine(), out t2);
                if (!b1 || !b2 || t1 >= n || t1 < 0 || t2 >= n || t2 < 0)
                    Console.WriteLine("Некорректный номер участника!\n" +
                        "Для продожения ввода нажмите Enter!\n" +
                        "Для окончания ввода нажмите Space!\n");
                else
                {
                    MindSwitcher(ref replaced, ref participants, t1, t2);
                    Console.WriteLine("Для продожения ввода нажмите Enter!" +
                        "\nДля окончания ввода нажмите Space!\n");
                }
                keyInfo = Console.ReadKey();
            }

            //Вывод участников после обмена
            Console.WriteLine("\nТело и разум, после обменов: ");
            for (int i = 0; i < participants.Count(); i++)
                Console.WriteLine($"Участник {i} ---> {participants[i]}");

            //Список тел
            List<int> bodies = new List<int>(n);
            //Список разумов
            List<int> minds = new List<int>(n);

            for (int i = 0; i < participants.Count(); i++)
            {
                bodies.Add(i);
                minds.Add(participants[i]);
            }
            Console.WriteLine();

            //Получение циклов
            Console.WriteLine("Найденные циклы: ");
            var cycles = getCycles(bodies, minds);
            foreach (var cycle in cycles)
            {
                foreach (var part in cycle)
                    Console.Write($"{part} ");
                Console.WriteLine();
            }
            Console.WriteLine();

            //Два вспомогательных участника
            int helper1 = n;
            int helper2 = n + 1;
            participants.Add(helper1);
            participants.Add(helper2);

            //Возвращение всех разумов в свои тела
            foreach (var cycle in cycles)
            {
                MindSwitcher(ref replaced, ref participants, helper1, cycle[cycle.Count() - 1]);
                for (int i = 0; i < cycle.Count(); i++)
                    MindSwitcher(ref replaced, ref participants, helper2, cycle[i]);
                MindSwitcher(ref replaced, ref participants, helper1, cycle[0]);
                MindSwitcher(ref replaced, ref participants, helper1, helper2);
            }

            //Вывод участников после обмена
            Console.WriteLine("\nТело и разум, после обменов: ");
            for (int i = 0; i < participants.Count(); i++)
                Console.WriteLine($"Участник {i} ---> {participants[i]}");
        }
    }
}
