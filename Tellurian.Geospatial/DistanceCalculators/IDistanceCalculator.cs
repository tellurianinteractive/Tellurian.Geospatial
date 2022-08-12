﻿namespace Tellurian.Geospatial.DistanceCalculators;

/// <summary>
/// Abtraction of distance calculation making it possible to 
/// extend with other distance calcutaion formulas.
/// </summary>
public interface IDistanceCalculator
{
    Distance GetDistance(in Position from, in Position to);
}
