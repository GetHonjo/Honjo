using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Blade.Tests
{
    public class TestHelper
    {
       
        public static string sample0 = @"
                        Name {{Name}} 
                        Amount : {{Amount}} 
                        IsGood : {{IsGood}} 

                        You.Name : {{You.Name}} 
                        You.Amount : {{You.Amount}} 
                        You.IsGood : {{You.IsGood}} 

                        Amount + Amount : {{Amount+Amount}} 
                        Amount - You.Amount : {{Amount - You.Amount}}
                        You.Amount - Amount : {{You.Amount - Amount}}
                        You.Name + Name: {{You.Name + Name}} 
";
        public static string sample1 = @"
{{if IsGood == true}} 
  I will do 
{{else}}  
 yo!  
{{/if}}

{{if You.IsGood == true}}
 I will do   
{{else}} 
 yo!  
{{/if}}

{{if You.Amount > Amount}} 
 I will do   
{{else}} 
 yo!  
{{/if}}

{{if You.Amount < Amount}} 
 I will do  
{{else}}    
 yo!  
{{/if}}

{{if You.Amount<Amount}} 
 I will do  
{{else}}    
 yo!  
{{/if}}
";
        public static string sample2 = @"{{item in MyList at index}} <div>no.{{index}} : {{item}}</div> {{/item}}";
        public static string sample3 = @"{{item 
                           in 
                           MyList 
                           at 
                           index
                        }} 

                                 <div>no.{{index}} : {{item}}</div> 
                         {{/item}}";


        public static string sample4 = " You.Name + Name: {{You.Name + Name}} ";
       

    }
}