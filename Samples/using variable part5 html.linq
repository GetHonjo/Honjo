<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
{{var divStyle=<span>I just want to ask if you cann do something</span>}}
{{divStyle}}
";
var model=new {};
var result=new Honjo().Compile(template, model );
result.Dump();