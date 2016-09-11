using System;
using System.Collections.Generic;
using HonjoLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Honjo.Tests
{
    [TestClass]
    public class when_honjo_is_used
    {
        [TestMethod]
        public void sample_test()
        {
            var result = new HonjoLib.Honjo().Compile("{{var x=200}}{{x+Amount}}", new {Amount = 100});

            Assert.AreEqual("300", result);
        }

        [TestMethod]
        public void variable_and_model_operands()
        {
            var testSetUp = new TestSetUp(
                "{{var x=200}}{{x+Amount}}",
                new {Amount = 100},
                "300",10, TimeSpan.FromSeconds(30));

            HanjoTestHelper.Test(testSetUp);
        }
       // [TestMethod]
        public void using_basic_types_inline()
        {
            var testSetUp = new TestSetUp(
                "{{float.Parse('3.141592654')}}",
                new { Amount = 100 },
                "3.141593");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void conditional_with_variable_and_model_operands1()
        {
            var testSetUp = new TestSetUp(
                "{{var x=200}}{{if x>Amount}}show me{{else}}dont{{/if}}",
                new {Amount = 100},
                "show me");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void conditional_with_variable_and_model_operands2()
        {
            var testSetUp = new TestSetUp(
                "{{var x=50}}{{if x>Amount}}show me{{else}}dont{{/if}}",
                new {Amount = 100},
                "dont");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void inline_arithmetic_operation()
        {
            var testSetUp = new TestSetUp(
                "{{100+200+300}}",
                new {Amount = 100},
                "600");

            HanjoTestHelper.Test(testSetUp);
        }

      

        [TestMethod]
        public void getting_dates()
        {
            var testSetUp = new TestSetUp(
                "In the year {{DateTime.Now.Year}}, its so easy to forget everything",
                new {Amount = 100},
                "In the year " + DateTime.Now.Year + ", its so easy to forget everything");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void use_of_inline_variables()
        {
            var testSetUp = new TestSetUp(
                "{{var x=200}}{{var y=50}}{{x*y}}",
                new {Amount = 100},
                "10000");

            HanjoTestHelper.Test(testSetUp);
        }


        [TestMethod]
        public void injecting_html_style_using_variable()
        {
            var testSetUp = new TestSetUp(
                @"{{var divStyle=display:none}}
                  <div style=""{{divStyle}}"">Hey!I'm invinsible</div>",
                new {},
                @"<div style=""display:none"">Hey!I'm invinsible</div>");

            HanjoTestHelper.Test(testSetUp, true, true);
        }

        [TestMethod]
        public void basic_interpolation()
        {
            var testSetUp = new TestSetUp(
                "Name {{Name}}",
                new {Name = "Samuel"},
                "Name Samuel");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void basic_if_else_statement()
        {
            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{else}}yo!{{/if}}",
                new {IsGood = true},
                "I will do");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void multiline_if_else_statement()
        {
            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}
                      I will do
                  {{else}}
                      yo!
                  {{/if}}",
                new {IsGood = true},
                "I will do");

            HanjoTestHelper.Test(testSetUp, true, true);
        }


        [TestMethod]
        public void basic_if_else_statement2()
        {
            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{else}}yo!{{/if}}",
                new {IsGood = false},
                "yo!");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void simple_if_statement()
        {
            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{/if}}",
                new {IsGood = false},
                "");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void simple_if_statement2()
        {
            var testSetUp = new TestSetUp(
                @"{{if IsGood == true}}I will do{{/if}}",
                new {IsGood = true},
                "I will do");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void list_of_items_with_index()
        {
            var testSetUp = new TestSetUp(
                @"{{item in MyList at index}}<div>no.{{index}}:{{item}}</div>{{/item}}",
                new {MyList = new List<string> {"a", "b", "w"}},
                "<div>no.0:a</div><div>no.1:b</div><div>no.2:w</div>");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void list_of_items_without_index()
        {
            var testSetUp = new TestSetUp(
                @"{{item in MyList}}<div>{{item}}</div>{{/item}}",
                new {MyList = new List<string> {"a", "b", "w"}},
                "<div>a</div><div>b</div><div>w</div>");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void list_of_items_with_index_and_inline_variable()
        {
            var testSetUp = new TestSetUp(
                @"{{var t=100}}{{item in MyList at index}}<div>no.{{index+t}}:{{item}}</div>{{/item}}",
                new {MyList = new List<string> {"a", "b", "w"}},
                "<div>no.100:a</div><div>no.101:b</div><div>no.102:w</div>");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void direct_inline_templating()
        {
            var testSetUp = new TestSetUp(
                @"{{10}}",
                new {},
                "10");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void using_variables_and_model_as_opearnds()
        {
            var testSetUp = new TestSetUp(
                @"{{var y=Amount}}{{var x=200}}{{y+x}} is good",
                new {Amount = 300},
                "500 is good");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void using_variables_and_model_as_opearnds2()
        {
            var testSetUp = new TestSetUp(
                @"{{var x=200}}{{var y=Amount}}{{y+x}} is good",
                new {Amount = 300},
                "500 is good");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void list_of_items_with_index_and_inline_variable2()
        {
            var testSetUp = new TestSetUp(
                @"{{var t=100}}{{item in MyList}}<div>{{t}}{{item}}</div>{{/item}}",
                new {MyList = new List<string> {"a", "b", "w"}},
                "<div>100a</div><div>100b</div><div>100w</div>");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void both_list_of_items_with_index_and_without_index()
        {
            var testSetUp = new TestSetUp(
                @"{{item in MyList}}<div>{{item}}</div>{{/item}}" +
                @"{{item in MyList at index}}<div>no.{{index}}:{{item}}</div>{{/item}}",
                new {MyList = new List<string> {"a", "b", "w"}},
                "<div>a</div><div>b</div><div>w</div>" +
                "<div>no.0:a</div><div>no.1:b</div><div>no.2:w</div>");

            HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void string_concatenation_of_model_properties()
        {
            var testSetUp = new TestSetUp(
                @"You.Name+Name:{{You.Name + Name}}",
                new
                {
                    Name = "Sam",
                    You = new
                    {
                        Name = "Sam2"
                    }
                },
                "You.Name+Name:Sam2Sam");

            var result = HanjoTestHelper.Test(testSetUp, false);
        }

        [TestMethod]
        public void model_only_operand_addition()
        {
            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount + Amount}}",
                new
                {
                    Amount = 3,
                    You = new
                    {
                        Amount = 5
                    }
                },
                "You.Amount+Amount:8");

            var result = HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void model_only_operand_subtraction()
        {
            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount - Amount}}",
                new
                {
                    Amount = 3,
                    You = new
                    {
                        Amount = 5
                    }
                },
                "You.Amount+Amount:2");

            var result = HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void model_only_operand_division()
        {
            var testSetUp = new TestSetUp(
                @"You.Amount+Amount:{{You.Amount / Amount}}",
                new
                {
                    Amount = 2,
                    You = new
                    {
                        Amount = 10
                    }
                },
                "You.Amount+Amount:5");

            var result = HanjoTestHelper.Test(testSetUp);
        }

        [TestMethod]
        public void nested_if_statements_onelevel()
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
                new {IsGood = false, IsNotGood = false},
                "I will do");

            HanjoTestHelper.Test(testSetUp, true, true);
        }

        [TestMethod]
        public void nested_if_statements_onelevel2()
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
                     {{/if}}that                       
                  {{/if}}
                 ",
                new {IsGood = false, IsNotGood = false},
                @"I will do                     that");

            HanjoTestHelper.Test(testSetUp, true, true, true);
        }

        //TODO THIS TEST SEEM TO NEVER RETURN
        // [TestMethod]
        public void LoadTest_sample()
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
                        IsGood = false
                    },
                    MyList = new List<string> {"a", "b", "c"}
                }, "", totalNumberOfIteration);

            HanjoTestHelper.Test(testSetUp, false);
        }
    }
}
