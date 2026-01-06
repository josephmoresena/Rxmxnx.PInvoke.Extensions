namespace Rxmxnx.PInvoke.ApplicationTest;

public static class Utf8ConcatenationHelper
{
	public static void CStringBuildingFeature()
	{
		Span<Int32> interruptions = stackalloc Int32[8];
		CStringBuilder bohemianRhapsody = new();
		bohemianRhapsody.AppendLine("I'm just a poor boy, I need no sympathy "u8)
		                .AppendLine("Because I'm easy come, easy go"u8).AppendLine("Little high, little low "u8)
		                .AppendLine("Any way the wind blows doesn't really matter to me, to me"u8).AppendLine()
		                .AppendLine("Mama, just killed a man "u8)
		                .AppendLine("Put a gun against his head, pulled my trigger, now he's dead "u8)
		                .AppendLine("Mama, life had just begun "u8)
		                .AppendLine("But now I've gone and thrown it all away "u8)
		                .AppendLine("Mama, ooh, didn't mean to make you cry "u8)
		                .AppendLine("If I'm not back again this time tomorrow "u8)
		                .AppendLine("Carry on, carry on as if nothing really matters"u8).AppendLine()
		                .AppendLine("Too late, my time has come "u8)
		                .AppendLine("Sends shivers down my spine, body's aching all the time "u8)
		                .AppendLine("Goodbye, everybody, I've got to go "u8)
		                .AppendLine("Gotta leave you all behind and face the truth "u8).Append("Mama, ooh"u8);
		interruptions[0] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine().AppendLine("I don't wanna die "u8)
		                .AppendLine("I sometimes wish I'd never been born at all"u8).AppendLine()
		                .AppendLine("I see a little silhouetto of a man "u8);
		interruptions[1] = bohemianRhapsody.Length;

		bohemianRhapsody.AppendLine("Thunderbolt and lightning, very, very frightening me "u8);
		interruptions[2] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine().AppendLine("But I'm just a poor boy, nobody loves me "u8);
		interruptions[3] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine().AppendLine("Easy come, easy go, will you let me go? "u8);
		interruptions[4] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine("Let me go!"u8);
		interruptions[5] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine("Let me go!"u8);
		interruptions[6] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine("Never, never, never, never let me go"u8);
		interruptions[7] = bohemianRhapsody.Length;
		bohemianRhapsody.AppendLine().AppendLine("No, no, no, no, no, no, no"u8).AppendLine()
		                .AppendLine("Oh, mamma mia, mamma mia "u8).AppendLine("Mamma mia, let me go "u8)
		                .AppendLine("Beelzebub has a devil put aside for me, for me, for me"u8).AppendLine()
		                .AppendLine("So you think you can stone me and spit in my eye? "u8)
		                .AppendLine("So you think you can love me and leave me to die? "u8)
		                .AppendLine("Oh, baby, can't do this to me, baby "u8)
		                .AppendLine("Just gotta get out, just gotta get right outta here"u8).AppendLine()
		                .AppendLine("Ooh "u8).AppendLine("Ooh, yeah, ooh, yeah "u8)
		                .AppendLine("Nothing really matters, anyone can see "u8).AppendLine("Nothing really matters "u8)
		                .Append("Nothing really matters to me"u8);

		bohemianRhapsody.Insert(interruptions[7] - CString.NewLine.Length, ")"u8);
		bohemianRhapsody.Insert(interruptions[6], "("u8);
		bohemianRhapsody.Insert(interruptions[6], "Will not let you go "u8);
		bohemianRhapsody.Insert(interruptions[6] - CString.NewLine.Length, ")"u8);
		bohemianRhapsody.Insert(interruptions[5], "("u8);
		bohemianRhapsody.Insert(interruptions[5], "Will not let you go ("u8);

		bohemianRhapsody.Insert(interruptions[4], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[4], "سْمِ ٱللَّٰهِ"u8);
		bohemianRhapsody.Insert(interruptions[4], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[4], "Let him go!"u8);
		bohemianRhapsody.Insert(interruptions[4] + 11, ")"u8);
		bohemianRhapsody.Insert(interruptions[4], "("u8);
		bohemianRhapsody.Insert(interruptions[4], "We will not let you go "u8);
		bohemianRhapsody.Insert(interruptions[4], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[4], "سْمِ ٱللَّٰهِ"u8);
		bohemianRhapsody.Insert(interruptions[4], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[4], "Let him go!"u8);
		bohemianRhapsody.Insert(interruptions[4] + 11, ")"u8);
		bohemianRhapsody.Insert(interruptions[4], "("u8);
		bohemianRhapsody.Insert(interruptions[4], "No, we will not let you go "u8);

		bohemianRhapsody.Insert(interruptions[3], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[3], "Spare him his life from this monstrosity"u8);
		bohemianRhapsody.Insert(interruptions[3], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[3], "He's just a poor boy from a poor family"u8);

		bohemianRhapsody.Insert(interruptions[2], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[2], "Galileo, Galileo Figaro, magnifico"u8);
		bohemianRhapsody.Insert(interruptions[2] + 9, "(Galileo) "u8);
		bohemianRhapsody.Insert(interruptions[2], "(Galileo) "u8);

		bohemianRhapsody.Insert(interruptions[1], CString.NewLine);
		bohemianRhapsody.Insert(interruptions[1], "Scaramouche, Scaramouche, will you do the Fandango?"u8);

		bohemianRhapsody.Insert(interruptions[0], " (any way the wind blows)"u8);

		bohemianRhapsody.Insert(0, CString.NewLine);
		bohemianRhapsody.Insert(0, "Open your eyes, look up to the skies and see"u8);
		bohemianRhapsody.Insert(0, CString.NewLine);
		bohemianRhapsody.Insert(0, "Caught in a landslide, no escape from reality"u8);
		bohemianRhapsody.Insert(0, CString.NewLine);
		bohemianRhapsody.Insert(0, "Is this the real life? Is this just fantasy?"u8);

		Console.WriteLine(bohemianRhapsody.ToString());
	}
}