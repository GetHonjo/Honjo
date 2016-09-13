<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
{{var divStyle=font-family: Verdana,sans-serif;}}
{{item in MyList at index}}
  <div style=""{{divStyle}}"">no.{{index}}:{{item}}</div>
{{/item}}
";
var model=new {MyList = new List<string> {"a", "b", "w"}};
var result=new Honjo().Compile(template, model );
result.Dump();