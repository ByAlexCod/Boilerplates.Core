using System.Collections.Generic;
using Boilerplates.Core;
using NUnit.Framework;

namespace Boilerplates.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Boilerplate bp = new Boilerplate("./TestReadme");
            var annalyzeResult = bp.GetBPLPlaceholders();
            Assert.Contains("Bpl_project_name", annalyzeResult);
            Assert.Contains("bpl_class_name", annalyzeResult);
            Assert.Contains("bpl_project_class_name", annalyzeResult);
            Assert.Contains("BPL_ONE_FOLDER", annalyzeResult);
            Assert.Contains("bpl_Readme.md", annalyzeResult);
        }

        [Test]
        public void Test2()
        {
            Boilerplate bp = new Boilerplate("./TestReadme");
            var annalyzeResult = bp.GetBPLPlaceholders();
            var mapping = new Dictionary<string, string>();
            mapping.Add("Bpl_project_name", "Blablabla");
            mapping.Add("BPL_ONE_FOLDER", "Niazaki");
            mapping.Add("bpl_class_name", "Class 1");
            bp.Boil("./Boulou", mapping);
        }
    }
}