global using Microsoft.VisualStudio.TestTools.UnitTesting;
global using System;
global using System.Linq;
global using Tellurian.Geospatial.Tests;

namespace Tellurian.Geospatial.Tests;

internal static class TestData
{
    public static readonly Position Hövik = Position.FromDegrees(58.033785, 11.744987);
    public static readonly Position Höviksnäs = Position.FromDegrees(58.033157, 11.754460);

    public static readonly Position Stockholm = Position.FromDegrees(59.326242, 17.841972);
    public static readonly Position Wellington = Position.FromDegrees(-41.2442198, 174.6918153);

}
