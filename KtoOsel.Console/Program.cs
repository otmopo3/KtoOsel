using System;


namespace KtoOsel
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var game = new GameLogicVm ();
			game.PlayGameToEndCommand.Execute (null);
			Console.WriteLine (game.GameLog);

		}
	}
}
