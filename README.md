# aws-neptune-explorer

Install dotnet core  
https://www.microsoft.com/net/learn/get-started/linux/ubuntu16-04  

git clone https://github.com/taherchhabra/aws-neptune-explorer.git  
cd aws-neptune-explorer  

update the NeptuneEndpoint in appsettings.json   

dotnet restore  
dotnet run  

To run it as daemon  
sudo nano /etc/systemd/system/GremlinExplorer.service   

[Unit]  
Description=GremlinExplorer.service   

[Service]  
WorkingDirectory=/home/ubuntu/aws-neptune-explorer/bin/Debug/netcoreapp2.0/publish  
ExecStart=/usr/bin/dotnet /home/ubuntu/aws-neptune-explorer/bin/Debug/netcoreapp2.0/publish/aws-neptune-explorer.dll  
Restart=always  
RestartSec=10 # Restart service after 10 seconds if dotnet service crashes  
SyslogIdentifier=GremlinExplorer  
User=root  
Environment=ASPNETCORE_ENVIRONMENT=Development  
  
[Install]  
WantedBy=multi-user.target  


sudo systemctl enable GremlinExplorer.service  
sudo systemctl start GremlinExplorer.service  
sudo systemctl stop GremlinExplorer.service  
sudo systemctl status GremlinExplorer.service  
  
sudo journalctl -fu GremlinExplorer.service    


