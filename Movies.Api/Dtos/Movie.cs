﻿using Movies.Shared;

namespace Movies.Api
{
    public class Movie
    {
        //public int Id { get; internal set; }
        public string Title { get; internal set; }

        public MpaaRating MpaaRating { get; internal set; }
        public double Rating { get; internal set; }

        public override string ToString() =>
            $"{nameof(Title)}: '{Title}'," +
            $" {nameof(MpaaRating)}: '{MpaaRating}'," +
            $" {nameof(Rating)}: '{Rating}'";
    }
}