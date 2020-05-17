using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace _8PuzzleGame
{
    class Search
    {
        public Search()
        {

        }

        // სიგანეში ძებნის მეთოდი
        // ომელიც აბრუნებს იმ გზას რაც ალგორითმა გაიარა საწყისი მდგომარეობიდან
        // საბოლოო მიზნის მდგომარეობამდე
        public List<Vertex> BFS(Vertex root)
        {
            // აქ ვინახავთ გავლილ გზას, საწყისი მდგომარეობიდან საბოლოო მდგომარეობამდე
            List<Vertex> PathToSolution = new List<Vertex>();
            // ისეთი წვეროების სია რომლის შვილები ჯერ გამოკვლეული არაა
            List<Vertex> OpenList = new List<Vertex>();
            // ისეთი წვეროების სია რომლის შვილები გამოკვლეულია
            List<Vertex> ClosedList = new List<Vertex>();


            // პირველ საწყის წვეროს ვამატებთ გამოუკვლეველი წვეროების რიცხვში
            OpenList.Add(root);
            // მიზნის მიღწევის ლოგიკური ცვლადი რომელიც თავიდან მცდარია
            bool goalFound = false;

            // სანამ იარსებებს გამოუკვლეველი წვეროები და მიზანი მიღწეული არ იქნება მაშინ მანამ უნდა იმუშაოს 
            // ქვემოთ მოცემულმა ციკლმა
            while (OpenList.Count > 0 && !goalFound) 
            {
                // ქვემოთა სამი ხაზით , ვიღებთ გამოუკვლეველ წვეროს და ის გადაგვაქ გამოკვლეულ წვეროებსი და 
                // და ამის შემდეგ ვიწყებთ გამოკვლევას ...
                Vertex currentVertex = OpenList[0];
                ClosedList.Add(currentVertex);
                OpenList.RemoveAt(0);

                // ვადგენთ ყველა შესაძლო გადაადგილებას და ვწერთ currentVertex-ში
                // უფრო კონკრეტულად აქ ჩაიწერება მისი შვილების რაოდენობდა
                currentVertex.ExpandMove();

                // currentVertex წვეროს თითოეული შვილისთვის ვატარებთ შემდეგ ოპერაციებს -> 
                for (int i = 0; i < currentVertex.child.Count; i++)
                {
                    // ვითრებთ currentVertex კონკრეტულ ერთ შვილ წვეროს და ვწერთ currentChild ცვლდაში
                    Vertex currentChild = currentVertex.child[i];

                    // ვამოწმებთ არის თუ არა მიზანი მიღწეული
                    // თუ კი დავბეჭდავთ გზას საწყისიდან საბოლოო მდგომარეობამდე
                    if (currentChild.GoalTest()) 
                    {
                        Console.WriteLine("mizani migweulia.");
                        goalFound = true;
                        

                        PathTrace(PathToSolution,currentChild);
                    }

                    // თუ ჯერ კიდევ დარჩა გამოუკვლეველი წვერო და ის რომელიმე წვეროს შვილია
                    // მაშინ გააგრძელებს ალგორითმი მუშაობას და გამოუკვლეველი სიაში ჩასვამს მოცემულ წვეროს
                    if(!Contains(OpenList,currentChild) && !Contains(ClosedList,currentChild)) 
                    {
                        OpenList.Add(currentChild);
                    }
                }
            }


            return PathToSolution;
        }

        // დამხმარე მეთოდი რომელიც იწერს გზას კონკრეტული წერტილიდან
        public void PathTrace(List<Vertex> path,Vertex v)
        {
            Console.WriteLine("mimdinareobs gzis chawera..");
            Vertex current = v;
            path.Add(current);

            while (current.parent != null) 
            {
                current = current.parent;
                path.Add(current);
            }
        }


        // დამხმარე ლოგიკური მეთოდი რომელიც ამოწმებს თუ მოცემული წვერო არის თუ არა 
        // რომელიმე წვეროს შვილი, ანუ გასაგები რომ იყოს უფრო closedList და openList შეცავს თუ არა 
        // ამ კონკრეტულ მომენტში არსებულ წვეროს ანუ c , როგორც currentChild-ს
        public static bool Contains(List<Vertex> list, Vertex c)
        {
            bool contains = false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].IsSamePuzzle(c.puzzle))
                {
                    contains = true;
                }
            }
            return contains;
        }

    }
}
