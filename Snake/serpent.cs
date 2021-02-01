using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Snake
{
    public class serpent
    {
        public int Dim;
        public int[] Vect;
        public int[] Pos;
        public Canvas canvas;
        public List<System.Windows.Shapes.Rectangle> Corps = new List<Rectangle>();
        public List<int[]> PosCorps = new List<int[]>();
        public bool Fed = false;
        public Random rdm = new Random();
        public System.Windows.Shapes.Rectangle Food = new Rectangle(){Fill = Brushes.Red};
        public int[] PosFood;
        public int[] LastPos = new int[]{0,0};
        public bool IsDead = false;

        public serpent(int dimension,Canvas canv)
        {
            Dim = dimension;
            canvas = canv;
            Vect = new int[]{1,0};
            Pos = new int[]{rdm.Next(800/Dim),rdm.Next(800/Dim)};
            PosCorps.Add(Pos);
            Array.Copy(Pos,LastPos,2);
            Naissance();

            Food.Height = Dim;
            Food.Width = Dim;
            PosFood = new int[]{rdm.Next(800/Dim), rdm.Next(800/Dim)};
            Canvas.SetLeft(Food, PosFood[0] * Dim);
            Canvas.SetTop(Food, PosFood[1] * Dim);
            canvas.Children.Add(Food);
        }

        public void Naissance()
        {
            var rect = new System.Windows.Shapes.Rectangle();
            rect.Fill = Brushes.AntiqueWhite;
            rect.Height = Dim;
            rect.Width = Dim;
            Canvas.SetLeft(rect,Pos[0]*Dim);
            Canvas.SetTop(rect,Pos[1]*Dim);
            Corps.Add(rect);
            canvas.Children.Add(rect);
        }

        public void MajCorps()
        {
            Array.Copy(PosCorps[PosCorps.Count-1],LastPos,2);
            for (int i = Corps.Count-1; 0 < i; i--)
            {
                var pos = new int[2]{0,0};
                Array.Copy(PosCorps[i-1],pos,2);
                var rect = Corps[i];
                Canvas.SetLeft(rect,pos[0]*Dim);
                Canvas.SetTop(rect,pos[1]*Dim);
                Array.Copy(pos,PosCorps[i],2);
            }
            MajPos();
            var tete = Corps[0];
            Canvas.SetLeft(tete, Pos[0] * Dim);
            Canvas.SetTop(tete, Pos[1] * Dim);
        }

        public void MajPos()
        {
            Pos[0] = (Pos[0] + Vect[0])%(800/Dim);
            Pos[1] = (Pos[1] + Vect[1])%(800/Dim);
            CheckDeath();
            if (Pos[0] < 0)
            {
                Pos[0] = 800 / Dim - 1;
            }
            if (Pos[1] < 0)
            {
                Pos[1] = 800 / Dim - 1;
            }
        }

        public void SetFood()
        {
            if (Fed)
            {
                FindSlot();
                Canvas.SetLeft(Food,PosFood[0]*Dim);
                Canvas.SetTop(Food,PosFood[1]*Dim);
                Fed = false;
            }
        }

        private void HasAte()
        {
            if (Pos.SequenceEqual(PosFood))
            {
                Fed = true;

                var rect = new System.Windows.Shapes.Rectangle {Fill = Brushes.AntiqueWhite, Height = Dim, Width = Dim, Stroke = Brushes.Black, StrokeThickness = 1};
                Canvas.SetLeft(rect,LastPos[0]*Dim);
                Canvas.SetTop(rect,LastPos[1]*Dim);
                canvas.Children.Add(rect);
                Corps.Add(rect);
                var newPos = new int[2]{0,0};
                Array.Copy(LastPos,newPos,2);
                PosCorps.Add(newPos);
            }
        }

        private void FindSlot()
        {
            var pos = new int[] { rdm.Next(800 / Dim), rdm.Next(800 / Dim) };
            bool test = true;
            foreach (var posCorps in PosCorps)
            {
                if (posCorps.SequenceEqual(pos))
                {
                    test = false;
                }
            }

            if (test)
            {
                PosFood = pos;
            }
            else
            {
                FindSlot();
            }
        }

        public void CheckDeath()
        {
            if (PosCorps.FindAll(o => o.SequenceEqual(Pos)).Count == 2)
            {
                IsDead = true;
            }
        }

        public void Update()
        {
            MajCorps();
            HasAte();
            SetFood();
        }
    }
}
