#if !NET9_0_OR_GREATER
using System;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	public static class Utf8ConcatenationHelper
	{
		public static void CStringBuildingFeature()
		{
			Span<Int32> interruptions = stackalloc Int32[8];
			CStringBuilder bohemianRhapsody = new();
			bohemianRhapsody.AppendLine("I'm just a poor boy, I need no sympathy ")
			                .AppendLine("Because I'm easy come, easy go").AppendLine("Little high, little low ")
			                .AppendLine("Any way the wind blows doesn't really matter to me, to me").AppendLine()
			                .AppendLine("Mama, just killed a man ")
			                .AppendLine("Put a gun against his head, pulled my trigger, now he's dead ")
			                .AppendLine("Mama, life had just begun ")
			                .AppendLine("But now I've gone and thrown it all away ")
			                .AppendLine("Mama, ooh, didn't mean to make you cry ")
			                .AppendLine("If I'm not back again this time tomorrow ")
			                .AppendLine("Carry on, carry on as if nothing really matters").AppendLine()
			                .AppendLine("Too late, my time has come ")
			                .AppendLine("Sends shivers down my spine, body's aching all the time ")
			                .AppendLine("Goodbye, everybody, I've got to go ")
			                .AppendLine("Gotta leave you all behind and face the truth ").Append("Mama, ooh");
			interruptions[0] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine().AppendLine("I don't wanna die ")
			                .AppendLine("I sometimes wish I'd never been born at all").AppendLine()
			                .AppendLine("I see a little silhouetto of a man ");
			interruptions[1] = bohemianRhapsody.Length;

			bohemianRhapsody.AppendLine("Thunderbolt and lightning, very, very frightening me ");
			interruptions[2] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine().AppendLine("But I'm just a poor boy, nobody loves me ");
			interruptions[3] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine().AppendLine("Easy come, easy go, will you let me go? ");
			interruptions[4] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine("Let me go!");
			interruptions[5] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine("Let me go!");
			interruptions[6] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine("Never, never, never, never let me go");
			interruptions[7] = bohemianRhapsody.Length;
			bohemianRhapsody.AppendLine().AppendLine("No, no, no, no, no, no, no").AppendLine()
			                .AppendLine("Oh, mamma mia, mamma mia ").AppendLine("Mamma mia, let me go ")
			                .AppendLine("Beelzebub has a devil put aside for me, for me, for me").AppendLine()
			                .AppendLine("So you think you can stone me and spit in my eye? ")
			                .AppendLine("So you think you can love me and leave me to die? ")
			                .AppendLine("Oh, baby, can't do this to me, baby ")
			                .AppendLine("Just gotta get out, just gotta get right outta here").AppendLine()
			                .AppendLine("Ooh ").AppendLine("Ooh, yeah, ooh, yeah ")
			                .AppendLine("Nothing really matters, anyone can see ").AppendLine("Nothing really matters ")
			                .Append("Nothing really matters to me");

			bohemianRhapsody.Insert(interruptions[7] - Environment.NewLine.Length, ")");
			bohemianRhapsody.Insert(interruptions[6], "(");
			bohemianRhapsody.Insert(interruptions[6], "Will not let you go ");
			bohemianRhapsody.Insert(interruptions[6] - Environment.NewLine.Length, ")");
			bohemianRhapsody.Insert(interruptions[5], "(");
			bohemianRhapsody.Insert(interruptions[5], "Will not let you go (");

			bohemianRhapsody.Insert(interruptions[4], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[4], "سْمِ ٱللَّٰهِ");
			bohemianRhapsody.Insert(interruptions[4], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[4], "Let him go!");
			bohemianRhapsody.Insert(interruptions[4] + 11, ")");
			bohemianRhapsody.Insert(interruptions[4], "(");
			bohemianRhapsody.Insert(interruptions[4], "We will not let you go ");
			bohemianRhapsody.Insert(interruptions[4], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[4], "سْمِ ٱللَّٰهِ");
			bohemianRhapsody.Insert(interruptions[4], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[4], "Let him go!");
			bohemianRhapsody.Insert(interruptions[4] + 11, ")");
			bohemianRhapsody.Insert(interruptions[4], "(");
			bohemianRhapsody.Insert(interruptions[4], "No, we will not let you go ");

			bohemianRhapsody.Insert(interruptions[3], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[3], "Spare him his life from this monstrosity");
			bohemianRhapsody.Insert(interruptions[3], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[3], "He's just a poor boy from a poor family");

			bohemianRhapsody.Insert(interruptions[2], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[2], "Galileo, Galileo Figaro, magnifico");
			bohemianRhapsody.Insert(interruptions[2] + 9, "(Galileo) ");
			bohemianRhapsody.Insert(interruptions[2], "(Galileo) ");

			bohemianRhapsody.Insert(interruptions[1], Environment.NewLine);
			bohemianRhapsody.Insert(interruptions[1], "Scaramouche, Scaramouche, will you do the Fandango?");

			bohemianRhapsody.Insert(interruptions[0], " (any way the wind blows)");

			bohemianRhapsody.Insert(0, Environment.NewLine);
			bohemianRhapsody.Insert(0, "Open your eyes, look up to the skies and see");
			bohemianRhapsody.Insert(0, Environment.NewLine);
			bohemianRhapsody.Insert(0, "Caught in a landslide, no escape from reality");
			bohemianRhapsody.Insert(0, Environment.NewLine);
			bohemianRhapsody.Insert(0, "Is this the real life? Is this just fantasy?");

			Console.WriteLine(bohemianRhapsody.ToString());
		}
	}
}
#endif