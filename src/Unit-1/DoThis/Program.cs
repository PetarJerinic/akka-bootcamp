using Akka.Actor;

namespace WinTail
{
	#region Program

	internal class Program
	{
		public static ActorSystem MyActorSystem;

		private static void Main(string[] args)
		{
			// initialize MyActorSystem
			MyActorSystem = ActorSystem.Create("MyActorSystem");

			// make tailCoordinatorActor
			var tailCoordinatorProps = Props.Create(() => new TailCoordinatorActor());
			var tailCoordinatorActor = MyActorSystem.ActorOf(tailCoordinatorProps,
				"tailCoordinatorActor");

			// time to make your first actors!
			var consoleWriterProps = Props.Create<ConsoleWriterActor>();
			var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterProps, "consoleWriterActor");

			var fileValidatorActorProps = Props.Create<FileValidatorActor>(consoleWriterActor);
			//var validationActorProps = Props.Create(() => new FileValidatorActor(consoleWriterActor));
			var validationActor = MyActorSystem.ActorOf(fileValidatorActorProps, "validationActor");

			var consoleReaderProps = Props.Create<ConsoleReaderActor>();
			var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderProps, "consoleReaderActor");

			// tell console reader to begin
			consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

			// blocks the main thread from exiting until the actor system is shut down
			MyActorSystem.WhenTerminated.Wait();
		}
	}

	#endregion Program
}