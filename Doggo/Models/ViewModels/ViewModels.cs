﻿using System.Collections.Generic;

namespace Doggo.Models.ViewModels
{
    public class ProfileViewModel
    {
        public Owner Owner { get; set; }
        public List<Walker> Walkers { get; set; }
        public List<Dog> Dogs { get; set; }
    }
    public class DogFormViewModel
    {
        public Dog Dog { get; set; }
        public List<Owner> Owners { get; set; }
    }
    public class OwnerFormViewModel
    {
        public Owner Owner { get; set; }
        public List<Dog> Dogs { get; set; }
        public List<Neighborhood> Neighborhoods { get; set; }
    }

    public class WalkerViewModel
    {
        public Walker Walker { get; set; }
        public List<Walks> Walks { get; set; }
        public int TotalWalksDuration { get; set; }
    }
}
