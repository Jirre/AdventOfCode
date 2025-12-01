using System;

namespace AdventOfCode.Y2025;

class SplashScreenImpl : SplashScreen {

    public void Show() {

        var color = Console.ForegroundColor;
        Write(0xcc00, false, "           ▄█▄ ▄▄█ ▄ ▄ ▄▄▄ ▄▄ ▄█▄  ▄▄▄ ▄█  ▄▄ ▄▄▄ ▄▄█ ▄▄▄\n           █▄█ █ █ █ █ █▄█ █ █ █   █ █ █▄ ");
            Write(0xcc00, false, " █  █ █ █ █ █▄█\n           █ █ █▄█ ▀▄▀ █▄▄ █ █ █▄  █▄█ █   █▄ █▄█ █▄█ █▄▄  int y = 2025;\n           ");
            Write(0xcc00, false, " \n           ");
            Write(0x666666, false, "         ____           *                               \n           ________/O___\\__________|_______");
            Write(0x666666, false, "_________________  ");
            Write(0xcccccc, false, " 1 ");
            Write(0x666666, false, "**\n           ");
            Write(0x333333, false, "   _______||_________                                   \n              | _@__ || _o_    |           ");
            Write(0x333333, false, "                   ");
            Write(0x666666, false, " 2\n           ");
            Write(0x333333, false, "   |_&_%__||_oo__^=_[                                   \n                                           ");
            Write(0x333333, false, "                   ");
            Write(0x666666, false, " 3\n                                                                   \n                             ");
            Write(0x666666, false, "                                  4\n                                                                ");
            Write(0x666666, false, "   \n                                                               5\n                               ");
            Write(0x666666, false, "                                    \n                                                               ");
            Write(0x666666, false, "6\n                                                                   \n                              ");
            Write(0x666666, false, "                                 7\n                                                                 ");
            Write(0x666666, false, "  \n                                                               8\n                                ");
            Write(0x666666, false, "                                   \n                                                               9");
            Write(0x666666, false, "\n                                                                   \n                               ");
            Write(0x666666, false, "                               10\n                                                                  ");
            Write(0x666666, false, " \n                                                              11\n                                 ");
            Write(0x666666, false, "                                  \n                                                              12\n");
            Write(0x666666, false, "           \n");
            
        Console.ForegroundColor = color;
        Console.WriteLine();
    }

    private static void Write(int rgb, bool bold, string text){
       Console.Write($"\u001b[38;2;{(rgb>>16)&255};{(rgb>>8)&255};{rgb&255}{(bold ? ";1" : "")}m{text}");
    }
}