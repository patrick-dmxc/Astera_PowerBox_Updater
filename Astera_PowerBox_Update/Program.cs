// See https://aka.ms/new-console-template for more information
using TitanPowerBoxUpdate;
try
{
    Console.WriteLine("Hello, World!");
    string ip = "2.236.204.11";
    string firmware = "C:\\Users\\Q-Light\\Documents\\Firmwares\\Astera\\Titan PowerBox\\titan_firmware 1_21.frm";
    await Communication.SendUpdate(ip, firmware);
    await Communication.SendReboot(ip);
    Console.WriteLine("Done!");
    Console.ReadLine();
}
catch(Exception e)
{

}