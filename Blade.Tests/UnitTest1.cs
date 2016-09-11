﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using BladeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//http://odetocode.com/articles/80.aspx
namespace Blade.Tests
{
    [TestClass]
    public class UnitTest1
    {

        

        [TestMethod]
        public void TestMethod0_1()
        {

            var testSetUp = new TestSetUp(
                "{{var x=200}}{{x+Amount}}",
                new { Amount = 100 },
                "300");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod0_5()
        {

            var testSetUp = new TestSetUp(
                "{{var x=200}}{{if x>Amount}}show me{{else}}dont{{/if}}",
                new { Amount = 100 },
                "show me");

            testSetUp.Blade.Test(testSetUp);
        }

        [TestMethod]
        public void TestMethod0_5_1()
        {

            var testSetUp = new TestSetUp(
                "{{var x=50}}{{if x>Amount}}show me{{else}}dont{{/if}}",
                new { Amount = 100 },
                "dont");

            testSetUp.Blade.Test(testSetUp);
        }

        [TestMethod]
        public void TestMethod0_100()
        {

            var testSetUp = new TestSetUp(
                "{{100+200+300}}",
                new { Amount = 100 },
                "600");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod0_102()
        {

            var testSetUp = new TestSetUp(
                "{{float.Parse('3.141592654')}}",
                new { Amount = 100 },
                "3.141593");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod0_101()
        {

            var testSetUp = new TestSetUp(
                "In the year {{DateTime.Now.Year}}, its so easy to forget everything",
                new { Amount = 100 },
                 "In the year "+DateTime.Now.Year.ToString()+ ", its so easy to forget everything");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod0_4()
        {

            var testSetUp = new TestSetUp(
                "{{var x=200}}{{var y=50}}{{x*y}}",
                new { Amount = 100 },
                "10000");

            testSetUp.Blade.Test(testSetUp);
        }




        
        [TestMethod]
        public void TestMethod0_1_3()
        {

            var testSetUp = new TestSetUp(
                @"{{var divStyle=display:none}}
                  <div style=""{{divStyle}}"">Hey!I'm invinsible</div>",
                new {  },
                @"<div style=""display:none"">Hey!I'm invinsible</div>");

            testSetUp.Blade.Test(testSetUp,true,true);
        }
        [TestMethod]
        public void TestMethod0()
        {

            var testSetUp = new TestSetUp(
                "Name {{Name}}",
                new { Name = "Samuel" },
                "Name Samuel");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod1()
        {

            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{else}}yo!{{/if}}",
                new { IsGood = true },
                "I will do");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod1_1()
        {

            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}
                      I will do
                  {{else}}
                      yo!
                  {{/if}}",
                new { IsGood = true },
                "I will do");

            testSetUp.Blade.Test(testSetUp,true,true);
        }

       

        [TestMethod]
        public void TestMethod2()
        {

            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{else}}yo!{{/if}}",
                new { IsGood = false },
                "yo!");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod2_1()
        {

            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{/if}}",
                new { IsGood = false },
                "");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod2_2()
        {

            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{/if}}",
                new { IsGood = true },
                "I will do");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod3()
        {

            var testSetUp = new TestSetUp(
                 @"{{item in MyList at index}}<div>no.{{index}}:{{item}}</div>{{/item}}",
                new { MyList = new List<string>() { "a", "b", "w" } },
                "<div>no.0:a</div><div>no.1:b</div><div>no.2:w</div>");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod3_1()
        {

            var testSetUp = new TestSetUp(
                 @"{{item in MyList}}<div>{{item}}</div>{{/item}}",
                new { MyList = new List<string>() { "a", "b", "w" } },
                "<div>a</div><div>b</div><div>w</div>");

            testSetUp.Blade.Test(testSetUp);
        }

        [TestMethod]
        public void TestMethod345()
        {

            var testSetUp = new TestSetUp(
                 @"{{var t=100}}{{item in MyList at index}}<div>no.{{index+t}}:{{item}}</div>{{/item}}",
                new { MyList = new List<string>() { "a", "b", "w" } },
                "<div>no.100:a</div><div>no.101:b</div><div>no.102:w</div>");

            testSetUp.Blade.Test(testSetUp);
        }

        [TestMethod]
        public void TestMethod500()
        {

            var testSetUp = new TestSetUp(
                 @"{{10}}",
                new { },
                "10");

            testSetUp.Blade.Test(testSetUp);
        }

        [TestMethod]
        public void TestMethod501()
        {

            var testSetUp = new TestSetUp(
                 @"{{var y=Amount}}{{var x=200}}{{y+x}} is good",
                new {Amount=300 },
                "500 is good");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod501_2()
        {

            var testSetUp = new TestSetUp(
                 @"{{var x=200}}{{var y=Amount}}{{y+x}} is good",
                new { Amount = 300 },
                "500 is good");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod3_123()
        {

            var testSetUp = new TestSetUp(
                 @"{{var t=100}}{{item in MyList}}<div>{{t}}{{item}}</div>{{/item}}",
                new { MyList = new List<string>() { "a", "b", "w" } },
                "<div>100a</div><div>100b</div><div>100w</div>");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod3_combination()
        {

            var testSetUp = new TestSetUp(
                 @"{{item in MyList}}<div>{{item}}</div>{{/item}}"+
                 @"{{item in MyList at index}}<div>no.{{index}}:{{item}}</div>{{/item}}",
                new { MyList = new List<string>() { "a", "b", "w" } },
                "<div>a</div><div>b</div><div>w</div>"+
                "<div>no.0:a</div><div>no.1:b</div><div>no.2:w</div>");

            testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod4()
        {

            var testSetUp = new TestSetUp(
                @"You.Name+Name:{{You.Name + Name}}",
                 new
                 {
                     Name = "Sam",
                     You = new
                     {
                         Name = "Sam2"
                     },
                 },
                "You.Name+Name:Sam2Sam");

         var result=   testSetUp.Blade.Test(testSetUp,false);
        }

        [TestMethod]
        public void TestMethod5()
        {

            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount + Amount}}",
                 new
                 {
                     Amount = 3,
                     You = new
                     {
                         Amount = 5
                     },
                 },
                "You.Amount+Amount:8");

            var result = testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod6()
        {

            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount - Amount}}",
                 new
                 {
                     Amount = 3,
                     You = new
                     {
                         Amount = 5
                     },
                 },
                "You.Amount+Amount:2");

            var result = testSetUp.Blade.Test(testSetUp);
        }
        [TestMethod]
        public void TestMethod7()
        {

            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount / Amount}}",
                 new
                 {
                     Amount = 2,
                     You = new
                     {
                         Amount = 10
                     },
                 },
                "You.Amount+Amount:5");

            var result = testSetUp.Blade.Test(testSetUp);
        }
        //todo FAILING TEST - UNABLE TO DO NESTED IFF
       // [TestMethod]
        public void TestMethod1_2()
        {

            var testSetUp = new TestSetUp(
                @"
                  {{if IsGood == true}}
                      I will do
                  {{else}}
                     {{if IsNotGood == false}}
                         I will do
                     {{else}}
                         yo!
                     {{/if}}
                  {{/if}}
                 ",
                new { IsGood = false, IsNotGood = false },
                "yo!");

            testSetUp.Blade.Test(testSetUp, true, true);
        }

        //TODO THIS TEST SEEM TO NEVER RETURN
       // [TestMethod]
        public void LoadTest()
        {
            var totalNumberOfIteration = 1;
            var testSetUp = new TestSetUp(
               TestHelper.sample0 + TestHelper.sample1 + TestHelper.sample2 + TestHelper.sample3,
                 new
                 {
                     Name = "Sam",
                     Amount = 10,
                     IsGood = true,
                     You = new
                     {
                         Name = "Sam2",
                         Amount = 3,
                         IsGood = false,
                     },
                     MyList = new List<string>() { "a", "b", "c" },
                     //MyBigList = new List<dynamic>() {  new
                     //{
                     //    Name = "100",
                     //    Amount = 100,
                     //    IsGood = false,
                     //}, new
                     //{
                     //    Name = "1000",
                     //    Amount =1000,
                     //    IsGood = false,
                     //} }
                 }, "", totalNumberOfIteration);

            testSetUp.Blade.Test(testSetUp, false);
        }

    }


}