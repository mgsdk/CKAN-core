using System;

public class Utilities
{
    public static bool IsUnix
    {
        get
        {
            // Magic numbers ahoy! This arcane incantation was found
            // in a Unity help-page, which was found on a scroll,
            // which was found in an urn that dated back to Mono 2.0.
            // It documents singular numbers of great power.
            //
            // "And lo! 'pon the 4, 6, and 128 the penguin shall
            // come, and it infiltrate dominate from the smallest phone to
            // the largest cloud."
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
        }
    }
}
