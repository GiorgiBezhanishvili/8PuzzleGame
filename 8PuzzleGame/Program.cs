using System;
using System.Collections.Generic;

namespace _8PuzzleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // ვქმნით ფაზლს ერთგანზომილებიან მასივში
            int[] puzzle = new int[9]
            {
                1,2,4,
                3,0,5,
                7,6,8
            };
            // ვქმნით Vertex ობიექტს და ვაწოდებთ puzzle მასივს
            Vertex vertex = new Vertex(puzzle);
            // ვქმნით Search ობიექტს
            Search search = new Search();

            // solution სიაში ვყრით ყველა გზას რაც ალგორითმა გაიარა საწყისიდან საბოლოო მიზნის მგომარეობამდე
            List<Vertex> solution = search.BFS(vertex);


            // ვბეჭდავთ მოცემულ "გზებს" თუ არის რათქმაუნდა შესაბამისი გზა.
            if (solution.Count > 0)
            {
                solution.Reverse();
                for (int i = 0; i < solution.Count; i++)
                {
                    solution[i].PrintPzl();
                }
            }
            else 
            {
                Console.WriteLine("gza miznis mdgomareobamde ver moidzebna");
            }

        }
    }
}
