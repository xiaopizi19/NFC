rem �л���.protoЭ�����ڵ�Ŀ¼  
cd E:\MaJiang\Proto\protoc-gen-lua\plugin\proto 
rem ����ǰ�ļ����е�����Э���ļ�ת��Ϊlua�ļ�  
for %%i in (*.proto) do (    
echo %%i  
"..\..\protoc.exe" --plugin=protoc-gen-lua="..\..\plugin\protoc-gen-lua.bat" --lua_out=. %%i  
  
)  
echo end  
pause  