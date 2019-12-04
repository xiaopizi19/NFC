# NFC

3.10.0-1062.1.1.el7.x86_64
CentOS 7.7.1908
mysql ZFCzfc4894.
https://meet.hrglobe.cn/liujinjin/4D3F0CK8

应用层
传输层
网络层
链路层
物理层

挂载共享文件夹失败 重新挂载
查看
vmware-hgfsclient
挂载
/usr/bin/vmhgfs-fuse .host:/ /mnt/win/ -o subtype=vmhgfs-fuse,allow_other

查看当前端口和进程对应关系
netstat -plnt
top
shift + c 按cpu 占用排序
shitf + m 按内存排序

systemctl start mysqld
systemctl stop mysqld
systemctl status mysqld.service

cd etc/sysconfig/network-scripts/
vim ifcfg-ens33
netstat -an | find "LISTEN"
netsh interface portproxy add v4tov4 listenport
=49899 listenaddress=0.0.0.0 connectaddress=127.0.0.1 connectport=49898 protocol=tcp

netsh interface portproxy delete v4tov4 listenport=49899 listenaddress=0.0.0.0
-addr 192.168.20.87:49898
g++ -lm  -g -o a.out luaClass.cpp /home/lua/lua-5.3.3/src/liblua.a -ldl

 -addr=192.168.20.87:49898


systemctl start redis.service #启动redis服务器

systemctl stop redis.service #停止redis服务器

systemctl restart redis.service #重新启动redis服务器

systemctl status redis.service #获取redis服务器的运行状态

systemctl enable redis.service #开机启动redis服务器

systemctl disable redis.service #开机禁用redis服务器

程序员的自我修养
程序指令和程序数据
模板和泛型编程
模板的实例化

 2018.1.2f1
