setlocal

@rem enter this directory
echo Generating Draw Poker protos

cd /d %~dp0

set TOOLS_PATH=..\..\..\protos\Tools
rem set G_PATH=..\..\..\protos\Google
set PROTO_FILES=./DrawPokerMessages.proto

"%TOOLS_PATH%\protoc.exe" -I. -I..\..\..\Protos -I..\..\GameCommon --csharp_out . %PROTO_FILES% 

endlocal