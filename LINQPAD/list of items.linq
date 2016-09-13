<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
{{item in MyList}}
  <div>{{item}}</div>
{{/item}}
";
var model=new {MyList = new List<string> {"a", "b", "w"}};
var result=new Honjo().Compile(template, model );
result.Dump();