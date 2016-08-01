del *.nupkg
nuget pack MyChy.Frame.Common\MyChy.Frame.Common.csproj  -Prop Configuration=Release
nuget pack MyChy.Frame.Common.Redis\MyChy.Frame.Common.Redis.csproj  -Prop Configuration=Release
nuget pack MyChy.Frame.Common.Data\MyChy.Frame.Common.Data.csproj  -Prop Configuration=Release
nuget push *.nupkg -s http://nuget.chyenc.com 8bfc20c1-83a6-4d0f-a48e-209a8dda7ad2
