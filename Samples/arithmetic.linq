<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
You.Amount/Amount = {{You.Amount / Amount}}
";
var model= new  {
                    Amount = 2,
                    You = new
                    {
                        Amount = 10
                    }
                };
var result=new Honjo().Compile(template, model );
result.Dump();