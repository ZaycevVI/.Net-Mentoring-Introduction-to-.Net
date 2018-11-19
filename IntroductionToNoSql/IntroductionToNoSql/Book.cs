using System.Collections.Generic;
using System.Linq;

namespace IntroductionToNoSql
{
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public int Count { get; set; }
        public List<string> Genre { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            var str = $"[Name]: {Name}, [Author]: {Author}, [Count]: {Count}, [Year]: {Year}, [Genre]: ";

            return str + Genre.Aggregate((f, s) => f + "/" + s);
        }
    }
}