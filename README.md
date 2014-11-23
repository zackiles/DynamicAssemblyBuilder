##Dynamic Assembly Builder##

A really old project, but somebody will likely find it useful. This library is for code injection into 32/86x binaries. Commonly known as "RunPE" this enables a secondary .NET file to be injected into any other host binary. When the final binary is run, the first .NET code silently injections itself into the memory of another process. This technique is commonly used to beat file-sginature scans from anti-viruses.

Don't ask me how it works, it's been many years since I wrote it and I only rarely used VB instead of C# back then.