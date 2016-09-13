<Query Kind="Statements">
  <NuGetReference Prerelease="true">Honjo</NuGetReference>
  <Namespace>HonjoLib</Namespace>
</Query>

var template=
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
";
var model= new {IsGood = false, IsNotGood = false};
				
var result=new Honjo().Compile(template, model );
result.Dump();