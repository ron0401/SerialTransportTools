# SerialTransportTools

## Abstract

This tool converts serial communication to UDP communication and UDP communication to serial communication.

For example, by converting USB-serial communication connected to a PC to UDP communication, it can be received or captured by another machine.

It currently includes the following two tools:

* .NET Core CUI (multi-platform)
* ESP32

## sttnet
.NET Core Tools.

It's converts serial communication to UDP communication or other serial port.

### Environment
sttnet needs dotnet sdk for builds or needs runtime for execute.

* [for ubuntu](https://docs.microsoft.com/ja-jp/dotnet/core/install/linux-ubuntu)
* [Windows 10](https://docs.microsoft.com/ja-jp/dotnet/core/install/windows?tabs=netcore31)

### Example
If you want to send UDP for 192.168.1.2:50001 from serial port "COM5".
```
sttnet -p COM5 -u 192.168.1.2:50001
```
