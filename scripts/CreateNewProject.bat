color 3
@echo off

dotnet new -i .template\Memoyu.Core.WebApi.Template.1.0.0.nupkg

set /p OP=Please set your project name(for example:Memoyu.Core):

md .NewProject

cd .NewProject

dotnet new memoyucoretpl -n %OP%

cd ../

echo "----------------Create Successfully!! ^ please see the folder .NewProject----------------"

dotnet new -u Memoyu.Core.WebApi.Template

echo "----------------Delete Template Successfully!----------------"

pause