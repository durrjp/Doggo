using System;
using System.ComponentModel;

namespace Doggo.Models
{
    public class Walks
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        [DisplayName("Dog Walker")]
        public int WalkerId { get; set; }
        public Walker Walker { get; set; }
        [DisplayName("Dog")]
        public int DogId { get; set; }
        public Dog Dog { get; set; }
        public Owner Owner { get; set; }
    }
}
