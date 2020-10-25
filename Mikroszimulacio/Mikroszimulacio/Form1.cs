using Mikroszimulacio.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mikroszimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1234);

        List<Person> male = new List<Person>();
        List<Person> female = new List<Person>();

        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Temp\halál.csv");

        }

        private void Simulation()
        {
            for (int year = 2005; year <= 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();

                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
        }

        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;

            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.DeathP).FirstOrDefault();

            if (rng.NextDouble() <= pDeath)
            {
                person.IsAlive = false;
            }

            if (person.IsAlive&&person.Gender==Gender.Female)
            {
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.BirthP).FirstOrDefault();

                if (rng.NextDouble()<=pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);

                }
            }

            if (person.IsAlive && person.Gender == Gender.Male) male.Add(person);
            if (person.IsAlive && person.Gender == Gender.Female) female.Add(person);


        }

        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person() { BirthYear = int.Parse(line[0]), Gender = (Gender)Enum.Parse(typeof(Gender), line[1]), NbrOfChildren = int.Parse(line[2]) });
                }
            }

            return population;
        }

        public List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> birthProbabilities = new List<BirthProbability>();

            using(StreamReader sr=new StreamReader(csvpath, Encoding.Default))
            {
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthProbabilities.Add(new BirthProbability() { Age = int.Parse(line[0]), NbrOfChildren = int.Parse(line[1]), BirthP = double.Parse(line[2]) });
                }
            }

            return birthProbabilities;
        }

        public List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deathProbabilities = new List<DeathProbability>();

            using(StreamReader sr=new StreamReader(csvpath, Encoding.Default))
            {
                while(!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathProbabilities.Add(new DeathProbability() { Gender = (Gender)Enum.Parse(typeof(Gender), line[0]), Age = int.Parse(line[1]), DeathP = double.Parse(line[2]) });
                }
            }

            return deathProbabilities;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            male.Clear();
            female.Clear();
            richTextBox1.Clear();

            Simulation();

            DisplayResults();
        }

        private void DisplayResults()
        {
            for (int i = 2006; i <= 2024; i++)
            {
                richTextBox1.Text += "Szimulációs év: " + i + "\n" + "\t" + "Fiúk: " + male.Count() + "\n" + "\t" + "Lányok: " + female.Count() + "\n" + "\n";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                textBox1.Text = ofd.FileName;
            }
        }
    }
}
