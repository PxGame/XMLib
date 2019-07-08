@echo off

cd %~dp0

set PROTOC=protoc.exe
set SRC=.\proto\
set DST=.\build\

echo build from %SRC% to %DST%

echo clean %DST%
if exist %DST% (
    RD /S /Q %DST%
)
MD %DST%

echo ======= start gen =======
for %%f in (%SRC%*.proto) do (
    echo %%f
    %PROTOC% --proto_path=%SRC% --csharp_out=%DST% %%f
)
echo ======== end gen ========

pause

::protoc --proto_path=src --csharp_out=build/gen --csharp_opt=file_extension=.g.cs,base_namespace=Example src/foo.proto