<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
{{var divStyle=<span>I just want to ask if you cann do something</span>}}
{{item in numbers}}
   {{item}}. {{divStyle}}  
{{/item}}
";
var model=new {numbers=new List<string>(){"a","b","a","b"}};
var result=new Honjo().Compile(template, model );
result.Dump();