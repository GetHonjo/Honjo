<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
@"
{{var divStyle=display:none}}
<div style=""{{divStyle}}"">Hey!I'm invinsible</div>
";
var model=new {};
var result=new Honjo().Compile(template, model );
result.Dump();