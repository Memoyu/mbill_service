color 3
@echo off

dotnet new -i .template\Memoyu.Mbill.WebApi.Template.1.0.0.nupkg

set /p OP=Please set your project name(for example:Memoyu.Mbill):

md .NewProject

cd .NewProject

dotnet new memoyucoretpl -n %OP%

cd ../

echo "----------------Create Successfully!! ^ please see the folder .NewProject----------------"

dotnet new -u Memoyu.Mbill.WebApi.Template

echo "----------------Delete Template Successfully!----------------"

pause