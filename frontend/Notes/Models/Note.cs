﻿namespace Notes.Models
{
    public class Note
    {
        public int Id
        {
            get;
            set;
        }

        public string? Title
        {
            get;
            set;
        }

        public string? Body
        {
            get;
            set;
        }

        public User? User
        {
            get;
            set;
        }
    }
}
