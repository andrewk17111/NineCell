﻿namespace NineCell;

internal static class AnsiUtils
{
    public const string reset = "\u001b[0m";
    public const string bold = "\u001b[1m";
    public const string intense = bold;
    public const string faint = "\u001b[2m";
    public const string dim = faint;
    public const string dark = faint;
    public const string italic = "\u001b[3m";
    public const string underline = "\u001b[4m";
    public const string slowblink = "\u001b[5m";
    public const string fastblink = "\u001b[6m";
    public const string reverse = "\u001b[7m";
    public const string invert = reverse;
    public const string conceal = "\u001b[8m";
    public const string hide = conceal;
    public const string crossed = "\u001b[9m";
    public const string strike = crossed;
    public const string dunderline = "\u001b[21m";
    public const string rintensity = "\u001b[22m";
    public const string ritalic = "\u001b[23m";
    public const string runderline = "\u001b[24m";
    public const string rblink = "\u001b[25m";
    public const string rreverse = "\u001b[27m";
    public const string rinvert = rreverse;
    public const string reveal = "\u001b[28m";
    public const string rcrossed = "\u001b[29m";
    public const string rstrike = rcrossed;
    public const string fgblack = "\u001b[30m";
    public const string fgred = "\u001b[31m";
    public const string fggreen = "\u001b[32m";
    public const string fgyellow = "\u001b[33m";
    public const string fgblue = "\u001b[34m";
    public const string fgmagenta = "\u001b[35m";
    public const string fgcyan = "\u001b[36m";
    public const string fgwhite = "\u001b[37m";
    public const string fgreset = "\u001b[39m";
    public const string bgblack = "\u001b[40m";
    public const string bgred = "\u001b[41m";
    public const string bggreen = "\u001b[42m";
    public const string bgyellow = "\u001b[43m";
    public const string bgblue = "\u001b[44m";
    public const string bgmagenta = "\u001b[45m";
    public const string bgcyan = "\u001b[46m";
    public const string bgwhite = "\u001b[47m";
    public const string bgreset = "\u001b[49m";
    public const string overline = "\u001b[53m";
    public const string roverline = "\u001b[55m";
    public const string fggrey = "\u001b[90m";
    public const string fggray = fggrey;
    public const string fgbrightred = "\u001b[91m";
    public const string fgbrightgreen = "\u001b[92m";
    public const string fgbrightyellow = "\u001b[93m";
    public const string fgbrightblue = "\u001b[94m";
    public const string fgbrightmagenta = "\u001b[95m";
    public const string fgbrightcyan = "\u001b[96m";
    public const string fgbrightwhite = "\u001b[97m";
    public const string bggrey = "\u001b[100m";
    public const string bggray = bggrey;
    public const string bgbrightred = "\u001b[101m";
    public const string bgbrightgreen = "\u001b[102m";
    public const string bgbrightyellow = "\u001b[103m";
    public const string bgbrightblue = "\u001b[104m";
    public const string bgbrightmagenta = "\u001b[105m";
    public const string bgbrightcyan = "\u001b[106m";
    public const string bgbrightwhite = "\u001b[107m";
}
