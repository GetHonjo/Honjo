<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
Total = {{ 
          Amount+
		  You.Amount +
		  You.You.Amount +
		  You.You.You.Amount 
}}
";
var model= new  {
                    Amount = 2,
                    You = new
                    {
                        Amount = 3,
						You = new
                            {
                               Amount = 4,
							   You = new
                                      {
                                         Amount = 5
                                       }
                             }
                    }
                };
				
var result=new Honjo().Compile(template, model );
result.Dump();