<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template="{{var x=200}}{{if x<Amount}}show me{{else}}dont{{/if}}";
var model=new {Amount = 100};
var result=new Honjo().Compile(template, model );
result.Dump();