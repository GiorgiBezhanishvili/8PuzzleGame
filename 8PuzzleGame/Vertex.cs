using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace _8PuzzleGame
{
    class Vertex
    {
        public List<Vertex> child = new List<Vertex>(); // "შვილი" წვეროების/მოქმედებების შესანახად
        public Vertex parent; // მშობელი წვეროების შესანახად
        // ფაზლი რომელიც წარმოდგელინია ერთგანზომილებიანი მასივით
        // O(n) წფრივი დროის მისაღწევად და ზედემეტი ოპერაციების შესამცირებლად
        public int[] puzzle = new int[9];  
        public int index = 0;
        public int col = 3;

        public Vertex(int[] pz)
        {
            for (int i = 0; i < 9; i++)
            {
                puzzle[i] = pz[i];
            }
        }

        // მოცემული მეთოდი ტესტავს არის თუ არა მიზანი მიღწეული
        // მუშაობის პრინციპი : მიზანი მიღწეული იქნება მაშინ როდესაც 
        // "მატრიცაში"(ჩვენს შემთხვევაში მასივში) რიცხვები განლაგებული იქნება ზრდადობის მიხედვით 0-დან 8-მდე
        public bool GoalTest() 
        {
            bool goal = true;
            int mx = puzzle[0];

            for (int i = 1; i < puzzle.Length; i++)
            {
                if (mx > puzzle[i])
                    goal = false;
                mx = puzzle[i];
            }
            return goal;
        }


        // მოცემული კოდის რეგიონი ასრულებს რიცხვების მოძრაობას ზემოთ,ქვემოთ, მარცხნივ და მარჯვნივ
        #region MoveTo

        public void MoveRight(int[] puzzle,int index) // გადაადგილება მარჯვნივ
        {
            /* მარჯვნივ გადაადგილე ხდება მაშინ თუ გადასაადგილებელი ელემენტი არ დგას 
             * უკიდურეს მარჯვენა მხარეს (ანუ ისეთ მახარეზე სადაც მარჯვნივ ვერ გადაადგილდები).
             * რადგან საქმე წრფივ მასივთან გჰვაქ და ჩვენი ფაზლის ელემენტები მასშია , მაშინ ეს იმას ნიშნავს რომ
             * ყოველი სტრიქონის ბოლო (უკიდურესი მარჯვება მხარე) იქნება მაქსიმალური თავისთავად და 
             * ამ რიცხვის მოდული სვეტების რაოდენობასთან ყოველთვის მოგვცემს შედეგს სვეტების რაოდენობას დაკლებული ერთი,
             * და "დაკლებული" ერთი იმიტომ რომ მესამე სვეტზე(მარჯვენა მახარეს) მდგომი  ელემენტი 
             * მარჯვენა მხარეს ვერ გადაადგილდება
             */
            if (index % col < col - 1)
            {
                int[] newPuzzle = new int[9];
                CopyPzl(newPuzzle, puzzle);

                int tmp = newPuzzle[index + 1];
                newPuzzle[index + 1] = newPuzzle[index];
                newPuzzle[index] = tmp;

                Vertex child = new Vertex(newPuzzle);
                this.child.Add(child);
                child.parent = this;
            }
        }

       
        public void MoveLeft(int[] puzzle, int index) // გადაადგილება მარცხნივ
        {
            // აქ რადგან მარცხენა მხარესაა გადააადგილება მაშასადამე უკიდურესი მარცხენა სვეტი
            // არ ჩაითვლება (რადგან მარცხნივ ვერ გადავადგილდებით) და ეს იმას ნიშნავს რომ მარცხნიდან
            // პირველივე სვეტი გამოაკლდება საქმეს, ანუ ეს იქნება 0 ინდექსზე მდგარი სვეტი
            if (index % col > 0) 
            { 
                int[] newPuzzle = new int[9];
                CopyPzl(newPuzzle, puzzle);

                int tmp = newPuzzle[index - 1];
                newPuzzle[index - 1] = newPuzzle[index];
                newPuzzle[index] = tmp;

                Vertex child = new Vertex(newPuzzle);
                this.child.Add(child);
                child.parent = this;
            }
        }

        public void MoveUP(int[] puzzle, int index) // გადაადგილება ზემოთ 
        {
            // ამ შემთხვევაში რადგანა სვეტების და სტრიქონების რაოდენობა ერთიდაიგივეა ისევ ვიყენებთ col ცვლადს
            // და აქ პირველი სტრიქონი არ გამოგვადგება , ასერომ პირველ სტრიქონზე არსებულ ინდექსებს თუ გამოვაკლებთ 
            // სტრიქონების მაქსიმალურ რაოდენობას შედეგი იქნება უარყოფითი და თუ ესეთი შედეგი დაგვიდაგა, უბრალოდ გამოვტოვებთ ამ სვლას

            if (index - col >= 0)
            {
                int[] newPuzzle = new int[9];
                CopyPzl(newPuzzle, puzzle);

                int tmp = newPuzzle[index - 3];
                newPuzzle[index - 3] = newPuzzle[index];
                newPuzzle[index] = tmp;

                Vertex child = new Vertex(newPuzzle);
                this.child.Add(child);
                child.parent = this;
            }
        }

        public void MoveDown(int[] puzzle, int index) // გადაადგილება ქვემოთ
        {
            // აქ უნდა გამოვრიცხოთ ბოლო სტრიქონი
            // ამიტომ ინდექსს მიმატებული სტრიქონის საერთო სიგრძე არ უნდა აღემატებოდეს
            // ფაზლის მასივის საერთო სიგრძეს
            if (index + col < puzzle.Length)
            {
                int[] newPuzzle = new int[9];
                CopyPzl(newPuzzle, puzzle);

                int tmp = newPuzzle[index + 3];
                newPuzzle[index + 3] = newPuzzle[index];
                newPuzzle[index] = tmp;

                Vertex child = new Vertex(newPuzzle);
                this.child.Add(child);
                child.parent = this;
            }
        }

        // დამხმარე მეთოდი ერთი მასივის მეორეში კოპირებისთვის
        public void CopyPzl(int[] x,int[] y) 
        {
            for (int i = 0; i < y.Length; i++)
            {
                x[i] = y[i];
            }
        }
        #endregion

        // მოცემული მეთოდით ვბეჭდავთ ფაზლს მატრიცულ ფორმაში
        public void PrintPzl() 
        {
            Console.WriteLine();
            int k = 0;
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write(puzzle[k] + " ");
                    k++;
                }
                Console.WriteLine();
            }
        }

        // მოცემული მეთოდი ამოწმებს არგუმენტად მიღებული ფაზლი ემთხვევა თუ არა
        // კლასში გამოცხადებული puzzle მასივს
        public bool IsSamePuzzle(int[] pzl) 
        {
            bool samePuzzl = true;
            for (int i = 0; i < pzl.Length; i++)
            {
                if (puzzle[i] != pzl[i]) 
                {
                    samePuzzl = false;
                }
            }
            return samePuzzl;
        }

        // მოცემული მეთოდი თვლის ყოველ შესაძლო სვლას კონკრეტული მდგომარეობიდან გამომდინარე
        // მაგალითად თუ ნული ცენტრშია მოთავსებული მოცემული ფუნქცია გამოთვლის ნულის მდგომარეობას 
        // მარჯვნი, მარცხნივ, ზემოთ და ქვემოთ
        public void ExpandMove() 
        {
            for (int i = 0; i < puzzle.Length; i++)
            {
                if (puzzle[i] == 0)
                    index = i;
            }

            MoveRight(puzzle,index);
            MoveLeft(puzzle, index);
            MoveDown(puzzle, index);
            MoveUP(puzzle, index);
        }



    }
}
