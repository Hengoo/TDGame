using System;
using System.IO;
using System.Threading;

namespace TDGame.Kommunikation
{
	/// <summary>Ermöglicht sicheren TCP Zugriff bei mehreren Threads</summary>
	public class LockedBinaryWriter : BinaryWriter
	{
		private Object monitor;

		/// <summary>Standartkonstruktor</summary>
		public LockedBinaryWriter()
			: base()
		{ }

		/// <summary>Konstruktor mit Parameterübergabe</summary>
		/// <param name="output">Übergibt den output direkt an den BinaryWriter</param>
		public LockedBinaryWriter(Stream output)
			: base(output)
		{
			monitor = new Object();
		}

		/// <summary>Sperrt dieses Objekt.</summary>
		public virtual void Lock()
		{
			Monitor.Enter(monitor);
		}

		/// <summary>Entsperrt dieses Objekt.</summary>
		public virtual void Unlock()
		{
			Monitor.Exit(monitor);
		}
	}
}